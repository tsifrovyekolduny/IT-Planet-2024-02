using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MoveChip : MonoBehaviour
{
    enum PreferredMove
    {
        Up,Down,Left,Right, None
    };
    private PreferredMove preferredMove;
    private int number_chip;
    private int row_position;
    private int col_position;
    private int speed;
    private Vector3 empty_position = new Vector3(0, 0, 0);
    private Vector3 old_position = new Vector3(-10, -10, -10);
    int new_row, new_col;

    private GameObject ui_motion;
    private GameObject ui_completed;

    private bool can_move;

    private Vector3 initialMousePos;
    private Vector3 finalMousePos;
    private Vector3 mouseDelta;

    void Start()
    {
        speed = 2;
        number_chip = int.Parse(gameObject.name);
        ui_motion = GameObject.Find("Motion");
        ui_completed = GameObject.Find("Completed");
    }
    void Update()
    {
        if (can_move)
        {
            MoveChipOnBoard();
        }
    }

    Vector3 input_vector;
    void OnMouseDown()
    {
        initialMousePos = Input.mousePosition;
    }
    void OnMouseUp()
    {
        if (!can_move)
        {
            finalMousePos = Input.mousePosition;
            FindOnBoard();
            CalculateDirection();
        }
    }

    void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }
    void MotionCountPlus()
    {
        Global.count++;
        //ui_motion.GetComponent<Text>().text = "КОЛИЧЕСТВО ХОДОВ\n\n " + Global.count.ToString();
    }
    void MoveChipOnBoard()
    {
        if (transform.position != empty_position)
        {
            transform.position = Vector3.MoveTowards(transform.position, empty_position, speed * Time.deltaTime);
        }
        else
        {
            Global.board[row_position, col_position] = 0;
            Global.board[new_row, new_col] = number_chip;
            can_move = false;
            row_position = new_row;
            col_position = new_col;
            MotionCountPlus();
            CheckOnComplete();
        }
    }
    void CheckOnComplete()
    {
        int count = 1;
        int maxAcceptCount = 16 - Global.countEraceBlocks + 1;
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (Global.board[row + Global.x_offset, col + Global.y_offset] == count)
                {
                    Debug.Log(count);
                }
                else
                {
                    return;
                }
                count++;
                if (maxAcceptCount == count)
                {
                    Completed();
                }
            }
        }
    }
    public void Completed()
    {
        Debug.Log("Уровень завершён");
        //ui_completed.GetComponent<Text>().enabled = true;
        GameManager.Instance.CompleteLevel(SceneManager.GetActiveScene().name);
    }
    void CalculateDirection()
    {
        try
        {
            // Вычисляем предпочтительное
            mouseDelta = finalMousePos - initialMousePos;
           if(Math.Abs(mouseDelta.x) > Math.Abs(mouseDelta.y))
            {
                if (Math.Abs(mouseDelta.x) < 10)
                {
                    preferredMove = PreferredMove.None;
                }
                else
                {
                    if (mouseDelta.x > 0)
                    {
                        preferredMove = PreferredMove.Right;
                    }
                    else
                    {
                        preferredMove = PreferredMove.Left;
                    }
                }
            }
            else
            {
                if (Math.Abs(mouseDelta.y) < 10)
                {
                    preferredMove = PreferredMove.None;
                }
                else
                {
                    if (mouseDelta.y > 0)
                    {
                        preferredMove = PreferredMove.Up;
                    }
                    else
                    {
                        preferredMove = PreferredMove.Down;
                    }
                }
            }

            //left
            if (Global.board[row_position, col_position - 1] == 0)
            {

                empty_position = new Vector3(transform.position.x, 0, transform.position.z - GlobalVars.z_offset);
                new_row = row_position;
                new_col = col_position - 1;
                PlaySound();
                can_move = true;
                //if (empty_position != old_position)
                //{
                //    old_position = new Vector3(transform.position.x, 0, transform.position.z);
                //    return;
                //}
                old_position = new Vector3(transform.position.x, 0, transform.position.z);
                if (preferredMove == PreferredMove.Left)
                {
                    return;
                }
            }
        }
        catch { }


        //up
        try
        {
            if (Global.board[row_position - 1, col_position] == 0)
            {
                empty_position = new Vector3(transform.position.x - GlobalVars.x_offset, 0, transform.position.z);
                new_row = row_position - 1;
                new_col = col_position;
                PlaySound();
                can_move = true;
                //if (empty_position != old_position)
                //{
                //    old_position = new Vector3(transform.position.x, 0, transform.position.z);
                //    return;
                //}
                
                old_position = new Vector3(transform.position.x, 0, transform.position.z);

                if (preferredMove == PreferredMove.Up)
                {
                    return;
                }
            }
        }
        catch { }

        //right
        try
        {
            if (Global.board[row_position, col_position + 1] == 0)
            {
                empty_position = new Vector3(transform.position.x, 0, transform.position.z + GlobalVars.z_offset);
                new_row = row_position;
                new_col = col_position + 1;
                PlaySound();
                can_move = true;
                //if (empty_position != old_position)
                //{
                //    old_position = new Vector3(transform.position.x, 0, transform.position.z);
                //    return;
                //}
                old_position = new Vector3(transform.position.x, 0, transform.position.z);

                if (preferredMove == PreferredMove.Right)
                {
                    return;
                }
            }
        }
        catch { }

        //down
        try
        {
            if (Global.board[row_position + 1, col_position] == 0)
            {

                empty_position = new Vector3(transform.position.x + GlobalVars.x_offset, 0, transform.position.z);
                new_row = row_position + 1;
                new_col = col_position;
                PlaySound();
                can_move = true;
                //if (empty_position != old_position)
                //{
                //    old_position = new Vector3(transform.position.x, 0, transform.position.z);
                //    return;
                //}
                old_position = new Vector3(transform.position.x, 0, transform.position.z);

                if (preferredMove == PreferredMove.Down)
                {
                    return;
                }
            }
        }
        catch { }

    }
    void FindOnBoard()
    {
        for (int row = 0; row < Global.x_size; row++)
        {
            for (int col = 0; col < Global.y_size; col++)
            {
                if (Global.board[row, col] == number_chip)
                {
                    row_position = row;
                    col_position = col;
                }
            }
        }
    }
}
