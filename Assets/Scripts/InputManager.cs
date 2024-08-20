using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum State
    {
        Idle, Dragging 
    }

    public GridPiece selectedGridPiece;
    public Action<GridPiece> OnPieceSelected;
    public static Action<GridPiece> OnPieceReleased;
    public State state = State.Idle;
    private void Update()
    {
        if (Input.GetMouseButton(0) && selectedGridPiece == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GridPiece gridPiece = hit.collider.GetComponentInParent<GridPiece>();
                
                if (gridPiece != null && gridPiece.state == GridPiece.State.Idle)
                {
                    // set grid piece to selected
                    selectedGridPiece = gridPiece;
                    OnPieceSelected?.Invoke(selectedGridPiece);
                    state = State.Dragging;
                }
            }
        }

        // we released the mouse
        if (Input.GetMouseButtonUp(0))
        {
            // check if we have a grid piece selected
            if (selectedGridPiece != null)
            {
                // release it an snap it to the closest item

                if (selectedGridPiece.SnapToClosestTarget())
                {
                    
                    // if it is not touching anything, give it back to the grid piece holder
                    OnPieceReleased?.Invoke(selectedGridPiece);
                    // set selected grid piece to null when we have parented it

                    selectedGridPiece = null;
                }
            }
        }

        if (selectedGridPiece != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.y - 1f;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            selectedGridPiece.transform.parent = null;
            selectedGridPiece.transform.position = worldPos;
        }
        else 
            state = State.Idle;
    }
}
