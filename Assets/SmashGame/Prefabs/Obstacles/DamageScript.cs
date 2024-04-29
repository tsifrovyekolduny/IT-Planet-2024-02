using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public bool isDamaged;
    public Material brokenStateMaterial;
    private BoxCollider _boxCollider;
    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        isDamaged = false;
        _boxCollider = GetComponent<BoxCollider>();
        if (GetComponent<Rigidbody>() != null)
        {
            _rb = GetComponent<Rigidbody>();
            _rb.isKinematic = true;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet" && _rb.isKinematic != true)
        {
            isDamaged = true;
            if (gameObject.GetComponentsInChildren<Transform>().Length > 1)
            {
                Destroy(_boxCollider);
                Destroy(_rb);
            }
            
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
            {
                if (child.GetComponent<Rigidbody>() == null) { 
                    child.AddComponent<Rigidbody>();
                    child.AddComponent<MeshCollider>().convex = true;
                    MeshCollider MC = GetComponent<MeshCollider>();
                    MeshRenderer renderer = child.GetComponent<MeshRenderer>();
                    if (renderer != null && brokenStateMaterial != null)
                    {
                        renderer.material = brokenStateMaterial;
                    }
                }
                
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "StartPlatform")
        {
            if (_rb != null)
            {
                _rb.isKinematic = false;
            }
        }
    }
}
