using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitIconOfLastDoor();
        InitHubAfterLevel();        
    }

    public void InitDoorsAfterLevel()
    {
        if (GameManager.Instance.CompletedLevels.Maze == LevelState.Won)
        {
            SetBlockToDoor("LabLevel", false);
        }
        if (GameManager.Instance.CompletedLevels.TagGame == LevelState.Won)
        {
            SetBlockToDoor("Game", false);
        }
        if (GameManager.Instance.CompletedLevels.TicTacToe == LevelState.Won)
        {
            SetBlockToDoor("TicTacToeLevel", false);
        }

        if (GameManager.Instance.GetNumberOfLevels(LevelState.Won, false) > 0)
        {
            SetBlockToDoor("SmashGame", true);
        }
    }

    public void SetBlockToDoor(string name, bool isBlocked)
    {
        GameObject.Find(name).GetComponent<Door>().isOpenable = isBlocked;
    }

    public void InitIconOfLastDoor()
    {
        Cursor.visible = true;
        Texture2D icon;

        if (GameManager.Instance == null)
        {
            icon = Resources.Load<Texture2D>("Icons/Doors/games0");
        }
        else
        {
            int numberOfCompletedLevels = GameManager.Instance.GetNumberOfLevels(LevelState.Won, false);

            if (numberOfCompletedLevels == 0)
            {
                icon = Resources.Load<Texture2D>("Icons/Doors/games0");
            }
            else if (numberOfCompletedLevels < 3 && numberOfCompletedLevels > 0)
            {
                icon = Resources.Load<Texture2D>("Icons/Doors/games1-2");
            }
            else
            {
                icon = Resources.Load<Texture2D>("Icons/Doors/games3");
            }
        }

        GameObject.Find("SmashGame").GetComponent<Interactable>().InteractionCursor = icon;

    }

    public void InitHubAfterLevel()
    {
        if (GameManager.Instance?.LastLevel != null)
        {
            Transform door = GameObject.Find(GameManager.Instance.LastLevel.LevelName).transform;            
            Transform camera = Camera.main.transform;

            Door doorScript = door.GetComponent<Door>();
            doorScript.CanBeFocusable = false;

            camera.LookAt(door);
            camera.position = door.position;

            HubCameraMovement cameraScript = Camera.main.GetComponent<HubCameraMovement>();            
            cameraScript.StopAllCoroutines();
            cameraScript.SetBlock(true);

            cameraScript.EventOnMovingToEnd.AddListener(doorScript.CloseDoor);
            cameraScript.EventOnMovingToEnd.AddListener(delegate { cameraScript.SetBlock(false); });
            cameraScript.EventOnMovingToEnd.AddListener(delegate { doorScript.CanBeFocusable = true; });
            cameraScript.EventOnMovingToEnd.AddListener(InitDoorsAfterLevel);
            StartCoroutine(cameraScript.MoveCameraToPoint(Vector3.zero, false));
        }
    }
}
