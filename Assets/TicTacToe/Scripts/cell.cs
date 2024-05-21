using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    [SerializeField]
    private int id;

    [SerializeField]
    private Main main;

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (main.GameResult == 0)
            {
                main.SetGreen(id);
            }

            if (main.GameResult == 0)
            {
                main.SetRed();
            }
        }                
    }
}
