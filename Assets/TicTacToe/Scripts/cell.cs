using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField]
    private int id;

    [SerializeField]
    private Main main;

    private void OnMouseDown()
    {
        if (main.CheckFinishCondition() == 0)
        {
            main.SetGreen(id);
        }

        if (main.CheckFinishCondition() == 0)
        {
            main.SetRed();
        }
    }
}
