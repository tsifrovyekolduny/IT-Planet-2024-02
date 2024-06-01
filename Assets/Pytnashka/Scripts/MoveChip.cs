using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;



public class MoveChip : MonoBehaviour
{
    public interface IStrategy
    {
        bool Move(MoveChip chip);
    }
    class MoveLeft : IStrategy
    { 
        bool IStrategy.Move(MoveChip chip)
        {
            if (Global.Get.board[chip._rowPosition , chip._colPosition - 1] == 0)
            {
                chip.empty_position = new Vector3(chip.transform.position.x, 0, chip.transform.position.z - Global.Get.z_offset_f);
                chip._newRow = chip._rowPosition;
                chip._newCol = chip._colPosition - 1;
                chip.PlaySound();
                chip.SwitchStateMoving();
                chip.old_position = new Vector3(chip.transform.position.x, 0, chip.transform.position.z);
                return true;
            }
            return false;
        }
    }

    class MoveUp : IStrategy
    {
        bool IStrategy.Move(MoveChip chip)
        {
            if (Global.Get.board[chip._rowPosition - 1, chip._colPosition] == 0)
            {
                chip.empty_position = new Vector3(chip.transform.position.x - Global.Get.x_offset_f, 0, chip.transform.position.z);
                chip._newRow = chip._rowPosition - 1;
                chip._newCol = chip._colPosition;
                chip.PlaySound();
                //_state = ChipState.Moving;
                chip.SwitchStateMoving();
                chip.old_position = new Vector3(chip.transform.position.x, 0, chip.transform.position.z);
            }
            return false;
        }
    }

    class MoveRight : IStrategy
    {
        bool IStrategy.Move(MoveChip chip)
        {
            if (Global.Get.board[chip._rowPosition, chip._colPosition + 1] == 0)
            {
                chip.empty_position = new Vector3(chip.transform.position.x, 0, chip.transform.position.z + Global.Get.z_offset_f);
                chip._newRow = chip._rowPosition;
                chip._newCol = chip._colPosition + 1;
                chip.PlaySound();
                chip.SwitchStateMoving();
                chip.old_position = new Vector3(chip.transform.position.x, 0, chip.transform.position.z);
                return true;
            }
            return false;
        }
    }

    class MoveDown : IStrategy
    {
        bool IStrategy.Move(MoveChip chip)
        {
            if (Global.Get.board[chip._rowPosition + 1, chip._colPosition] == 0)
            {
                chip.empty_position = new Vector3(chip.transform.position.x - Global.Get.x_offset_f, 0, chip.transform.position.z);
                chip._newRow = chip._rowPosition - 1;
                chip._newCol = chip._colPosition;
                chip.PlaySound();
                chip.SwitchStateMoving();
                chip.old_position = new Vector3(chip.transform.position.x, 0, chip.transform.position.z);
                return true;
            }
            return false;
        }
    }

    public class UnCertainDirection : IStrategy
    {
        List<PreferredMove> GetListAvailablesMoves(MoveChip chip)
        {
            List<PreferredMove> retValue = new List<PreferredMove>();
            if (Global.Get.board[chip._rowPosition, chip._colPosition - 1] == 0)
            {
                retValue.Add(PreferredMove.Left);
            }
            if (Global.Get.board[chip._rowPosition - 1, chip._colPosition] == 0)
            {
                retValue.Add(PreferredMove.Up);
            }
            if (Global.Get.board[chip._rowPosition, chip._colPosition + 1] == 0)
            {
                retValue.Add(PreferredMove.Right);
            }
            if (Global.Get.board[chip._rowPosition + 1, chip._colPosition] == 0)
            {
                retValue.Add(PreferredMove.Down);
            }
            return retValue;
        }
        public bool Move(MoveChip chip)
        {
            bool retValue = false;
            IStrategy strategy = null;
            List<PreferredMove> listAvailableMoves = GetListAvailablesMoves(chip);

            if(listAvailableMoves.Count > 0)
            {
                chip.preferredMove = listAvailableMoves.First();
            }
            else
            {
                chip.preferredMove = PreferredMove.None;
            }

            switch (chip.preferredMove)
            {
                case PreferredMove.Up:
                    strategy = new MoveUp();
                    break;
                case PreferredMove.Down:
                    strategy = new MoveDown();
                    break;
                case PreferredMove.Left:
                    strategy = new MoveLeft();
                    break;
                case PreferredMove.Right:
                    strategy = new MoveRight();
                    break;
                case PreferredMove.None:
                    return false;
            }
            retValue = strategy.Move(chip);
            return retValue;
        }

    }
    

