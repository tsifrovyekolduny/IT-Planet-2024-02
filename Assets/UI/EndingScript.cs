using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScript : MonoBehaviour
{
    public GameObject GoodEnging;
    public GameObject BadEnging;
    public GameObject SoSoEnging;

    // Start is called before the first frame update
    void Start()
    {
        int counterOfCompletetedGames = GameManager.Instance.GetNumberOfCompletedLevels();
        switch (counterOfCompletetedGames)
        {
            case 0:
            case 1:
                ShowBadEnding();
                break;
            case 2:
                ShowSoSoEnding();
                break;
            case 3:
                ShowGoodEnding();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGoodEnding()
    {
        GoodEnging.SetActive(true);
    }

    public void ShowSoSoEnding()
    {
        SoSoEnging.SetActive(true);
    }

    public void ShowBadEnding()
    {
        BadEnging.SetActive(true);
    }
}
