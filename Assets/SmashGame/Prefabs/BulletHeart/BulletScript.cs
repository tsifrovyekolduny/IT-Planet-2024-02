
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float secondsOfLife = 4;

    private void Start()
    {
        Destroy(gameObject, 4);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
