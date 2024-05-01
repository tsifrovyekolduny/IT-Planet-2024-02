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
        canTakeDamage = false;
        time = Time.deltaTime;
        hp = GameManager.Instance.GetNumberOfCompletedLevels();
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