    public class CertainDirection : IStrategy
    {

        public bool Move(MoveChip chip)
        {    
            bool retValue = false;
            
            IStrategy strategy = null;
            switch (chip.preferredMove)
            {
                case PreferredMove.Up:
                    strategy = new MoveUp();
                    break;
                case PreferredMove.Down:
                    strategy = new MoveDown();
                    break;
                case PreferredMove.Left:
                    strategy = new MoveLeft();
                    break;
                case PreferredMove.Right:
                    strategy = new MoveRight();
                    break;
                case PreferredMove.None:
                    return false;
            }
            retValue = strategy.Move(chip);
            return retValue;
        }
    }
    enum PreferredMove
    {
        Up, Down, Left, Right, None
    };

    private PreferredMove preferredMove;

    private int _numberChip;
    private int _rowPosition;
    private int _colPosition;
    private int _speed;
    private Vector3 empty_position = new Vector3(0, 0, 0);
    private Vector3 old_position = new Vector3(-10, -10, -10);
    int _newRow, _newCol;

    //private GameObject ui_motion;
    //private GameObject ui_completed;

    //private bool can_move;
    private enum ChipState
    {
        Standing,
        Moving
    };

    ChipState _state = ChipState.Standing;
    void SwitchStateMoving()
    {
        _state = ChipState.Moving;
    }
    void SwitchStateStanding()
    {
        _state = ChipState.Standing;
    }

    private Vector3 initialMousePos;
    private Vector3 finalMousePos;
    private Vector3 mouseDelta;

    void Start()
    {
        _speed = 2;
        _numberChip = int.Parse(gameObject.name);
        //ui_motion = GameObject.Find("Motion");
        //ui_completed = GameObject.Find("Completed");
    }
    void Update()
    {
        if (_state == ChipState.Moving)
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
            if (_state == ChipState.Standing)
            {
                finalMousePos = Input.mousePosition;
                FindOnBoard();
                CalculateDirection();
                if (_state == ChipState.Moving)
                {
                    Global.Get.board[_newRow, _newCol] = _numberChip;
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
        if (Global.Get.count == Global.Get.max_count_steps)
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
            transform.position = Vector3.MoveTowards(transform.position, empty_position, _speed * Time.deltaTime);
        }
        else
        {
            Global.Get.board[_rowPosition, _colPosition] = 0;
            Global.Get.board[_newRow, _newCol] = _numberChip;
            //_state = ChipState.Standing;
            SwitchStateStanding();
             _rowPosition = _newRow;
            _colPosition = _newCol;
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
    PreferredMove GetPreferredMove()
    {
        PreferredMove preferredMove = PreferredMove.None;
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
        return preferredMove;
    }

    void CalculateDirection()
    {
        try
        {
            // Вычисляем предпочтительное
            preferredMove = GetPreferredMove();
        }
        catch { }
        
        try
        {
            //left
            if (Global.Get.board[_rowPosition, _colPosition - 1] == 0)
            {
                empty_position = new Vector3(transform.position.x, 0, transform.position.z - Global.Get.z_offset_f);
                _newRow = _rowPosition;
                _newCol = _colPosition - 1;
                PlaySound();
                SwitchStateMoving();
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
            if (Global.Get.board[_rowPosition - 1, _colPosition] == 0)
            {
                empty_position = new Vector3(transform.position.x - Global.Get.x_offset_f, 0, transform.position.z);
                _newRow = _rowPosition - 1;
                _newCol = _colPosition;
                PlaySound();
                //_state = ChipState.Moving;
                SwitchStateMoving();

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
            if (Global.Get.board[_rowPosition, _colPosition + 1] == 0)
            {
                empty_position = new Vector3(transform.position.x, 0, transform.position.z + Global.Get.z_offset_f);
                _newRow = _rowPosition;
                _newCol = _colPosition + 1;
                PlaySound();
                SwitchStateMoving();
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
            if (Global.Get.board[_rowPosition + 1, _colPosition] == 0)
            {
                empty_position = new Vector3(transform.position.x + Global.Get.x_offset_f, 0, transform.position.z);
                _newRow = _rowPosition + 1;
                _newCol = _colPosition;
                PlaySound();
                SwitchStateMoving();
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
                if (Global.Get.board[row, col] == _numberChip)
                {
                    _rowPosition = row;
                    _colPosition = col;
                }
            }
        }
    }
}
