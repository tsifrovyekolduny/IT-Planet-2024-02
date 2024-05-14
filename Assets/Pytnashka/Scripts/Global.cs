using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Global : MonoBehaviour
{
    public static int count = 0;
    public static int countEraceBlocks = 0;//1-15
    public const int x_size = 6, y_size = 6, x_offset = 1, y_offset = 1;
    public static int[,] board = new int[x_size, y_size];
    public static GameObject ramka = null, kartinka = null, oboi = null, comod = null, camera = null;
    public static bool test = false;
}
