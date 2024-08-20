using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text movesText;
    [SerializeField]
    private ScoreManager scoreManager;
    [SerializeField]
    private LevelManager levelManager;
    private void OnEnable()
    {
        ScoreManager.OnScoreDeltaChanged += OnScoreDeltaChanged;
        levelManager.OnMoveUsed += OnMoveUsed;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreDeltaChanged -= OnScoreDeltaChanged;
        levelManager.OnMoveUsed -= OnMoveUsed;
    }

    private void Start()
    {
        OnScoreDeltaChanged(0);
        OnMoveUsed();
    }

    void OnScoreDeltaChanged(int deltaScore)
    {
        scoreText.text = $"{scoreManager.LevelScore}";
    }
    void OnMoveUsed()
    {
        movesText.text = $"{levelManager.MovesLeft}";
    }

}
