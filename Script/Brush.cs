using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//このスクリプトではブラシの大きさと色（透明）を設定しています.
public class Brush : MonoBehaviour
{
    public int brushWidth;      //横
    public int brushHeight;     //縦
    public Color color = Color.clear;   //透明

    public Color[] colors { get; set; }

    //ブラシの色を設定する関数
    public void UpdateBrushColor()
    {
        colors = new Color[brushWidth * brushHeight];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }
    }
}
