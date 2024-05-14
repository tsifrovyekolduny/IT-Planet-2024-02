using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabTrackingPlayer : MonoBehaviour
{
    public Transform Player;
    public Transform Heart;

    private bool rightMouseButtonOnHold = false;

    Vector3 _startPosition;
    public float Speed = 10f;
    [Range(0f, 1f)]
    public float lerpShiftX = 0.1f;
    [Range(0f, 1f)]
    public float lerpShiftY = 0.1f;
    void Start()
    {
        _startPosition = transform.position;
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            rightMouseButtonOnHold = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            rightMouseButtonOnHold = false;
        }

        if (Player != null)
        {
            Transform trackingObject;

            if (rightMouseButtonOnHold || GameManager.Instance.CompletedLevels.Maze == LevelState.NotStarted)
            {
                trackingObject = Heart;
            }
            else
            {
                trackingObject = Player;
            }

            float lerpedX = Mathf.Lerp(_startPosition.x, trackingObject.position.x, lerpShiftX);
            float lerpedY = Mathf.Lerp(_startPosition.y, trackingObject.position.y, lerpShiftY);

            Vector3 lerpedPosition = new Vector3(lerpedX,  lerpedY, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, lerpedPosition, Speed * Time.deltaTime);
        }
    }
}
