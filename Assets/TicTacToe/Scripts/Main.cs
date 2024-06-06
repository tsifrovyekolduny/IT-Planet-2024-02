using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    private GameObject[] _greenPlayer = new GameObject[50];
    private GameObject[] _redPlayer = new GameObject[50];

    private int[,] _map = new int[10, 10];

    [SerializeField] private GameObject _greenObject;
    [SerializeField] private GameObject _redObject;

    [SerializeField] private AudioClip[] _bottlePlaceSound;
    [SerializeField] private AudioClip[] _heartPlaceSound;

    public GameObject PlayerSpawnpoint;
    public GameObject EnemySpawnpoint;

    private int _greenNumber = 0;
    private int _redNumber = 0;

    public bool PlayerMovementBlocked;

    public float SpawningDelay = 0.05f;
    public float MovingSpeed = 3f;

    public float FlyingSpeed = 10f;
    public int LineSizeToWin = 5;

    private bool _finished = false;
    public int GameResult = 0;

    public int CheckFinishCondition(int id)
    {
        List<int> checksums = new List<int>();

        int currentX = id / 10;
        int currentY = id % 10;

        //x row
        for (int coordinateX = currentX - (LineSizeToWin - 1); coordinateX <= currentX; ++coordinateX)
        {
            int checksum = 0;

            for (int addition = 0; addition < LineSizeToWin; ++addition)
            {
                if (0 <= coordinateX + addition & coordinateX + addition < 10)
                {
                    checksum += _map[currentY, coordinateX + addition];
                }
            }
            checksums.Add(checksum);
        }

        //y row
        for (int coordinateY = currentX - (LineSizeToWin - 1); coordinateY <= currentY; ++coordinateY)
        {
            int checksum = 0;

            for (int addition = 0; addition < LineSizeToWin; ++addition)
            {
                if (0 <= coordinateY + addition & coordinateY + addition < 10)
                {
                    checksum += _map[coordinateY + addition, currentX];
                }
            }
            checksums.Add(checksum);
        }

        //main diagonal
        for (int deletion = LineSizeToWin - 1; deletion >= 0; --deletion)
        {
            int checksum = 0;

            for (int addition = 0; addition < LineSizeToWin; ++addition)
            {
                if (currentX - deletion + addition >= 0 & 
                    currentX - deletion + addition < 10 &
                    currentY - deletion + addition >= 0 &
                    currentY - deletion + addition < 10)
                {
                    checksum += _map[currentY - deletion + addition, currentX - deletion + addition];
                }
            }
            checksums.Add(checksum);
        }

        //not main diagonal
        for (int deletion = LineSizeToWin - 1; deletion >= 0; --deletion)
        {
            int checksum = 0;

            for (int addition = 0; addition < LineSizeToWin; ++addition)
            {
                if (currentX - deletion + addition >= 0 &
                    currentX - deletion + addition < 10 &
                    currentY + deletion - addition >= 0 &
                    currentY + deletion - addition < 10)
                {
                    checksum += _map[currentY + deletion - addition, currentX - deletion + addition];
                }
            }
            checksums.Add(checksum);
        }

        if (checksums.Contains(5))
        {
            return 1;
        }
        if (checksums.Contains(-5))
        {
            return -1;
        }
        return 0;
    }

    public IEnumerator MoveObject(GameObject currentObject, Vector3 position, bool playerMovement)
    {
        Vector3 oldPosition = currentObject.transform.position;

        while (currentObject.transform.position != position)
        {            
            currentObject.transform.position = Vector3.MoveTowards(currentObject.transform.position, position, FlyingSpeed);            

            yield return null;
        }

        if (playerMovement)
        {
            SoundManager.Instance.PlayAudioClip(_heartPlaceSound, transform, 1f);
        }
        else
        {
            SoundManager.Instance.PlayAudioClip(_bottlePlaceSound, transform, 1f);
        }
    }

    private void Update()
    {
        if(GameResult != 0 && !_finished)
        {
            _finished = true;
            bool isWin = GameResult == 1;
            GameManager.Instance.CompleteLevel(SceneManager.GetActiveScene().name, 5f, isWin);            
        }
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
            if (GameResult != 0)
            {
                yield break;
            }

            Vector3 newEnemyPosition = new Vector3(UnityEngine.Random.Range(enemyX - shift, enemyX + shift), 
                                                    commonY,
                                                    UnityEngine.Random.Range(enemyZ - shift, enemyZ + shift));

            Vector3 newPlayerPosition = new Vector3(UnityEngine.Random.Range(playerX - shift, playerX + shift),
                                                    commonY,
                                                    UnityEngine.Random.Range(playerZ - shift, playerZ + shift));

            _redPlayer[objectIndex] = Instantiate(_redObject, newEnemyPosition, _redObject.transform.rotation);
            _greenPlayer[objectIndex] = Instantiate(_greenObject, newPlayerPosition, _greenObject.transform.rotation);

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
        PlayerMovementBlocked = true;

        _map[id % 10, id / 10] = 1;
        DeleteInteractablre(id);

        Collider currentCollider = GameObject.Find(id.ToString()).GetComponent<Collider>();

        currentCollider.enabled = false;

        _greenPlayer[_greenNumber].transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        _greenPlayer[_greenNumber].GetComponent<Rigidbody>().isKinematic = true;

        StartCoroutine(MoveObject(_greenPlayer[_greenNumber].gameObject, currentCollider.transform.position, true));

        ++_greenNumber;

        GameResult = CheckFinishCondition(id);
    }

    private void DeleteInteractablre(int id)
    {
        var interactable = GameObject.Find(id.ToString()).GetComponent<Interactable>();
        Destroy(interactable);
    }

    public void SetRed()
    {
        PlayerMovementBlocked = false;

        int id;        
        Collider currentCollider;
        do
        {
            id = UnityEngine.Random.Range(0, 99);
            currentCollider = GameObject.Find(id.ToString()).GetComponent<Collider>();
        }
        while (currentCollider.enabled == false);
        DeleteInteractablre(id);
        _map[id % 10, id / 10] = -1;

        _redPlayer[_redNumber].transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        _redPlayer[_redNumber].GetComponent<Rigidbody>().isKinematic = true;

        StartCoroutine(MoveObject(_redPlayer[_redNumber].gameObject, currentCollider.transform.position, false));

        currentCollider.enabled = false;

        ++_redNumber;

        GameResult = CheckFinishCondition(id);
    }
}
