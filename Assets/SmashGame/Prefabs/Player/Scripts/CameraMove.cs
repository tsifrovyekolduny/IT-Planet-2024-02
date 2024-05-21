using System.Collections;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] protected AudioClip[] _getDamageSoundClip;

    private int _hp = 3;
    private bool _isFinished = false;
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

    public Texture2D AimCursor;
    public Texture2D UsualCursor;

    void Start()
    {
        Cursor.SetCursor(AimCursor, new Vector2(25f, 25f), CursorMode.Auto);
        _canTakeDamage = true;
        time = Time.deltaTime;
        _hp = GameManager.Instance.LifeCounter;
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
        GameManager.Instance.LifeCounter = _hp;

        if (_hp <= 0 & !_isFinished)
        {
            _isFinished = true;
            Cursor.SetCursor(UsualCursor, Vector2.zero, CursorMode.Auto);
            GameManager.Instance.CompleteLevel("SmashGame", timeAfterEnd: 10f, false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Damage" && _canTakeDamage)
        {
            SoundManager.Instance.PlayAudioClip(_getDamageSoundClip, transform, 1f);

            bool isDamaged = collision.gameObject.GetComponent<DamageScript>().isDamaged;
            if (!isDamaged)
            {
                TakeDamage();
                StartCoroutine(HurtReload());
            }
        }
    }

    void OnTriggerEnter(Collider otherObject)
    {
        if (otherObject.tag == "Endingzone" && !_isFinished)
        {
            _isFinished = true;
            Cursor.SetCursor(UsualCursor, Vector2.zero, CursorMode.Auto);
            Debug.Log("Smash Game won");
            GameManager.Instance.CompleteLevel("SmashGame");
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
        if (_hp > 0)
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
