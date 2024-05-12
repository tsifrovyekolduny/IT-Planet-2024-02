using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Door : MonoBehaviour
{
    public float OpenAngle = -120f;
    public bool OpenedOnStart = false;
    public float OpeningSpeed = 100f;
    public bool CanBeFocusable;
    protected bool _focused;
    protected bool _hovered = false;
    public bool isOpenable = true;
    public bool fourthDoor = false;
    public bool finalDoor = false;
    public Animator AnimatorDoor;
    public AnimationClip AnimationClipDoor;

    [SerializeField] protected AudioClip _openSoundClip;
    [SerializeField] protected AudioClip _closeSoundClip;
    [SerializeField] protected AudioClip _enterSoundClip;

    public void Awake()
    {
        if (OpenedOnStart)
        {
            SetToHingeJointTarget(OpenAngle, true);
        }
    }

    public void OnMouseOver()
    {
        if (fourthDoor && GameManager.Instance.GetNumberOfCompletedLevels() > 0)
        {
            isOpenable = true;
        }
        if (!EventSystem.current.IsPointerOverGameObject() && isOpenable && !finalDoor)
        {
          
            OpenDoor();
            
            if (!_hovered)
            {
                SoundManager.s_Instance.PlayAudioClip(_openSoundClip, transform, 1f);
                _hovered = true;
            }
        }        
    }

    void UnFocusOtherDoors()
    {

    }

    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && isOpenable && !finalDoor)
        {
            if (CanBeFocusable)
            {
                _focused = true;
                foreach (Door door in GameObject.FindObjectsOfType<Door>())
                {
                    if (door != this)
                    {
                        door.CloseDoor();
                    }
                }
                if (AnimatorDoor != null)
                {
                    AnimatorDoor.Play(AnimationClipDoor.name);
                }

                
                SoundManager.s_Instance.PlayAudioClip(_enterSoundClip, transform, 1f);

                DestroyImmediate(GameObject.Find("MusicManager"));
            }
        }       

    }

    public void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && isOpenable && !finalDoor)
        {
            if (!_focused)
            {
                CloseDoor();

                SoundManager.s_Instance.PlayAudioClip(_closeSoundClip, transform, 1f);

                _hovered = false;
            }
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
