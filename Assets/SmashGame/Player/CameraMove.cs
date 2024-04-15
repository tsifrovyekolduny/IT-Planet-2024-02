using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class CameraMove : MonoBehaviour
{
    public int hp = 3;
    public GameObject first, second, third, fourth;
    public float speed = 1.0f;
    public float pushForce;
    public float time;
    Vector3 startPosition;
    public GameObject shootingPoint;
    public GameObject Bullet;
    public GameObject[] respawns;
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
            CutSceneCamera.GetComponent<ForFinalScript>().EndingStarted(hp=0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
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
        if (otherObject.tag == "Endingzone") {
            CutSceneCamera.GetComponent<ForFinalScript>().EndingStarted();
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
                Vector3 testVector = shootingPoint.transform.position;
                Debug.DrawLine(shootingPoint.transform.position, hit.point);
                Vector3 dir = shootingPoint.transform.position - hit.point;
                dir.Normalize();
                bullet.GetComponent<Rigidbody>().AddForce(-1 * dir * pushForce, ForceMode.VelocityChange);
            }
        }
    }
}
