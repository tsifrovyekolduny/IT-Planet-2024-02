using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    [SerializeField] protected AudioClip[] _hitSoundClips;

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
            SoundManager.Instance.PlayAudioClip(_hitSoundClips, transform, 1f);

            isDamaged = true;
            if (gameObject.GetComponentsInChildren<Transform>().Length > 1)
            {
                Destroy(_boxCollider);
                Destroy(_rb);
            }
            
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
            {
                if (child.GetComponent<Rigidbody>() == null) {                                         
                    MeshRenderer renderer = child.GetComponent<MeshRenderer>();
                    if (renderer != null && brokenStateMaterial != null)
                    {
                        renderer.material = brokenStateMaterial;
                    }
                    child.AddComponent<Rigidbody>();
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
    }
}
