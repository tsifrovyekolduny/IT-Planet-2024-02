using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEditor.PlayerSettings;

public class PlatformScript : MonoBehaviour
{
    public Light lightSource;
    public GameObject platform;
    bool lightSourceTriggered;
    private float _lightIntencity;

    void Awake()
    {
        lightSourceTriggered = false;
        _lightIntencity = 2;
        
        ResetPlatform();
    }

    // Update is called once per frame
    void Update()
    {
        lightSource.intensity = _lightIntencity;
        if (lightSourceTriggered && lightSource.intensity < 6.0)
        {
            _lightIntencity += 0.0035f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.tag == "StartPlatform")
        {
            ShowPlatform();

            lightSourceTriggered = true;
        }
    }

    void ResetPlatform()
    {
        platform.GetComponent<MeshRenderer>().enabled = false;
        transform.GetComponent<MeshRenderer>().enabled = false;        
    }    

    void ShowPlatform()
    {
        platform.GetComponent<MeshRenderer>().enabled = true;
        transform.GetComponent<MeshRenderer>().enabled = true;
    }
}