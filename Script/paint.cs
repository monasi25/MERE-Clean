using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paint : MonoBehaviour
{
    Texture2D mDrawTexture;
    Color[] mBuffer;
    Texture2D mMainTexture;

    [SerializeField] float mSize = 20f;     // ブラシのサイズ
    //[SerializeField] RegionText mRegionText;        // テキストUIに付与しているスクリプトを参照

    [SerializeField]
    private GameObject _brushObj;

    void Start()
    {
        mMainTexture = (Texture2D)GetComponent<Renderer>().material.mainTexture;
        Color[] pixels = mMainTexture.GetPixels();

        mBuffer = new Color[pixels.Length];
        pixels.CopyTo(mBuffer, 0);
        mDrawTexture = new Texture2D(mMainTexture.width, mMainTexture.height, TextureFormat.RGBA32, false);
        mDrawTexture.filterMode = FilterMode.Point;
    }


    public void Draw(Vector2 p)
    {
        Color color = new Color(1f, 1f, 1f, 0f);

        for (int x = 0; x < mDrawTexture.width; x++)
        {
            for (int y = 0; y < mDrawTexture.height; y++)
            {
                if ((p - new Vector2(x, y)).magnitude < mSize)
                {
                    mBuffer.SetValue(color, x + mMainTexture.width * y);
                }
            }
        }
    }

    void Check()
    {
        var pixels = mDrawTexture.GetPixels(0, 0, mDrawTexture.width, mDrawTexture.height);
        int count = 0;      // ピクセルのアルファ値が0(透明)のピクセル数を格納

        foreach (var pixel in pixels)
        {
            if (pixel.a == 0f)
            {
                count++;
            }
        }
        var pixelTexture = pixels.Length / pixels.Length;           // テクスチャのサイズを1とする
        var pixelAlpha = (float)count / (float)pixels.Length;       // 透明に塗られたピクセル数を0から1までの範囲で求める

        // この時点でテクスチャのピクセルに対して透過にした割合を0から1までの範囲で設定できていますが
        // UIの表示用に値を0から100までの範囲に広げます
        // 
        // 単純に全部透過したかを確認したい場合は以下のようにすれば判定できます
        // if(pixelAlpha >= 1)  { 全部塗った時の処理 }

        var magnification = 100;        // 倍率
        var numPixelTexture = (pixelTexture * magnification);           // テクスチャのサイズを100にする
        var numPixelAlpha = (pixelAlpha * magnification);               // 透過率を0から100までの範囲にする
        if (numPixelAlpha >= 100f)
        {
            Debug.Log("掃除完了");
        }

        string text = $"{numPixelAlpha.ToString("N2")} / {numPixelTexture.ToString("N2")}";
       // mRegionText.SetText(text);      // テキストUIに値を設定

    }

    void Update()
    {
       // if (Input.GetMouseButton(0))
        
            Ray ray = new Ray(_brushObj.transform.position, new Vector3(0, -1, 0));
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                var vec = new Vector2(hit.textureCoord.x * mMainTexture.width, hit.textureCoord.y * mMainTexture.height);
                Draw(vec);
            }

            mDrawTexture.SetPixels(mBuffer);
            mDrawTexture.Apply();
            GetComponent<Renderer>().material.mainTexture = mDrawTexture;

           // Check();
        
    }

}