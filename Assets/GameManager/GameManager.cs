using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isFinishAvalable = false;
    public Door FourthDoor;
    [System.Serializable]
    public struct LevelsComleted
    {
        public LevelState Maze;
        public LevelState TicTacToe;
        public LevelState TagGame;

        public LevelsComleted(LevelState maze, LevelState ticTacToe, LevelState tagGame)
        {
            Maze = maze;
            TicTacToe = ticTacToe;
            TagGame = tagGame;
        }
     }
    public LevelsComleted CompletedLevels;

    public int GetNumberOfCompletedLevels()
    {
        int counter = 0;
        if (CompletedLevels.Maze != LevelState.NotStarted)
        {
            ++counter;
        }
        if (CompletedLevels.TicTacToe != LevelState.NotStarted)
        {
            ++counter;
        }
        if (CompletedLevels.TagGame != LevelState.NotStarted)
        {
            ++counter;
        }

        return counter;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        InitializeManager();
    }

    private void InitializeManager()
    {
        CompletedLevels = new LevelsComleted(LevelState.NotStarted, LevelState.NotStarted, LevelState.NotStarted);
        isFinishAvalable = false;
    }

    public bool IsFinishAvalable()
    {
        return isFinishAvalable;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteLevel(string name, float timeAfterEnd = 10f, bool isWin = true)
    {
        LevelState levelState = isWin ? LevelState.Won : LevelState.Defeat;

        Debug.Log(name);

        if (name == "LabLevel")
        {
            CompletedLevels.Maze = levelState;
        }
        if (name == "TicTacToeLevel")
        {
            CompletedLevels.TicTacToe = levelState;
        }
        if (name == "Game")
        {
            CompletedLevels.TagGame = levelState;
        }

        // TODO fade dark or fade light

        FourthDoor.isOpenable = true;
        Invoke("BackToHub", timeAfterEnd);
    }

    void BackToHub()
    {
        SceneManager.LoadScene("HubScene");
    }

    public void PickLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
}

public enum LevelState
{
    NotStarted,
    Won,
    Defeat
}
