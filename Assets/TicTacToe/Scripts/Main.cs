using System;
using System.Collections;
using UnityEngine;

public class Main : MonoBehaviour
{
    private PlayerObject[] _greenPlayer = new PlayerObject[50];
    private PlayerObject[] _redPlayer = new PlayerObject[50];

    private int[,] _map = new int[10, 10];

    [SerializeField]private GameObject _greenObject;
    [SerializeField]private GameObject _redObject;

    public GameObject PlayerSpawnpoint;
    public GameObject EnemySpawnpoint;

    private int _greenNumber = 0;
    private int _redNumber = 0;

    public float SpawningDelay = 0.05f;

    public float MovingSpeed = 1f;

    public int LineSizeToWin = 5;

    /*public void CheckFinishCondition()
    {
        // X row
        for (int coordinateX = 0; coordinateX < _map.Length - LineSizeToWin; ++coordinateX)
        {
            for (int coordinateY = 0; coordinateY < _map.Length; ++coordinateY)
            {
                int controlSumm = 0;
                for (int addition = 0; addition < LineSizeToWin; ++addition) 
                {
                    controlSumm += _map[coordinateX + addition, coordinateY];
                }

                if (controlSumm == 5)
                {
                    //победа
                    Debug.Log("good result");
                    return;
                }
                else if (controlSumm == -5)
                {
                    //слив
                    Debug.Log("bad result");
                    return;
                }
            }
        }
        // Y row
        for (int coordinateX = 0; coordinateX < _map.Length; ++coordinateX)
        {
            for (int coordinateY = 0; coordinateY < _map.Length - LineSizeToWin; ++coordinateY)
            {
                int controlSumm = 0;
                for (int addition = 0; addition < LineSizeToWin; ++addition)
                {
                    controlSumm += _map[coordinateX, coordinateY + addition];
                }

                if (controlSumm == 5)
                {
                    //победа
                    Debug.Log("good result");
                    return;
                }
                else if (controlSumm == -5)
                {
                    //слив
                    Debug.Log("bad result");
                    return;
                }
            }
        }
        // diagonal
        for (int coordinateX = 0; coordinateX < _map.Length - LineSizeToWin; ++coordinateX)
        {
            for (int coordinateY = 0; coordinateY < _map.Length - LineSizeToWin; ++coordinateY)
            {
                int controlSumm = 0;
                for (int addition = 0; addition < LineSizeToWin; ++addition)
                {
                    controlSumm += _map[coordinateX + addition, coordinateY + addition];
                }
                
                if (controlSumm == 5)
                {
                    //победа
                    Debug.Log("good result");
                    return;
                }
                else if (controlSumm == -5)
                {
                    //слив
                    Debug.Log("bad result");
                    return;
                }
            }
        }
    }
    */

    public IEnumerator MoveObject(GameObject currentObject, Vector3 position)
    {
        float counter = 0;

        while (counter < MovingSpeed)
        {
            counter += Time.deltaTime;

            Vector3 currentPosition = currentObject.transform.position;

            float time = Vector3.Distance(currentPosition, position) / (MovingSpeed - counter) * Time.deltaTime;

            currentObject.transform.position = Vector3.MoveTowards(currentPosition, position, time);

            yield return null;
        }
    }

    public IEnumerator SpawnObjects(float time)
    {
        for (int objectIndex = 0;  objectIndex < _greenPlayer.Length; ++objectIndex)
        {
            _redPlayer[objectIndex] = Instantiate(_redObject, EnemySpawnpoint.transform.position, Quaternion.identity).GetComponent<PlayerObject>();
            _greenPlayer[objectIndex] = Instantiate(_greenObject, PlayerSpawnpoint.transform.position, Quaternion.identity).GetComponent<PlayerObject>();
            yield return new WaitForSeconds(time);
        }
        yield break;
    }

    public void Start()
    {
        StartCoroutine(SpawnObjects(SpawningDelay));
    }

    public void SetGreen(int id)
    {
        _map[id % 10, id / 10] = 1;

        Collider currentCollider = GameObject.Find(id.ToString()).GetComponent<Collider>();

        currentCollider.enabled = false;

        _greenPlayer[_greenNumber].GetComponent<Transform>().rotation = Quaternion.Euler(-90f, 0f, 0f);
        _greenPlayer[_greenNumber].GetComponent<Rigidbody>().isKinematic = true;

        StartCoroutine(MoveObject(_greenPlayer[_greenNumber].gameObject, currentCollider.transform.position));

        ++_greenNumber;

        //CheckFinishCondition();
    }
    public void SetRed()
    {
        int id;
        Collider currentCollider;
        do
        {
            id = UnityEngine.Random.Range(0, 99);
            currentCollider = GameObject.Find(id.ToString()).GetComponent<Collider>();
        }
        while (currentCollider.enabled == false);

        _map[id % 10, id / 10] = -1;

        _redPlayer[_redNumber].GetComponent<Transform>().rotation = Quaternion.Euler(-90f, 0f, 0f);
        _redPlayer[_redNumber].GetComponent<Rigidbody>().isKinematic = true;

        StartCoroutine(MoveObject(_redPlayer[_redNumber].gameObject, currentCollider.transform.position));

        currentCollider.enabled = false;

        ++_redNumber;

        //CheckFinishCondition();
    }
}
