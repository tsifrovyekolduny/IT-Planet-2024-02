using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    private Collider[] _colliders = new Collider[100];

    private Transform[] _points;

    private PlayerObject[] _greenPlayer = new PlayerObject[50];
    private PlayerObject[] _redPlayer = new PlayerObject[50];

    [SerializeField]private GameObject _greenObject;
    [SerializeField]private GameObject _redObject;

    private int greenNumber = 0;

    public void Start()
    {
        float x = 1.7f;
        for (int index = 0; index < 5; ++index)
        {
            _redPlayer[index] = Instantiate(_redObject, new Vector3(-x, 0.5f, 2.5f), Quaternion.identity).GetComponent<PlayerObject>();
            _greenPlayer[index] = Instantiate(_greenObject, new Vector3(x, 0.5f, 2.5f), Quaternion.identity).GetComponent<PlayerObject>();
            x -= 0.85f;
        }
        for (int index = 0; index < _colliders.Length; ++index)
        {
            _points[index] = _colliders[index].GetComponent<Transform>();
        }
    }

    public void SetGreen(int id)
    {
        Debug.Log(id);
        Debug.Log(_greenPlayer.Length);
        //_colliders[id].enabled = false;
        _greenPlayer[greenNumber].StartMove(_points[id].position);
        ++greenNumber;
    }
}
