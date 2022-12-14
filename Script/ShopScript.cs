using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ショップ店員との会話表示、ショップ画面を出現させる等の処理を行うスクリプト
public class ShopScript : MonoBehaviour
{
    [SerializeField] private FootSound foot;    //足音で使用している効果音クラスを流用

    [SerializeField] private string[] talks;    //店員との会話内容を入れる配列
    private string[] words;                     //talksの文字列を一文字区切りにした文字を入れる配列
    [SerializeField] private Text text;         //Textオブジェクト

    [SerializeField] private Talk_Ray Talk_Ray;
    private bool finish = false;    //会話全内容表示終了後にtrue
    private bool flag = false;      //ショップ画面が出現しているかどうかを判断する変数
    private bool brink = false;     //画像を点滅させるかどうかの変数
    public int talkNum = 0;         //表示する会話内容を指定する変数

    [SerializeField] private GameObject MouseImage;     //クリックの画像
    [SerializeField] private UIManager UIManager;       //UIの出現等を管理するクラス

   
    void Update()
    {
        if ((Input.GetMouseButtonDown(0)))
        {
            //ショップ画面が出現していない かつ まだ会話内容が残っている時に処理される
            if(flag == false)
            {
                text.text = "";
                UIManager.ShopAppear();     //ショップ画面出現
                MouseImage.SetActive(false);    //クリック画像を無効化
                flag = true;        //ショップ画面が出現したためtrueに        
                brink = false;
            }

            //全ての会話内容を表示し終わった時に処理される
            if(finish == true)
            {
                text.text = "";
                Talk_Ray.end = true;    //Talk_Rayに会話が終わった事を知らせる
                flag = false;
                finish = false;
                brink = false;
                MouseImage.SetActive(false);
            }
        }

        if(brink == true)
        {
            MouseImage.SetActive(true); //クリック画像を表示、アタッチされているスクリプトによって有効化するだけで自動で点滅する
        }
    }

    //左クリックすると呼ばれる関数
    //会話開始時にスクリプトから呼び出すため、次に呼び出されるのはショップ画面を閉じて別れの挨拶を終える時のクリック
    public void MouseButtonClicked() 
    {
        text.text = "";
        StartCoroutine(Dialogue());
        flag = true;
    }


    IEnumerator Dialogue()
    {
        // 全角スペースで文字を分割する。
        words = talks[talkNum].Split('　');

        foreach (var word in words)
        {            
            text.text = text.text + word;          // 0.1秒刻みで１文字ずつ表示する。            
            foot.FootStep();                       //一文字表示する度に効果音再生
            yield return new WaitForSeconds(0.1f);
        }

        // 次のセリフがある場合には、トークボタンを表示する。
        if (talkNum + 1 < talks.Length)
        {
            flag = false;            
        }

        else               //会話終了判定 
        {           
            finish = true;
        }

        brink = true;

        // 次のセリフをセットする。
        talkNum = talkNum + 1;
    }
}
