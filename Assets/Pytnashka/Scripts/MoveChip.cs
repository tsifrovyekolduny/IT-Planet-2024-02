﻿using System;
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
        Up, Down, Left, Right, None
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
        if (Global.Get.test)
        {
            Completed();
        }
    }
    void OnMouseUp()
    {
        if (Global.Get.game_finished == false)
        {
            if (!can_move)
            {
                finalMousePos = Input.mousePosition;
                FindOnBoard();
                CalculateDirection();
                if (can_move)
                {
                    Global.Get.board[new_row, new_col] = number_chip;
                }
            }
        }
    }
    void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }
    void MotionCountPlus()
    {
        ++Global.Get.count;
        Global.Get.UpdateText();
        if(Global.Get.count == Global.Get.max_count_steps)
        {
            Global.Get.is_game_over = true;
            Completed();
        }
        //ui_motion.GetComponent<Text>().text = "КОЛИЧЕСТВО ХОДОВ\n\n " + Global.Get.count.ToString();
    }
    void MoveChipOnBoard()
    {
        if (transform.position != empty_position)
        {
            transform.position = Vector3.MoveTowards(transform.position, empty_position, speed * Time.deltaTime);
        }
        else
        {
            Global.Get.board[row_position, col_position] = 0;
            Global.Get.board[new_row, new_col] = number_chip;
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
        int maxAcceptCount = 16 - Global.Get.countEraceBlocks + 1;
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (Global.Get.board[row + Global.x_offset, col + Global.y_offset] == count)
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
        Global.Get.game_finished = true;
        Debug.Log("Уровень завершён");
        Global.Get.ramka.GetComponent<MeshRenderer>().enabled = true;
        Global.Get.kartinka.GetComponent<SpriteRenderer>().enabled = true;
        Global.Get.comod.GetComponent<MeshRenderer>().enabled = true;
        Global.Get.count_steps.enabled = false;
        if (Global.Get.is_game_over == false)
            Global.Get.oboi.GetComponent<MeshRenderer>().enabled = true;
        GameManager.Instance.CompleteLevel(SceneManager.GetActiveScene().name, 8, !Global.Get.is_game_over);
    }
    void CalculateDirection()
    {
        try
        {
            // Вычисляем предпочтительное
            mouseDelta = finalMousePos - initialMousePos;
            if (Math.Abs(mouseDelta.x) > Math.Abs(mouseDelta.y))
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


        }
        catch { }
        try
        {
            //left
            if (Global.Get.board[row_position, col_position - 1] == 0)
            {
                empty_position = new Vector3(transform.position.x, 0, transform.position.z - Global.Get.z_offset_f);
                new_row = row_position;
                new_col = col_position - 1;
                PlaySound();
                can_move = true;
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
            if (Global.Get.board[row_position - 1, col_position] == 0)
            {
                empty_position = new Vector3(transform.position.x - Global.Get.x_offset_f, 0, transform.position.z);
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
            if (Global.Get.board[row_position, col_position + 1] == 0)
            {
                empty_position = new Vector3(transform.position.x, 0, transform.position.z + Global.Get.z_offset_f);
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
            if (Global.Get.board[row_position + 1, col_position] == 0)
            {
                empty_position = new Vector3(transform.position.x + Global.Get.x_offset_f, 0, transform.position.z);
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
                if (Global.Get.board[row, col] == number_chip)
                {
                    row_position = row;
                    col_position = col;
                }
            }
        }
    }
}
