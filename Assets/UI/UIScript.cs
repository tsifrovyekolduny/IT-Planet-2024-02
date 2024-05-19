using UnityEngine;

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
    
    public void DiscardFromGame()
    {
        Debug.Log("Discard from game");
        GameManager.Instance.CompleteLevel("");
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void ChangeActive()
    {        
        if (Hidden)
        {
            _previousLockMode = Cursor.lockState;            
            GameManager.Instance.UnblockCursor();            

            MenuPanel.SetActive(true);
            if (!Surrender)
            {
                SurrenderButton.SetActive(false);
            }
            Time.timeScale = 0;
            Hidden = false;
        }
        else
        {
            MenuPanel.SetActive(false);
            Time.timeScale = 1;
            Hidden = true;

            if (_previousLockMode != CursorLockMode.Confined)
            {
                GameManager.Instance.BlockCursor();
            }

            ResetCursor();
        }
    }
}
