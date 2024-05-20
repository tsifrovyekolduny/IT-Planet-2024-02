using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintUI : MonoBehaviour
{
    public Image HintImage;
    public float ShowTime = 5f;

    void Start()
    {
        Color color = HintImage.color;
        color.a = 0;
        HintImage.color = color;

        var cameraScript = Camera.main.GetComponent<HubCameraMovement>();
        cameraScript.EventOnUnblockingAfterTime.AddListener(StartShow);
    }   

    void StartShow()
    {
        StartCoroutine("Show");
    }

    IEnumerator Show()
    {
        while (HintImage.color.a < 1)
        {
            Color color = HintImage.color;
            color.a += 0.01f;
            HintImage.color = color;
            yield return null;
        }
        Invoke("StartHide", ShowTime);
    }

    void StartHide()
    {
        StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        while (HintImage.color.a > 0)
        {
            Color color = HintImage.color;
            color.a -= 0.01f;
            HintImage.color = color;
            yield return null;
        }
    }

}
