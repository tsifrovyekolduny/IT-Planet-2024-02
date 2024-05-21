using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float secondsOfLife = 4;

    private void Start()
    {
        StartCoroutine(DestroyBullet(secondsOfLife));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Damage")
        {
            collision.gameObject.GetComponent<DamageScript>().BeingHit();
            StartCoroutine(DestroyBullet(0.4f));
        }
    }

    IEnumerator DestroyBullet(float timeToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroy);

        gameObject.SetActive(false);
    }
}
