using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockType blockType;
    private Renderer renderer;

    private Material idleMaterial;
    private Material placeableMaterial;
    private Material notPlaceableMaterial;

    [SerializeField]
    private bool active = false;

    public static Action<Vector3> OnDestroyMe;
    public bool Active => active;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        active = true;
    }

    public void SetActive(bool value)
    {
        active = value;
    }
    public bool IsInsideArea()
    {
        if (Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit))
        {
            Block block = hit.collider.GetComponent<Block>();
            if (block != null && block.blockType == BlockType.Open)
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveMe()
    {
        OnDestroyMe?.Invoke(transform.position);
        Destroy(gameObject);
    }
}
