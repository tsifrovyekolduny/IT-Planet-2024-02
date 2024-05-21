using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    public TextMeshProUGUI EndingText;
    public Image BackgroundPanel;
    public GameObject[] Buttons;

    private string _endingText;

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        GameManager.Instance.InitializeManager();
        GameManager.Instance.BackToHub();
    }

    public void ShowUI(string endingText)
    {
        _endingText = endingText;
        StartCoroutine("SlowShow");
    }

    public IEnumerator SlowShow()
    {        
        Color color;

        while (BackgroundPanel.color.a < 1)
        {
            color = BackgroundPanel.color;
            color.a += 0.01f;
            BackgroundPanel.color = color;
            yield return null;
        }
        
        EndingText.text = _endingText;
        while (EndingText.color.a < 1)
        {
            color = EndingText.color;
            color.a += 0.01f;
            EndingText.color = color;
            yield return new WaitForSeconds(0.1f);
        }
        GameManager.Instance.UnblockCursor();
        ShowButtons();
    }

    private void ShowButtons()
    {
        foreach(var button in Buttons)
        {
            button.SetActive(true);
        }
    }
}
