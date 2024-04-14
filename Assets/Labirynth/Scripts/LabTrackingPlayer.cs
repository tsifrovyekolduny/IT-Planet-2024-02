using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabTrackingPlayer : MonoBehaviour
{
    public Transform Player;
    public float Speed = 10f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Player != null)
        {
            transform.position = Player.transform.position + new Vector3(0, 1, -5);
        }
    }
}
