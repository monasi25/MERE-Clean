using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    public int brushWidth;
    public int brushHeight;
    public Color color = Color.clear;

    public Color[] colors { get; set; }

    public void UpdateBrushColor()
    {
        colors = new Color[brushWidth * brushHeight];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }
    }

    private void Update()
    {
        Debug.Log(brushHeight);
        Debug.Log(brushWidth);
    }
}
