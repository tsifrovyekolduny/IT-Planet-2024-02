using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForFinalScript : MonoBehaviour
{
    public GameObject GoodEnging;
    public GameObject BadEnging;
    public GameObject SoSoEnging;
    public Camera camera;

    public void EndingStarted(int hp = 0)
    {
        int counterOfCompletetedGames;
        if (hp == 0)
        {
            counterOfCompletetedGames = 0;
        }
        else
        {
            counterOfCompletetedGames = GameManager.Instance.GetNumberOfCompletedLevels();
        }
        
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            if (o.tag != "ForFinal")
            {
                Destroy(o);
            }
        }
        if (counterOfCompletetedGames == 0)
        {
            ShowBadEnding();
        }
        else if (counterOfCompletetedGames > 0 && counterOfCompletetedGames < 3)
        {
            ShowSoSoEnding();
        }
        else
        {
            ShowGoodEnding();
        }
    }

    public void ShowGoodEnding()
    {
        camera.backgroundColor = Color.white;
        GoodEnging.SetActive(true);
    }

    public void ShowSoSoEnding()
    {
        camera.backgroundColor = Color.gray;
        SoSoEnging.SetActive(true);
    }

    public void ShowBadEnding()
    {
        camera.backgroundColor = Color.black;
        BadEnging.SetActive(true);
    }
}
