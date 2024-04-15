using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
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


    void Start()
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteLevel(string name)
    {
        if (name == "Maze")
        {
            CompletedLevels.Maze = true;
        }
        if (name == "TicTacToe")
        {
            CompletedLevels.TicTacToe = true;
        }
        if (name == "TagGame")
        {
            CompletedLevels.TagGame = true;
        }
        SceneManager.LoadScene("HubScene");
    }

    public void PickLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
}
