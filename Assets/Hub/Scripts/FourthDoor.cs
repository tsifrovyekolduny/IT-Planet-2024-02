using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthDoor : MonoBehaviour
{
    public GameObject door;
    public GameObject floor;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.GetNumberOfCompletedLevels() > 0)
        {
            door.gameObject.SetActive(true);
            //door.gameObject.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            door.gameObject.SetActive(false);
        }
    }
    


    // Update is called once per frame
    void Update()
    {
       
    }
}
