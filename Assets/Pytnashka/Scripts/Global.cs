using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Global : MonoBehaviour
{
    public static int count = 0;
    public const int x_size = 4, y_size = 4, x_offset = 0, y_offset = 0;
    public static int[,] board = new int[x_size, y_size];
}
