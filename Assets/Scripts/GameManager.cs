using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private LevelManager levelManager;
    
    public enum State
    {
        Idle, Play, Pause, Lose, Win
    }

    private void OnEnable()
    {
        levelManager.OnLevelStateChanged += OnLevelStateChanged;

    }

    private void OnDisable()
    {
        levelManager.OnLevelStateChanged -= OnLevelStateChanged;
    }

    void OnLevelStateChanged(LevelManager.LevelState levelState)
    {
        if (levelState == LevelManager.LevelState.Win)
        {
            Debug.Log("YOU WIN MOOOO!");
        }
        if (levelState == LevelManager.LevelState.Lose)
        {
            Debug.Log("YOU LOSE MOOOOO!");   
        }
    }
    
    private void Start()
    {

    }
}
