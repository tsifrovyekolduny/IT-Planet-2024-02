using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabTrackingPlayer : MonoBehaviour
{
    public Transform Player;
    Vector3 _startPosition;
    public float Speed = 10f;
    [Range(0f, 1f)]
    public float lerpShift = 0.1f;
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Player != null)
        {
            float lerpedX = Mathf.Lerp(_startPosition.x, Player.position.x, lerpShift);
            float lerpedY = Mathf.Lerp(_startPosition.y, Player.position.y, lerpShift);

            Vector3 lerpedPosition = new Vector3(lerpedX,  lerpedY, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, lerpedPosition, Speed * Time.deltaTime);
        }
    }
}
