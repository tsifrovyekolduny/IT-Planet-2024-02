using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    [SerializeField] protected AudioClip[] _hitSoundClips;
    [SerializeField] private int points = 10;

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

    private void OnEnable()
    {
        EventManager.OnObjectDestroy += HandleBeingHit;
    }

    private void OnDisable()
    {
        EventManager.OnObjectDestroy -= HandleBeingHit;
    }

    public void HandleBeingHit(int hittedObjectID)
    {
        if (!isDamaged && hittedObjectID == gameObject.GetInstanceID())
        {
            if (_rb.isKinematic == false)
            {
                //SoundManager.Instance.PlayAudioClip(_hitSoundClips, transform, 1f);

                isDamaged = true;
                EventManager.RaiseAddPoints(points);
                if (gameObject.GetComponentsInChildren<Transform>().Length > 1)
                {
                    Destroy(_boxCollider);
                    Destroy(_rb);
                }

                foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
                {
                    if (child.GetComponent<Rigidbody>() == null)
                    {
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
    }

    private void OnBecameVisible()
    {
        if (_rb != null)
        {
            _rb.isKinematic = false;
        }
    }
}
