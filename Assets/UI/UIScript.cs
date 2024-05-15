using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiScript : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject SurrenderButton;    

    [SerializeField]
    private bool _hidden;
    public bool Surrender;


    void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 0.5f;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeActive();
        }
    }
    
    public void DiscardFromGame()
    {
        Debug.Log("Discard from game");
        GameManager.Instance.CompleteLevel("");
    }

    public void ChangeActive()
    {
        if (_hidden)
        {
            MenuPanel.SetActive(true);
            if (!Surrender)
            {
                SurrenderButton.SetActive(false);
            }
            Time.timeScale = 0;
            _hidden = false;
        }
        else
        {
            MenuPanel.SetActive(false);
            Time.timeScale = 1;
            _hidden = true;
        }
    }
}
