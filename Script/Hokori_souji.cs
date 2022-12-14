using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ホコリの掃除処理を行うクラス　各ホコリオブジェクトにアタッチされている
public class Hokori_souji : MonoBehaviour
{
    private GameObject Hokori;
    private Texture2D Tex;
    private Renderer rend;
    public Brush brush;     //ブラシ

    [SerializeField] private Souji_Manager Souji_Manager;
    [SerializeField] private GameDirector director;
    [SerializeField] private GameObject HokoriEfect;

    private int myNumber;   //管理クラスが保持するオブジェクトの何番目かを保持する変数
    private float FinPer = 94f; //掃除完了とするパーセント
    
    // Start is called before the first frame update
    void Start()
    {
        Hokori = this.gameObject;
        rend = Hokori.GetComponent<Renderer>();

        //オブジェクトにセットしてあるテクスチャと同じサイズのテクスチャを作成
        Tex = new Texture2D(Hokori.GetComponent<Renderer>().material.mainTexture.width, Hokori.GetComponent<Renderer>().material.mainTexture.height, TextureFormat.RGBA32, false);
        //Texに入れたホコリのテクスチャをコピー
        Graphics.CopyTexture(Hokori.GetComponent<Renderer>().material.mainTexture, 0, 0, Tex, 0, 0);

        brush.UpdateBrushColor();   //塗る色を透明に

        //Souji_ManagerのHokoti_Child配列に格納されているオブジェクトと自分のオブジェクトをリンクさせる
        for (int i = 0; i < Souji_Manager.Hokori_Parent.transform.childCount; i++)
        {
            if (Hokori == Souji_Manager.Hokori_Child[i])
            {
                myNumber = i;   //自分の番号を保持しておく
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //自分の番号のフラグが立つと掃除機のノズルが当たった所を透明にする処理を始める
        if(Souji_Manager.Hokori_Detection[myNumber] == true)   
        {
            //ピクセル座標に変換
            Souji_Manager.pixelUV.x *= Tex.width;
            Souji_Manager.pixelUV.y *= Tex.height;

            Tex.SetPixels((int)Souji_Manager.pixelUV.x - brush.brushWidth / 2, (int)Souji_Manager.pixelUV.y - brush.brushHeight / 2, brush.brushWidth, brush.brushHeight, brush.colors);
            Tex.Apply();
            rend.material.mainTexture = Tex;
            HokoriCheck();
        }
    }

    //全体の何%透明かをチェックし特定の値を超えると掃除完了とする関数
    void HokoriCheck()
    {
        int count = 0;
        var magnification = 100; //百分率に拡大用

        //ピクセル数を取得
        var pixels = Tex.GetPixels(0, 0, Tex.width, Tex.height);

        //ピクセルを一つずつ透明か確認
        foreach (var pixel in pixels)
        {
            if (pixel.a == 0f)
            {
                count++;
            }
        }
        
        //何%透明かを計算
        var pixelAlpha1 = (float)count / (float)pixels.Length;
        var numPixelAlpha1 = (pixelAlpha1 * magnification);

        if (numPixelAlpha1 >= FinPer)
        {
            Instantiate(HokoriEfect, Hokori.transform.position, HokoriEfect.transform.rotation);    //ホコリ掃除完了エフェクト
            Destroy(Hokori);    //オブジェクトを削除
            Souji_Manager.Hokori_DestroyNum[myNumber] = true;   //削除フラグを立ててシーン遷移を行っても消えておくようにする
            director.Cleaner(); //綺麗度加算
        }
    }
}
