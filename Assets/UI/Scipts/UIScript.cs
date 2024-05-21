using UnityEngine;
using UnityEngine.SceneManagement;

public class UiScript : Singletone<UiScript>
{
    public GameObject MenuPanel;
    public GameObject SurrenderButton;

    private bool _hidden = true;
    public bool Hidden
    {
        get
        {
            return _hidden;
        }
        private set
        {
            _hidden = value;
        }
    }
    private CursorLockMode _previousLockMode;

    public bool Surrender;

    protected override void Start()
    {
        base.Start();
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 0.5f;       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {            
            ChangeActive();
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level >= 1 & level <= 3)
        {
            Surrender = true;            
        }
        else
        {
            Surrender = false;            
        }
    }

    public void DiscardFromGame()
    {
        Debug.Log("Discard from game");
        UiScript.Instance.ChangeActive();
        GameManager.Instance.CompleteLevel(SceneManager.GetActiveScene().name, 0f, false);
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void ExitFromGame()
    {
        Debug.Log("Exit from game");
        Application.Quit();
    }

    public void ChangeActive()
    {        
        if (Hidden)
        {
            _previousLockMode = Cursor.lockState;            
            GameManager.Instance.UnblockCursor();            

            MenuPanel.SetActive(true);
            SurrenderButton.SetActive(Surrender);
            Time.timeScale = 0;
            Hidden = false;
        }
        else
        {
            MenuPanel.SetActive(false);
            Time.timeScale = 1;
            Hidden = true;

            if (_previousLockMode != CursorLockMode.None)
            {
                GameManager.Instance.BlockCursor();
            }

            ResetCursor();
        }
    }
}
