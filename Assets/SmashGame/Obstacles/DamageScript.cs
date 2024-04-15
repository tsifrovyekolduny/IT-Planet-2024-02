using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public bool isDamaged;
    public Material brokenStateMaterial;
    // Start is called before the first frame update
    void Start()
    {
        isDamaged = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(isDamaged);
        if (collision.gameObject.tag == "Bullet") {
            isDamaged = true;
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
            {
                child.AddComponent<Rigidbody>(); 
                child.AddComponent<MeshCollider>().convex = true;
                MeshRenderer renderer = child.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material = brokenStateMaterial;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
