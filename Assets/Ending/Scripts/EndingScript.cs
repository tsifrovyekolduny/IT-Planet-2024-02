using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class EndingScript : MonoBehaviour
{
    [SerializeField] private AudioSource _goodAudioClip;
    [SerializeField] private AudioSource _badAudioClip;
    [SerializeField] private AudioSource _soSoAudioClip;

    public GameObject[] GoodObjects;
    public GameObject[] SoSoObjects;
    public GameObject[] BadObjects;

    public VideoPlayer CardioVideo;
    public VideoClip BadCardioVariant;

    public Animator DeathAnimator;
    public Animator GirlAnimator;

    public EndUI EndCanvas;

    public float TimeBeforeActing = 5f;
    public float TimeBeforeEnd = 15f;

    [SerializeField]
    private int _counterOfWonnedGames;
    private string _endingText;

    private Animator _actor;

    private void Awake()
    {
        ChangeActiveToObjects(GoodObjects, false);
        ChangeActiveToObjects(SoSoObjects, false);
        ChangeActiveToObjects(BadObjects, false);
    }

    void InitBadEnding()
    {
        _endingText = "EKKL. 7:17";
        ChangeActiveToObjects(BadObjects, true);
        RenderSettings.fog = enabled;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fogDensity = 0.1f;
        RenderSettings.ambientLight = new Color(5f, 5f, 5f);
        _actor = DeathAnimator;
    }

    // Start is called before the first frame update
    void Start()
    {
        _counterOfWonnedGames = GameManager.Instance.LifeCounter;
        if (_counterOfWonnedGames <= 0)
        {
            InitBadEnding();
        }
        else if (_counterOfWonnedGames <= 2)
        {

            _endingText = "PS. 24:16";
            AudioSource audioSource = Instantiate(_soSoAudioClip, transform.position, Quaternion.identity);
            Destroy(audioSource.gameObject, audioSource.clip.length);

            ChangeActiveToObjects(SoSoObjects, true);
        }
        else
        {

            _actor = GirlAnimator;
            _endingText = "FES. 5:16";
            AudioSource audioSource = Instantiate(_goodAudioClip, transform.position, Quaternion.identity);

            Destroy(audioSource.gameObject, audioSource.clip.length);

            ChangeActiveToObjects(GoodObjects, true);
            TimeBeforeActing = 5f;
            TimeBeforeEnd = 10f;
        }
        Invoke("MakeActorsPlay", TimeBeforeActing);
    }

    void EndGame()
    {
        EndCanvas.gameObject.SetActive(true);
        EndCanvas.ShowUI(_endingText);
    }    

    void MakeActorsPlay()
    {
        if (_actor != null)
        {
            if (_counterOfWonnedGames < 1)
            {
                // bad ending
                Debug.Log(2);
            }
            else
            {
                // good ending
                Debug.Log(3);
            }
            _actor.SetBool("NextAction", true);
            if (GameManager.Instance.LifeCounter == 0)
            {
                CardioVideo.clip = BadCardioVariant;
            }
            Debug.Log("Actor playing");
        }
        Invoke("EndGame", TimeBeforeEnd);
    }

    void ChangeActiveToObjects(GameObject[] gameObjects, bool isActive)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(isActive);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
