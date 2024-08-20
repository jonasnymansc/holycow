using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int scoreInAreaPerBlockPerTurn = 10;
    private int scoreInAreaPerBlock = 10;
    private int levelScore = 0;

    public int LevelScore => levelScore;
    public static Action<int> OnScoreDeltaChanged;
    private void Start()
    {
        levelScore = 0;
    }

    private void OnEnable()
    {
        InputManager.OnPieceReleased += OnPieceReleased;

    }
    
    private void OnDisable()
    {
        InputManager.OnPieceReleased -= OnPieceReleased;
    }
    
    void OnPieceReleased(GridPiece gridPiece)
    {
        // give score for the first step 
        int countInsideArea = gridPiece.CountBlocksInsideArea();
        int deltaScore = countInsideArea * scoreInAreaPerBlockPerTurn;
        levelScore += deltaScore;
        OnScoreDeltaChanged?.Invoke(deltaScore);
    }
}
