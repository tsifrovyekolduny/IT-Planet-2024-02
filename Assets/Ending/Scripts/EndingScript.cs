using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingScript : MonoBehaviour
{
    public GameObject[] GoodObjects;
    public GameObject[] SoSoObjects;
    public GameObject[] BadObjects;

    public Animator DeathAnimator;
    public Animator GirlAnimator;

    public Canvas EndCanvas;

    public float TimeBeforeActing = 5f;
    public float TimeBeforeEnd = 15f;

    private int _counterOfWonnedGames;
    private string _endingText;

    private Animator _actor;

    private void Awake()
    {
        ChangeActiveToObjects(GoodObjects, false);
        ChangeActiveToObjects(SoSoObjects, false);
        ChangeActiveToObjects(BadObjects, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _counterOfWonnedGames = GameManager.Instance.GetNumberOfLevels(LevelState.Won, false);
        if(_counterOfWonnedGames < 1)
        {
            _endingText = "Мужчина в возрасте 42 лет скончался от сердечного приступа. Причина: алкоголизм";
            ChangeActiveToObjects(BadObjects, true);
            _actor = DeathAnimator;
        }
        else if (_counterOfWonnedGames < 4)
        {
            _endingText = "Мужчина в возрасте 45 лет застрелился. Причина: затяжная депрессия";
            ChangeActiveToObjects(SoSoObjects, true);
        }
        else
        {
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
            _actor.SetBool("NextAction", true);
            Debug.Log("Actor playing");
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
