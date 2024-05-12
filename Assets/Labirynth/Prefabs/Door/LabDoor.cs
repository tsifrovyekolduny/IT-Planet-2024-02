using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LabDoor : Door
{
    private bool _isOpened;
    IEnumerator OpenAfterTime()
    {
        yield return new WaitForSeconds(0.2f);
        
        OpenDoor();
        SoundManager.s_Instance.PlayAudioClip(_openSoundClip, transform, 0.3f);
        _isOpened = true;
    }

    public new void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && isOpenable)
        {
            if (!_hovered)
            {
                _hovered = true;
                StartCoroutine(OpenAfterTime());                
            }
        }
    }

    public new void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && isOpenable)
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
            }
        }
    }

    public new void OnMouseExit()
    {
        StopAllCoroutines();        
        _hovered = false;        

        if (!EventSystem.current.IsPointerOverGameObject() && isOpenable)
        {
            if (!_focused && _isOpened)
            {
                _isOpened = false;
                CloseDoor();
                SoundManager.s_Instance.PlayAudioClip(_closeSoundClip, transform, 0.3f);
            }
        }
    }
}
