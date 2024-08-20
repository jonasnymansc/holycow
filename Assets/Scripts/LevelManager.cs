using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public GameObject nextPieceHolder;
    public GridPiece nextPiece;

    [SerializeField]
    private List<GridPiece> gridPieces;
    
    public Action<GridPiece> OnPieceSpawned;
    public Action<LevelState> OnLevelStateChanged;
    public Action OnMoveUsed;

    [SerializeField]
    private int totalMoves = 10;
    private int movesLeft = 0;

    private List<Block> openBlocks = new List<Block>();
    [SerializeField]
    private List<GameObject> levels = new List<GameObject>();
    private List<GridPiece> placedGridPieces = new List<GridPiece>();

    public enum LevelState
    {
        Play, Win, Lose 
    }
    private LevelState levelState = LevelState.Play;

    [SerializeField]
    private int steps = 5;
    
    public int MovesLeft => movesLeft;

    private int currentLevelStep = 0;
    public int CurrentLevelStep => currentLevelStep;

    public int LevelSteps => steps;

    private void Start()
    {
        LoadNextStep();
    }

    private void LoadNextStep()
    {
        StartCoroutine(LoadNextStepDeferred());
    }

    private void OnEnable()
    {
        InputManager.OnPieceReleased += OnPieceReleased;
    }

    private void OnDisable()
    {
        InputManager.OnPieceReleased -= OnPieceReleased;
    }

    private void OnPieceReleased(GridPiece gridPiece)
    {
        placedGridPieces.Add(gridPiece);
        if (movesLeft > 0)
        {
            movesLeft--;
            OnMoveUsed?.Invoke();
        }

        if (CountOpenBlocks() == 0)
        {
            SetState(LevelState.Win);
            currentLevelStep++;
            LoadNextStep();
            return;
        }
        
        // check level end 
        if (movesLeft < 0)
        {
            SetState(LevelState.Lose);
            return;
        }

        SpawnNextPiece();
    }

    private IEnumerator LoadNextStepDeferred()
    {
        // destroy grid pieces
        foreach (var placedGridPiece in placedGridPieces)
        {
            Destroy(placedGridPiece.gameObject);
        }
        yield return null;
        if (placedGridPieces != null)
            placedGridPieces.Clear();
        
        foreach (var level in levels)
        {
            level.SetActive(false);
        }

        if (currentLevelStep >= levels.Count)
        {
            Debug.Log("No more levels");
            currentLevelStep = 0;
        }
        levels[currentLevelStep].SetActive(true);
        yield return null;
        // reset moves count
        movesLeft = totalMoves;
        
        openBlocks.Clear();
        openBlocks = GetComponentsInChildren<Block>().ToList().Where(b => b.blockType == BlockType.Open && b.Active).ToList();
        
        Debug.Log($"Loaded {openBlocks.Count} open blocks.");
        OnMoveUsed.Invoke();
        SpawnNextPiece();   // get a new piece
    }

    private void SetState(LevelState levelState)
    {
        this.levelState = levelState;
        OnLevelStateChanged?.Invoke(this.levelState);
    }

    public bool SpawnNextPiece()
    {
        // randomize the next piece
        int rnd = Random.Range(0, gridPieces.Count);
        nextPiece = gridPieces[rnd];
        GameObject go = Instantiate(nextPiece.gameObject, Vector3.zero, Quaternion.identity, nextPieceHolder.transform);
        go.transform.localPosition = Vector3.zero;
        OnPieceSpawned?.Invoke(go.GetComponent<GridPiece>());
        return true;
    }

    public int CountOpenBlocks()
    {
        int count = 0;
        foreach (var openBlock in openBlocks)
        {
            if (openBlock.blockType == BlockType.Open)
            {
                if (openBlock.Active)
                {
                    count++;
                }
            }
        }
        Debug.Log($"Found {count} open blocks.");
        return count;
    }
}
