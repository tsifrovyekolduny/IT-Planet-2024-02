using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint3D : MonoBehaviour
{
    public bool AttachedToCamera;
    public bool PlayOnAwake;
    public float StartTime = 5f;
    private bool _firstTime;

    void Start()
    {
        _firstTime = GameManager.Instance.IsFirstTimeOfScene();

        Debug.Log(GetComponent<Animation>()["0"]);

        if (AttachedToCamera)
        {
            transform.SetParent(Camera.main.transform);
        }

        if (PlayOnAwake)
        {
            Invoke("Show", StartTime);
        }
    }

    public void Show()
    {
        if (_firstTime)
        {
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("Play");
        }        
    }
}
