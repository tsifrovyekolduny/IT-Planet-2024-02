using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float OpenAngle = -120f;
    public bool OpenedOnStart = false;
    public float OpeningSpeed = 100f;
    public bool CanBeFocusable;
    private bool _focused;
    public Animator AnimatorDoor;
    public AnimationClip AnimationClipDoor;

    public void Awake()
    {
        if (OpenedOnStart)
        {
            SetToHingeJointTarget(OpenAngle, true);
        }
    }

    public void OnMouseOver()
    {
        OpenDoor();
    }

    public void OnMouseDown()
    {
        if (CanBeFocusable)
        {
            _focused = true;
            if (AnimatorDoor != null)
            {
                AnimatorDoor.Play(AnimationClipDoor.name);
            }
            
        }        
    }

    public void OnMouseExit()
    {
        if (!_focused)
        {
            CloseDoor();
        }

    }

    public void OpenDoor()
    {
        SetToHingeJointTarget(OpenAngle);
    }

    public void CloseDoor()
    {
        SetToHingeJointTarget(0f);
        _focused = false;
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
