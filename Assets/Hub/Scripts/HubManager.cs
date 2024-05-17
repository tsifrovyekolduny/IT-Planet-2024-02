using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.LastLevel != null)
        {
            InitHubAfterLevel();            
        }
    }

    // Update is called once per frame
    void Update()
    {

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

        if (GameManager.Instance.GetNumberOfCompletedLevels() > 0)
        {
            SetBlockToDoor("Final-SmashHit", true);
        }
    }

    public void SetBlockToDoor(string name, bool isBlocked)
    {
        GameObject.Find(name).GetComponent<Door>().isOpenable = isBlocked;
    }

    public void InitHubAfterLevel()
    {
        Transform door = GameObject.Find(GameManager.Instance.LastLevel.LevelName).transform;
        Transform camera = Camera.main.transform;

        Door doorScript = door.GetComponent<Door>();

        camera.LookAt(door);
        camera.position = door.position;        

        HubCameraMovement cameraScript = Camera.main.GetComponent<HubCameraMovement>();
        cameraScript.StartTimeForBlockingCamera = 0f;        

        cameraScript.EventOnMovingToEnd.AddListener(doorScript.CloseDoor);
        cameraScript.EventOnMovingToEnd.AddListener(delegate { cameraScript.SetBlock(false); });
        cameraScript.EventOnMovingToEnd.AddListener(InitDoorsAfterLevel);
        StartCoroutine(cameraScript.MoveCameraToPoint(Vector3.zero, false));
    }
}
