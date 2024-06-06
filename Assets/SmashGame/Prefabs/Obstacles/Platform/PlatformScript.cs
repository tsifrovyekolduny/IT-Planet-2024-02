using UnityEngine;


public class PlatformScript : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject[] prefabs;

    void Start()
    {
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        Instantiate(prefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }
}