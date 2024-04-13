using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private Transform _transform;
    private Vector3 target;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    public void StartMove(Vector3 vector)
    {
        target = vector;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (_transform.position != target)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, target, Time.deltaTime * _speed);

            yield return null;
        }
    }

}
