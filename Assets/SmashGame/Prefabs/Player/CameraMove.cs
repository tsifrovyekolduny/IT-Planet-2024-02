using System.Collections;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private int hp;
    private bool canTakeDamage;
    private Vector3 dir;
    private Ray ray;
    private RaycastHit hit;
    public float speed = 1.0f;
    public float pushForce;
    public float time;
    public GameObject shootingPoint;
    public GameObject Bullet;
    public GameObject[] respawns;
    public Camera CutSceneCamera;
    
    void Start()
    {
        canTakeDamage = true;
        time = Time.deltaTime;
        hp = GameManager.Instance.GetNumberOfCompletedLevels();
    }

    IEnumerator HurtReload()
    {
        canTakeDamage = false;

        yield return new WaitForSeconds(1);

        canTakeDamage = true;
    }

    void TakeDamage()
    {
        hp -= 1;
        
        if (hp <= 0)
        {
            CutSceneCamera.GetComponent<ForFinalScript>().EndingStarted(hp);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Damage" && canTakeDamage) {
            bool isDamaged = collision.gameObject.GetComponent<DamageScript>().isDamaged;
            if (!isDamaged) {
                TakeDamage();
                StartCoroutine(HurtReload());
            }
        }
    }

    void OnTriggerEnter(Collider otherObject)
    {


        if (otherObject.tag == "Endingzone") {
            CutSceneCamera.GetComponent<ForFinalScript>().EndingStarted();
        }
        

    }




    void FixedUpdate()
    {
        gameObject.transform.Translate(Vector3.forward * speed * time);
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject bullet = Instantiate(Bullet, shootingPoint.transform.position, transform.rotation);
                dir = shootingPoint.transform.position - hit.point;
                dir.Normalize();
                bullet.GetComponent<Rigidbody>().AddForce(-1 * dir * pushForce, ForceMode.VelocityChange);
            }
        }
    }
}
