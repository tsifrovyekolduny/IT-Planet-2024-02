using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMove : MonoBehaviour
{
    public float speed = 1.0f;
    public float pushForce;
    public float time;
    Vector3 startPosition;
    public GameObject shootingPoint;
    public GameObject Bullet;
    public GameObject bulletScript;
    void Start()
    {
        startPosition = transform.position;
        time = Time.deltaTime;
    }

    void OnTriggerEnter(Collider otherObject)

    {

        BackToSpawn();

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
                Vector3 dir = ray.origin - hit.point;
                dir.Normalize();
                bullet.GetComponent<Rigidbody>().AddForce(-1 * dir * pushForce, ForceMode.Impulse);
            }
        }
    }
}
