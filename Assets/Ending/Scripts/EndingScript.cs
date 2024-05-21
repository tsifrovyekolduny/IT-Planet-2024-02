using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class EndingScript : MonoBehaviour
{
    public GameObject[] GoodObjects;
    public GameObject[] SoSoObjects;
    public GameObject[] BadObjects;

    public VideoPlayer CardioVideo;
    public VideoClip BadCardioVariant;

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
            ChangeActiveToObjects(SoSoObjects, true);
        }
        else
        {
            _actor = GirlAnimator;
            _endingText = "FES. 5:16";
            ChangeActiveToObjects(GoodObjects, true);
            TimeBeforeActing = 5f;
            TimeBeforeEnd = 10f;
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

        while (image.color.a < 1)
        {
            color = image.color;
            color.a += 0.01f;
            image.color = color;
            yield return null;
        }

        TextMeshProUGUI textMesh = EndCanvas.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = _endingText;
        while (textMesh.color.a < 1)
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
