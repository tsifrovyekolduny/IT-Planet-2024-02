﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CreateBoard : MonoBehaviour
{
    public GameObject[] chips = new GameObject[16];
    private int skiped_element;


    public Vector3 board_position = new Vector3(-2f, -10f, -2f);
    void Start()
    {
        GenerateBoard();
        ShowBoard();
    }
    void Update()
    {

    }
    void GenerateBoard()
    {
        for (int i = 0; i < 13; i++)
        {
            bool cancel = false;
            do
            {
                int row = Random.Range(0, 4);
                int col = Random.Range(0, 4);
                if (Global.board[row + Global.x_offset, col + Global.y_offset] == 0)
                {
                    cancel = true;
                    Global.board[row + Global.x_offset, col + Global.y_offset] = i;
                }
            } while (!cancel);
        }
    }
    void ShowBoard()
    {
        GlobalVars.chips = chips;

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (Global.board[row + Global.x_offset, col + Global.y_offset] != 0)
                {
                    Vector3 coardinate = new Vector3(board_position.x + row * GlobalVars.x_offset, board_position.y, board_position.z + col * GlobalVars.z_offset);
                    int chip = Global.board[row + Global.x_offset, col + Global.y_offset] - 1;
                    var obj = Instantiate(chips[chip], coardinate, transform.rotation);
                    obj.name = chips[chip].name;
                }
            }
        }
    }
}
