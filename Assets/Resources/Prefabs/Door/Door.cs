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
    public string LevelName;    

    protected bool _focused;
    protected bool _hovered = false;
    public bool isOpenable = true;
    public bool fourthDoor = false;
    public bool finalDoor = false;

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

    public void OnMouseEnter()
    {
        ShowCursorWithIcon();
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

    public void ShowCursorWithIcon()
    {
        Cursor.visible = true;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
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
                
                SoundManager.s_Instance.PlayAudioClip(_enterSoundClip, transform, 1f);

                DestroyImmediate(GameObject.Find("MusicManager"));

                StartCoroutine(MoveCameraToDoor());
            }
        }
    }

    IEnumerator MoveCameraToDoor()
    {        
        HubCameraMovement camera = Camera.main.gameObject.GetComponent<HubCameraMovement>();
        Vector3 startPoint = camera.transform.position;
        camera.SetBlock(true);
        GameManager.Instance.MakeFade(Color.white, true);
        while (camera.transform.position != transform.position)
        {    
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, transform.position, camera.Speed * Time.deltaTime);

            yield return null;
        }

        GameManager.Instance.PickLevel(LevelName);
        yield break;
    }

    public void OnMouseExit()
    {
        HideCursor();
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
