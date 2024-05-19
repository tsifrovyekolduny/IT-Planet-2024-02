using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScript : MonoBehaviour
{
    public GameObject[] GoodObjects;
    public GameObject[] SoSoObjects;
    public GameObject[] BadObjects;

    private void Awake()
    {
        ChangeActiveToObjects(GoodObjects, false);
        ChangeActiveToObjects(SoSoObjects, false);
        ChangeActiveToObjects(BadObjects, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        int counterOfWonnedGames = GameManager.Instance.GetNumberOfLevels(LevelState.Won, false);
        if(counterOfWonnedGames < 1)
        {
            ChangeActiveToObjects(BadObjects, true);
        }
        else if (counterOfWonnedGames < 4)
        {
            ChangeActiveToObjects(SoSoObjects, true);
        }
        else
        {
            ChangeActiveToObjects(GoodObjects, true);
        }
    }



    void ChangeActiveToObjects(GameObject[] gameObjects, bool isActive)
    {
        foreach(GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(isActive);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


}
