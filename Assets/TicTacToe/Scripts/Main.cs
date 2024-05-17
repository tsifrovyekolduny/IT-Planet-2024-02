using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    private GameObject[] _greenPlayer = new GameObject[50];
    private GameObject[] _redPlayer = new GameObject[50];

    private int[,] _map = new int[10, 10];

    [SerializeField] private GameObject _greenObject;
    [SerializeField] private GameObject _redObject;

    [SerializeField] private AudioClip[] _glassFallSound;

    public GameObject PlayerSpawnpoint;
    public GameObject EnemySpawnpoint;

    private int _greenNumber = 0;
    private int _redNumber = 0;

    public float SpawningDelay = 0.05f;

    public float MovingSpeed = 1f;

    public int LineSizeToWin = 5;

    public int CheckFinishCondition()
    {
        // X row
        for (int coordinateX = 0; coordinateX < Mathf.Sqrt(_map.Length) - LineSizeToWin; ++coordinateX)
        {
            for (int coordinateY = 0; coordinateY < Mathf.Sqrt(_map.Length); ++coordinateY)
            {
                int controlSumm = 0;
                for (int addition = 0; addition < LineSizeToWin; ++addition) 
                {
                    controlSumm += _map[coordinateX + addition, coordinateY];
                }

                if (controlSumm == 5)
                {
                    //победа
                    GameManager.Instance.CompleteLevel(SceneManager.GetActiveScene().name);
                    return 1;
                }
                else if (controlSumm == -5)
                {
                    //слив
                    GameManager.Instance.CompleteLevel("");
                    return -1;
                }
            }
        }
        // Y row
        for (int coordinateX = 0; coordinateX < Mathf.Sqrt(_map.Length); ++coordinateX)
        {
            for (int coordinateY = 0; coordinateY < Mathf.Sqrt(_map.Length) - LineSizeToWin; ++coordinateY)
            {
                int controlSumm = 0;
                for (int addition = 0; addition < LineSizeToWin; ++addition)
                {
                    controlSumm += _map[coordinateX, coordinateY + addition];
                }

                if (controlSumm == 5)
                {
                    //победа
                    GameManager.Instance.CompleteLevel(SceneManager.GetActiveScene().name);
                    return 1;
                }
                else if (controlSumm == -5)
                {
                    //слив
                    Debug.Log("bad result");
                    GameManager.Instance.CompleteLevel("");
                    return -1;
                }
            }
        }
        // diagonal
        for (int coordinateX = 0; coordinateX < Mathf.Sqrt(_map.Length) - LineSizeToWin; ++coordinateX)
        {
            for (int coordinateY = 0; coordinateY < Mathf.Sqrt(_map.Length) - LineSizeToWin; ++coordinateY)
            {
                int controlSumm = 0;
                for (int addition = 0; addition < LineSizeToWin; ++addition)
                {
                    controlSumm += _map[coordinateX + addition, coordinateY + addition];
                }
                
                if (controlSumm == 5)
                {
                    //победа
                    GameManager.Instance.CompleteLevel(SceneManager.GetActiveScene().name);
                    return 1;
                }
                else if (controlSumm == -5)
                {
                    //слив
                    Debug.Log("bad result");
                    GameManager.Instance.CompleteLevel("");
                    return -1;
                }
            }
        }

        return 0;
    }

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

        SoundManager.Instance.PlayAudioClip(_glassFallSound, transform, 1f);
    }

    public IEnumerator SpawnObjects(float time)
    {        
        float commonY = EnemySpawnpoint.transform.position.y;

        float enemyZ = EnemySpawnpoint.transform.position.z;
        float playerZ = PlayerSpawnpoint.transform.position.z;

        float enemyX = EnemySpawnpoint.transform.position.x;
        float playerX = PlayerSpawnpoint.transform.position.x;
        float shift = 1.5f;

        for (int objectIndex = 0;  objectIndex < _greenPlayer.Length; ++objectIndex)
        {
            

            Vector3 newEnemyPosition = new Vector3(UnityEngine.Random.Range(enemyX - shift, enemyX + shift), 
                                                    commonY,
                                                    UnityEngine.Random.Range(enemyZ - shift, enemyZ + shift));

            Vector3 newPlayerPosition = new Vector3(UnityEngine.Random.Range(playerX - shift, playerX + shift),
                                                    commonY,
                                                    UnityEngine.Random.Range(playerZ - shift, playerZ + shift));

            _redPlayer[objectIndex] = Instantiate(_redObject, newEnemyPosition, _redObject.transform.rotation);
            _greenPlayer[objectIndex] = Instantiate(_greenObject, newPlayerPosition, _greenObject.transform.rotation);

            if (CheckFinishCondition() != 0)
            {
                yield break;
            }

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

        _greenPlayer[_greenNumber].transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        _greenPlayer[_greenNumber].GetComponent<Rigidbody>().isKinematic = true;

        StartCoroutine(MoveObject(_greenPlayer[_greenNumber].gameObject, currentCollider.transform.position));

        ++_greenNumber;

        CheckFinishCondition();
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

        _redPlayer[_redNumber].transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        _redPlayer[_redNumber].GetComponent<Rigidbody>().isKinematic = true;

        StartCoroutine(MoveObject(_redPlayer[_redNumber].gameObject, currentCollider.transform.position));

        currentCollider.enabled = false;

        ++_redNumber;

        CheckFinishCondition();
    }
}
