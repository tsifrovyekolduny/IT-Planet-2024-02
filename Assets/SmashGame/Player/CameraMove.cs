using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMove : MonoBehaviour
{
    public int hp = 3;
    public float speed = 1.0f;
    public float pushForce;
    public float time;
    Vector3 startPosition;
    public GameObject shootingPoint;
    public GameObject Bullet;
    public Camera CutSceneCamera;
    void Start()
    {
        startPosition = transform.position;
        time = Time.deltaTime;
    }

    void TakeDamage()
    {
        hp -= 1;
        if (hp == 0)
        {
            foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
            {
                if (o.name != "CutSceneCamera")
                {
                    Destroy(o);
                }
                
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Damage") {
            bool isDamaged = collision.gameObject.GetComponent<DamageScript>().isDamaged;
            if (!isDamaged) {
                TakeDamage();
            }
        }
    }

    void OnTriggerEnter(Collider otherObject)
    {
        if (otherObject.tag == "ResetLevel")
        {
            BackToSpawn();
        }
        

    }

    void BackToSpawn()
    {
        transform.position = startPosition;
    }



    void Update()
    {
        gameObject.transform.Translate(Vector3.forward * speed * time);
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject bullet = Instantiate(Bullet, shootingPoint.transform.position, transform.rotation);
                Vector3 dir = shootingPoint.transform.position - hit.point;
                dir.Normalize();
                bullet.GetComponent<Rigidbody>().AddForce(-1 * hit.point * pushForce, ForceMode.VelocityChange);
            }
        }
    }
}
