using UnityEngine;

public abstract class Factory : MonoBehaviour
{
    public Transform SpawnPoint;
    public abstract GameObject CreateObject();
}
