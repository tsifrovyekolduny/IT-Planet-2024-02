using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MainCamera")
        {
            GetComponent<Door>().OpenDoor();
        }
    }
}
