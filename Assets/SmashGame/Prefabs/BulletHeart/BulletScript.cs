using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float secondsOfLife = 4;

    private void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Damage")
        {
            collision.gameObject.GetComponent<DamageScript>().BeingHit();
        }
    }

    IEnumerator DestroyBullet()
    {

        yield return new WaitForSeconds(secondsOfLife);

        gameObject.SetActive(false);
    }
}
