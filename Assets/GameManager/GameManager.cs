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
        public bool Maze;
        public bool TicTacToe;
        public bool TagGame;

        public LevelsComleted(bool maze, bool ticTacToe, bool tagGame)
        {
            Maze = maze;    
            TicTacToe = ticTacToe;
            TagGame = tagGame;
        }
     }
    LevelsComleted CompletedLevels;

    public int GetNumberOfCompletedLevels()
    {
        int counter = 0;
        if (CompletedLevels.Maze)
        {
            ++counter;
        }
        if (CompletedLevels.TicTacToe)
        {
            ++counter;
        }
        if (CompletedLevels.TagGame)
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
        CompletedLevels = new LevelsComleted(false, false, false);
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

    public void CompleteLevel(string name)
    {
        Debug.Log(name);

        if (name == "LabLevel")
        {
            CompletedLevels.Maze = true;
        }
        if (name == "TicTacToeLevel")
        {
            CompletedLevels.TicTacToe = true;
        }
        if (name == "Game")
        {
            CompletedLevels.TagGame = true;
        }
        FourthDoor.isOpenable = true;
        SceneManager.LoadScene("HubScene");
    }

    public void PickLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
}
