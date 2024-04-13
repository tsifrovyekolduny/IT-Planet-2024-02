using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public bool isDamaged;
    // Start is called before the first frame update
    void Start()
    {
        isDamaged = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet") {
            isDamaged = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
