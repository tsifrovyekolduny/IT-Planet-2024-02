using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastCompletedLevel
{
    public string LevelName;
    public LevelState State;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isFinishAvalable = false;
    public Door FourthDoor;
    public GameObject FadeInTemplate;
    public GameObject FadeOutTemplate;

    private LastCompletedLevel _lastCompletedLevel;

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


    void Awake()
    {
        BlockCursor();
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

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log(level);
        if (level != 0)
        {
            MakeFade(Color.white, false);
            UnblockCursor();
        }
        else
        {
            BlockCursor();            
            if (_lastCompletedLevel != null)
            {                
                InitHub();                
            }
        }
    }

    public void InitHubDoors()
    {        
        if(CompletedLevels.Maze == LevelState.Won)
        {
            SetBlockToDoor("LabLevel", true);
        }
        if(CompletedLevels.TagGame == LevelState.Won)
        {
            SetBlockToDoor("Game", true);
        }
        if(CompletedLevels.TicTacToe == LevelState.Won)
        {
            SetBlockToDoor("TicTacToeLevel", true);
        }

        if(GetNumberOfCompletedLevels() > 0)
        {
            SetBlockToDoor("FourthDoor", false);
        }
    }

    public void SetBlockToDoor(string name, bool isBlocked)
    {
        GameObject.Find(name).GetComponent<Door>().isOpenable = isBlocked;
    }

    public void InitHub()
    {        
        Transform door = GameObject.Find(_lastCompletedLevel.LevelName).transform;
        Transform camera = Camera.main.transform;

        Door doorScript = door.GetComponent<Door>();        

        if (_lastCompletedLevel.State == LevelState.Defeat)
        {
            MakeFade(Color.black, false);
        }
        else
        {
            MakeFade(Color.white, false);
        }

        camera.LookAt(door);
        camera.position = door.position;

        HubCameraMovement cameraScript = Camera.main.GetComponent<HubCameraMovement>();

        cameraScript.EventOnMovingToEnd.AddListener(doorScript.CloseDoor);        
        cameraScript.EventOnMovingToEnd.AddListener(delegate { cameraScript.SetBlock(false); });
        cameraScript.EventOnMovingToEnd.AddListener(InitHubDoors);
        StartCoroutine(cameraScript.MoveCameraToPoint(Vector3.zero));
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

        _lastCompletedLevel = new LastCompletedLevel();
        _lastCompletedLevel.LevelName = name;
        _lastCompletedLevel.State = levelState;

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
