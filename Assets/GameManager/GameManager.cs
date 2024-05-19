using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastCompletedLevel
{
    public string LevelName;
    public LevelState State;
}

public class GameManager : Singletone<GameManager>
{    
    public GameObject FadeInTemplate;
    public GameObject FadeOutTemplate;

    public LastCompletedLevel LastLevel;

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

    public void MakeFade(Color color, bool fadeIn)
    {
        Fader fader;

        if (fadeIn)
        {
            fader = Instantiate(FadeInTemplate).GetComponent<Fader>();
        }
        else
        {
            fader = Instantiate(FadeOutTemplate).GetComponent<Fader>();
        }

        fader.BackgroundColor = color;
        fader.StartFade();
    }    

    public void BlockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        // InitializeManager();
        MakeFade(Color.black, false);
        BlockCursor();
    }

    private void OnLevelWasLoaded(int level)
    {        
        if (level != 0)
        {
            UnblockCursor();
            MakeFade(Color.white, false);            
        }
        else
        {
            BlockCursor();            
            if (LastLevel != null)
            {
                if (LastLevel.State == LevelState.Defeat)
                {
                    MakeFade(Color.black, false);
                }
                else
                {
                    MakeFade(Color.white, false);
                }
            }
        }
    }

    

    public void UnblockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

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
    
    private void InitializeManager()
    {
        CompletedLevels = new LevelsComleted(LevelState.NotStarted, LevelState.NotStarted, LevelState.NotStarted);        
    }    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteLevel(string name, float timeAfterEnd = 5f, bool isWin = true)
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

        if (isWin)
        {
            MakeFade(Color.white, true);
        }
        else
        {
            MakeFade(Color.black, true);
        }

        LastLevel = new LastCompletedLevel();
        LastLevel.LevelName = name;
        LastLevel.State = levelState;

        Invoke("BackToHub", timeAfterEnd);
    }

    void BackToHub()
    {
        BlockCursor();
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
