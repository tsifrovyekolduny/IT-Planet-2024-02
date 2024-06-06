using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Global {
    private static Global instance;
    public static Global Get
    {
        get
        {
            if (instance == null)
            {
                instance = new Global();
            }
            return instance;
        }
    }
    public static void ResetInstance()
    {
        Global g = new Global();
        instance = g;
    }
    public float alpha_value = 0.0f;

    public int count = 0;
    public int countEraceBlocks = 0;//1-15
    public const int x_size = 6, y_size = 6, x_offset = 1, y_offset = 1;
    public int[,] board = new int[x_size, y_size];
    public GameObject ramka = null, kartinka = null, oboi = null, comod = null, camera = null;
    public bool test = false;
    public int max_count_steps = 111;
    public TextMeshProUGUI count_steps;
    public bool is_game_over = false;

    // Start is called before the first frame update
    public float x_offset_f = 1.0f;
    public float z_offset_f = 1.0f;
    public GameObject[] chips;

    public bool game_finished = false;

    public void UpdateText()
    {
        String str = (max_count_steps - count).ToString();
        count_steps.text = str;
    }
}
