using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEditor.PlayerSettings;

public class PlatformScript : MonoBehaviour
{
    public Light lightSource;
    public GameObject platform;
    bool lightSourceTriggered;
    private float _lightIntencity;
    public GameObject spawnPoint;
    public GameObject[] prefabs;
    private int randomNumber;
    public GameObject obstacle;

    void Start()
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
        if (other.tag == "StartPlatform")
        {
            ShowPlatform();

            lightSourceTriggered = true;
        }
    }

    public void ResetPlatform()
    {

        platform.GetComponent<MeshRenderer>().enabled = false;        
    }    

    public void ShowPlatform()
    {
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        Instantiate(prefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        platform.GetComponent<MeshRenderer>().enabled = true;
    }
}