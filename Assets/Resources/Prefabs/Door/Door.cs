using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{    
    [SerializeField]
    private bool _isOpened = false;
    public float OpenAngle = -120f;
    public bool OpenedOnStart = false;
    public float OpeningSpeed = 100f;

    public void Update()
    {
        if (_isOpened)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    public void Awake()
    {
        OpenDoor();
        if (OpenedOnStart)
        {
            SetToHingeJointTarget(OpenAngle, true);
        }
    }

    public void OpenDoor()
    {
        SetToHingeJointTarget(OpenAngle);
    }

    public void CloseDoor()
    {
        SetToHingeJointTarget(0f);
    }

    private void SetToHingeJointTarget(float targetPosition, bool fast = false)
    {
        HingeJoint hingeJoint = transform.GetChild(0).GetChild(1).GetComponent<HingeJoint>();
        JointSpring jointSpring = hingeJoint.spring;
        if (fast)
        {
            jointSpring.spring = 10000;
        }
        else
        {
            jointSpring.spring = OpeningSpeed;
        }
        
        jointSpring.targetPosition = targetPosition;
        hingeJoint.spring = jointSpring;
    }    
}
