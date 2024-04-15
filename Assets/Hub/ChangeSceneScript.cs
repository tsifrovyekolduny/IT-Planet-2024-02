using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeToMaze()
    {
        GameManager.Instance.PickLevel("LabLevel");
    }

    public void ChangeToTictacToe()
    {
        GameManager.Instance.PickLevel("TicTacToeLevel");
    }

    public void ChangeToGame()
    {
        GameManager.Instance.PickLevel("Game");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
