using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridPiece : MonoBehaviour
{
    public List<Block> blocks;
    public Block snapBlock;

    public enum State
    {
        Idle, Placed 
    }

    public State state;
    private void Start()
    {
        blocks = GetComponentsInChildren<Block>().ToList();
    }

    public bool SnapToClosestTarget()
    {
        if (state == State.Placed)
            return false;
        
        if (Physics.Raycast(snapBlock.transform.position, Vector3.down, out RaycastHit hit))
        {
            //Debug.Log(hit.collider.name);
            transform.SetParent(hit.collider.transform);
            // adjust the position if not snapped properly.
            Vector3 diff = snapBlock.transform.position - transform.position;
            transform.localPosition = Vector3.zero + Vector3.up - diff;
            state = State.Placed;
            // check all blocks covering the open ones
            List<Block> removeBlocks = new List<Block>();;
            foreach (var block in blocks)
            {
                if (Physics.Raycast(block.transform.position, Vector3.down, out RaycastHit blockHit))
                {
                    Block openBlock = blockHit.collider.GetComponent<Block>();
                    if (openBlock != null)
                    {
                        if (openBlock.Active)
                        {
                            openBlock.SetActive(false);    
                        }

                        if (openBlock.blockType == BlockType.Closed)
                        {
                            removeBlocks.Add(block);
                        }
                    }
                }
            }

            foreach (var removeBlock in removeBlocks)
            {
                // TODO: Show destroy particle here. JN

                removeBlock.RemoveMe();
            }

            return true;
        }

        return false;
    }

    public int CountBlocksInsideArea()
    {
        int count = 0;
        foreach (var block in blocks)
        {
            if (block.IsInsideArea())
            {
                count++;
            }
        }

        return count;
    }
    public int CountBlocksOutsideArea()
    {
        int count = 0;
        foreach (var block in blocks)
        {
            if (!block.IsInsideArea())
            {
                count++;
            }
        }

        return count;
    }
}
