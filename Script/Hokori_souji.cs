using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hokori_souji : MonoBehaviour
{
    private GameObject Hokori;
    private Texture2D Tex;
    private Renderer rend;
    public Brush brush;

    [SerializeField] private Souji_Manager Souji_Manager;
    [SerializeField] private GameDirector director;
    [SerializeField] private GameObject HokoriEfect;

    private int myNumber;
    private float FinPer = 94f;
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        Hokori = this.gameObject;
        rend = Hokori.GetComponent<Renderer>();

        Tex = new Texture2D(Hokori.GetComponent<Renderer>().material.mainTexture.width, Hokori.GetComponent<Renderer>().material.mainTexture.height, TextureFormat.RGBA32, false);
        Graphics.CopyTexture(Hokori.GetComponent<Renderer>().material.mainTexture, 0, 0, Tex, 0, 0);

        brush.UpdateBrushColor();

        for (int i = 0; i < Souji_Manager.Hokori_Parent.transform.childCount; i++)
        {
            if (Hokori == Souji_Manager.Hokori_Child[i])
            {
                myNumber = i;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Souji_Manager.Hokori_Detection[myNumber] == true)
        {
            Souji_Manager.pixelUV.x *= Tex.width;
            Souji_Manager.pixelUV.y *= Tex.height;

            Tex.SetPixels((int)Souji_Manager.pixelUV.x - brush.brushWidth / 2, (int)Souji_Manager.pixelUV.y - brush.brushHeight / 2, brush.brushWidth, brush.brushHeight, brush.colors);
            Tex.Apply();
            rend.material.mainTexture = Tex;
            HokoriCheck();
        }
    }

    void HokoriCheck()
    {
        int count = 0;
        var magnification = 100;

        var pixels = Tex.GetPixels(0, 0, Tex.width, Tex.height);
        foreach (var pixel in pixels)
        {
            if (pixel.a == 0f)
            {
                count++;
            }
        }
        var pixelTexture = pixels.Length / pixels.Length;
        var pixelAlpha1 = (float)count / (float)pixels.Length;
        var numPixelTexture1 = (pixelTexture * magnification);
        var numPixelAlpha1 = (pixelAlpha1 * magnification);
        if (numPixelAlpha1 >= FinPer)
        {
            Instantiate(HokoriEfect, Hokori.transform.position, HokoriEfect.transform.rotation);
            Destroy(Hokori);
            director.Cleaner();
        }
    }
}
