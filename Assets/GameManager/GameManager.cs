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
    private Dictionary<string, bool> _scenesLoaded = new Dictionary<string, bool>();
    public GameObject FadeInTemplate;
    public GameObject FadeOutTemplate;

    public int LifeCounter;
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

    public bool IsFirstTimeOfScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (!_scenesLoaded.ContainsKey(sceneName))
        {
            return true;
        }
        return !_scenesLoaded[sceneName];
    }

    private void MarkSceneAsLoaded()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        _scenesLoaded[sceneName] = true;
    }

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
        MakeFade(Color.black, false);
        BlockCursor();        
    }

    private void OnLevelWasLoaded(int level)
    {        
        if (level < (SceneManager.sceneCountInBuildSettings - 1) && level > 0)
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public int GetNumberOfLevels(LevelState levelState = LevelState.NotStarted, bool intersect = true)
    {
        int counter = 0;
        if (intersect)
        {
            if (CompletedLevels.Maze != levelState)
            {
                ++counter;
            }
            if (CompletedLevels.TicTacToe != levelState)
            {
                ++counter;
            }
            if (CompletedLevels.TagGame != levelState)
            {
                ++counter;
            }
        }
        else
        {
            if (CompletedLevels.Maze == levelState)
            {
                ++counter;
            }
            if (CompletedLevels.TicTacToe == levelState)
            {
                ++counter;
            }
            if (CompletedLevels.TagGame == levelState)
            {
                ++counter;
            }
        }


        return counter;
    }

    public void InitializeManager()
    {
        CompletedLevels = new LevelsComleted(LevelState.NotStarted, LevelState.NotStarted, LevelState.NotStarted);
        LifeCounter = 0;
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

        if(timeAfterEnd > 0f)
        {
            if (isWin)
            {
                MakeFade(Color.white, true);
            }
            else
            {
                MakeFade(Color.black, true);
            }
        }        

        LastLevel = new LastCompletedLevel();
        LastLevel.LevelName = name;
        LastLevel.State = levelState;

        if (name == "SmashGame")
        {            
            Invoke("ToEnding", timeAfterEnd);
        }
        else
        {            
            Invoke("BackToHub", timeAfterEnd);
        }

    }

    public void BackToHub()
    {
        MarkSceneAsLoaded();
        BlockCursor();
        SceneManager.LoadScene("HubScene");
    }

    void ToEnding()
    {
        MarkSceneAsLoaded();
        SceneManager.LoadScene("Ending");
    }

    public void PickLevel(string name)
    {
        MarkSceneAsLoaded();
        if(name == "SmashGame")
        {
            LifeCounter = GetNumberOfLevels(LevelState.Won, false);
        }
        SceneManager.LoadScene(name);
    }
}

public enum LevelState
{
    NotStarted,
    Won,
    Defeat
}
