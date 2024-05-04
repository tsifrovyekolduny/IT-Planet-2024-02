using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CreateBoard : MonoBehaviour
{
    public GameObject[] chips;
    private int skiped_element;


    public Vector3 board_position = new Vector3(-2f, -10f, -2f);
    public GameObject ramka, kartinka;
    void Start()
    {
        Global.ramka = ramka;
        Global.kartinka = kartinka;
        GenerateBoard();
        ShowBoard();

        Renderer renderer = Global.kartinka.GetComponent<Renderer>();
        Color color = renderer.material.color;
        color.a = 0; // 1f делает объект полностью непрозрачным
        renderer.material.color = color;

        renderer = Global.ramka.GetComponent<Renderer>();
        color = renderer.material.color;
        color.a = 0; // 1f делает объект полностью непрозрачным
        renderer.material.color = color;
    }
    void Update()
    {
        if (MoveChip.game_finished)
        {
            Renderer renderer = Global.kartinka.GetComponent<Renderer>();
            Color color = renderer.material.color;
            if (color.a <1 )
            {
                color.a += 0.001f; // 1f делает объект полностью непрозрачным
                renderer.material.color = color;

            }

            renderer = Global.ramka.GetComponent<Renderer>();
            color = renderer.material.color;
            if (color.a < 1)
            {
                color.a += 0.001f; // 1f делает объект полностью непрозрачным
                renderer.material.color = color;

            }
        }

    }
    void GenerateBoard()
    {
        int maxCountBlocks = 16 - Global.countEraceBlocks;
        for (int i = 0; i <= maxCountBlocks; i++)
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
