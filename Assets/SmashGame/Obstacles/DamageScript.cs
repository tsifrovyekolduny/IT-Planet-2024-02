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
        if (collision.gameObject.tag == "Bullet") {
            isDamaged = true;
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
            {
                child.AddComponent<Rigidbody>(); 
                child.AddComponent<MeshCollider>().convex = true;
                MeshRenderer renderer = child.GetComponent<MeshRenderer>();
                if (renderer != null && brokenStateMaterial != null)
                {
                    renderer.material = brokenStateMaterial;
                }
                Destroy(child.gameObject, 5);
            }
            Destroy(gameObject, 5);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
