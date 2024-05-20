using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public void LoadScene()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            //case "Start": SceneManager.LoadScene("Assets/Scenes/Pytnashka/Game.unity", LoadSceneMode.Single); break;
            //case "Game": SceneManager.LoadScene("Assets/Scenes/Pytnashka/Start.unity", LoadSceneMode.Single); break;
            case "Start": SceneManager.LoadScene("Game", LoadSceneMode.Single); break;
            case "Game": SceneManager.LoadScene("Start", LoadSceneMode.Single); break;
        }
    }

    public void Shuffle()
    {
        Global.Get.count = 0;
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                Global.Get.board[row, col] = 0;
            }
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Close()
    {
        Application.Quit();
    }

    //public void Rotate()
    //{
    //    foreach (var ship in GlobalVars.chips)
    //    {
    //        float x = ship.transform.rotation.x;
    //        ship.transform.rotation = new Vector3(x, 0, 0);
    //    }
    //}
}
