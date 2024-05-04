using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float secondsOfLife = 4;

    private void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {

        yield return new WaitForSeconds(secondsOfLife);

        gameObject.SetActive(false);
    }
}
