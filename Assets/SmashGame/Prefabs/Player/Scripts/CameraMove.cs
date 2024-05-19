using System.Collections;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private int _hp = 3;
    private bool _canTakeDamage;
    private bool _isReloading;
    private Vector3 dir;
    private Ray ray;
    private RaycastHit hit;

    public float speed = 1.0f;
    public float pushForce;
    public float time;
    public float reloadTime = 0.8f;
    public float invisibleTime = 1f;
    public Animator PlayerAnimator;
    public GameObject ShootingPoint;
    public GameObject Bullet;
    public Camera CutSceneCamera;
    
    void Start()
    {
        _canTakeDamage = true;
        time = Time.deltaTime;
        //_hp = GameManager.Instance.GetNumberOfCompletedLevels();
        _isReloading = false;
    }

    IEnumerator HurtReload()
    {
        _canTakeDamage = false;

        PlayerAnimator.SetBool("isHurt", true);

        yield return new WaitForSeconds(invisibleTime);

        PlayerAnimator.SetBool("isHurt", false);

        _canTakeDamage = true;
    }

    void TakeDamage()
    {
        _hp -= 1;
        
        if (_hp <= 0)
        {
            GameManager.Instance.CompleteLevel("SmashHit", timeAfterEnd: 10f, false);            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Damage" && _canTakeDamage) {
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

            GameManager.Instance.CompleteLevel("SmashHit");            
        }
    }


    IEnumerator ReloadBullet()
    {
        _isReloading = true;

        PlayerAnimator.SetBool("isReloading", true);

        yield return new WaitForSeconds(reloadTime);

        PlayerAnimator.SetBool("isReloading", false);

        _isReloading = false;
    }

    void FixedUpdate()
    {
        if(_hp > 0)
        {
            gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isReloading && _hp > 0)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            GameObject bullet = Instantiate(Bullet, ShootingPoint.transform.position, transform.rotation);
            dir = ShootingPoint.transform.position - hit.point;
            dir.Normalize();
            bullet.GetComponent<Rigidbody>().AddForce(-1 * dir * pushForce, ForceMode.VelocityChange);
            StartCoroutine(ReloadBullet());
        }
    }

}
