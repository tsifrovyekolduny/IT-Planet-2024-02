using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EndingScript : MonoBehaviour
{
    [SerializeField] private AudioSource _goodAudioClip;
    [SerializeField] private AudioSource _badAudioClip;
    [SerializeField] private AudioSource _soSoAudioClip;

    public GameObject[] GoodObjects;
    public GameObject[] SoSoObjects;
    public GameObject[] BadObjects;    

    public Animator DeathAnimator;
    public Animator GirlAnimator;

    public Canvas EndCanvas;

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
        _endingText = "Мужчина в возрасте 42 лет скончался от сердечного приступа. Причина: алкоголизм";
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
        _counterOfWonnedGames = GameManager.Instance.GetNumberOfLevels(LevelState.Won, false);
        if (_counterOfWonnedGames < 1)
        {
            InitBadEnding();
        }
        else if (_counterOfWonnedGames < 4)
        {
            AudioSource audioSource = Instantiate(_soSoAudioClip, transform.position, Quaternion.identity);

            Destroy(audioSource.gameObject, audioSource.clip.length);

            _endingText = "Мужчина в возрасте 45 лет застрелился. Причина: затяжная депрессия";
            ChangeActiveToObjects(SoSoObjects, true);
        }
        else
        {
            AudioSource audioSource = Instantiate(_goodAudioClip, transform.position, Quaternion.identity);

            Destroy(audioSource.gameObject, audioSource.clip.length);

            _endingText = "Мужчина жив, здоров. Дочь пошла поступила в колледж. Жена вернулась. Работа восстановилась...";
            ChangeActiveToObjects(GoodObjects, true);
        }

        Invoke("MakeActorsPlay", TimeBeforeActing);
    }

    void EndGame()
    {
        EndCanvas.gameObject.SetActive(true);
        StartCoroutine("ShowUI");
    }

    IEnumerator ShowUI()
    {
        Image image = EndCanvas.GetComponentInChildren<Image>();
        Color color;

        while(image.color.a < 1)
        {
            Debug.Log(image.color.a);
            color = image.color;
            color.a += 0.01f;
            image.color = color;
            yield return null;
        }

        TextMeshProUGUI textMesh = EndCanvas.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = _endingText;
        while(textMesh.color.a < 1)
        {
            color = textMesh.color;
            color.a += 0.01f;
            textMesh.color = color;
            yield return new WaitForSeconds(0.1f);
        }

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
            //Debug.Log("Actor playing");
        }
        Invoke("EndGame", TimeBeforeEnd);
    }

    void ChangeActiveToObjects(GameObject[] gameObjects, bool isActive)
    {
        foreach(GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(isActive);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
