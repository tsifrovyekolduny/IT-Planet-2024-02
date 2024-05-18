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

    void Start()
    {
        isDamaged = false;
        _boxCollider = GetComponent<BoxCollider>();
        if (GetComponent<Rigidbody>() != null)
        {
            _rb = GetComponent<Rigidbody>();
            _rb.isKinematic = true;
        }
        if (gameObject.name == "Trash")
        {
            Debug.Log(_rb.isKinematic);
        }

    }

    public void BeingHit()
    {
        if (_rb.isKinematic == false)
        {
            Debug.Log(gameObject.name);
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

    private void OnBecameVisible()
    {
        if (_rb != null)
        {
            _rb.isKinematic = false;
        }
        if (gameObject.name == "Trash")
        {
            Debug.Log(gameObject.name + ": " + _rb.isKinematic);
        }
    }
}
