using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ミニレナ(ミニゲームMOB)との会話表示、ミニゲームシーンに遷移する処理等を行うスクリプト
public class CasinoTrans : MonoBehaviour
{

    [SerializeField] private string[] talks;        //ミニレナとの会話内容を入れる配列
    private string[] words;                         //talksの文字列を一文字区切りにした文字を入れる配列
    [SerializeField] private Text text;             //Textオブジェクト

    [SerializeField] private Talk_Ray Talk_Ray;     
    private bool finish = false;                    //会話全内容表示終了後にtrue
    private bool flag = false;
    private bool brink = false;                     //画像を点滅させるかどうかの変数
    private bool confirmApperar = false;
    private bool load = false;                      //ミニゲームシーンへの遷移中にtrue
    public int talkNum = 0;                         //表示する会話内容を指定する変数

    [SerializeField] private FootSound foot;        //足音で使用している効果音クラスを流用
    [SerializeField] private GameObject MouseImage;　//クリックの画像
    [SerializeField] private GameObject ConfirmUI;      //選択肢
    [SerializeField] private Image Triangle;        //選択肢の横に表示される三角形

    [SerializeField] private GameDirector GameDirector;

    private AudioSource aud;
    [SerializeField] private AudioClip Click,BUU;   //効果音

    //三角形の位置を表すenum
    enum TriPos
    {
        Up,
        Down,
    }

    TriPos tri = TriPos.Up;

   
    void Start()
    {
        aud = this.GetComponent<AudioSource>();
    }

    
    void Update()
    {
        //ロード中は誤作動防止のため処理を丸ごと止める
        if (load == false)
        {
            if ((Input.GetMouseButtonDown(0)) && (confirmApperar == false))     //選択肢が表示されていない時に行う処理
            {
                //最初の会話内容でクリックした時に処理される
                if (flag == false)
                {
                    text.text = "";
                    MouseImage.SetActive(false);
                    ConfirmUI.SetActive(true);      //選択肢を表示させる
                    flag = true;
                    confirmApperar = true;          //選択肢表示状態に
                    brink = false;
                }

                //いいえを選択した場合処理される
                if (finish == true)
                {
                    text.text = "";
                    Talk_Ray.end = true;
                    MouseImage.SetActive(false);
                    flag = false;
                    finish = false;
                    brink = false;
                }
            }

            if (confirmApperar == true)     //選択肢が表示されている時に処理される
            {
                if (Input.GetKeyDown(KeyCode.W))        //三角形を動かして選択肢を決める
                {
                    Triangle.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, 0);
                    tri = TriPos.Up;
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    Triangle.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, -50);
                    tri = TriPos.Down;
                }

                if (Input.GetKeyDown(KeyCode.Space))        //決定ボタン
                {
                    if (tri == TriPos.Up)                   //はいで決定ボタンを押した場合
                    {
                        if (GameDirector.money > 100)       //お金がない状態でミニゲームシーンに遷移するのを防ぐ
                        {
                            aud.PlayOneShot(Click);
                            load = true;
                            GameDirector.Save();            //メインゲームシーンをセーブ
                            FadeManager.Instance.LoadScene("RacePlayerSelect", 2.0f);   //遷移
                        }

                        else
                        {
                            aud.PlayOneShot(BUU);       //「お金が足りません」SEが流れる
                        }
                    }

                    if (tri == TriPos.Down)         //いいえで決定ボタンを押した場合
                    {
                        aud.PlayOneShot(Click);
                        ConfirmUI.SetActive(false);     //確認テキストを非表示に
                        confirmApperar = false;
                        MouseButtonClicked2();
                    }
                }
            }

            if (brink == true)
            {
                MouseImage.SetActive(true);
            }
        }
    }

    public void MouseButtonClicked2()
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
            foot.FootStep();                        //一文字表示する度に効果音再生
            yield return new WaitForSeconds(0.1f);
        }

        // 次のセリフがある場合には、トークボタンを表示する。
        if (talkNum + 1 < talks.Length)
        {
            flag = false;
        }

        else               //会話終了判定 
        {
            Debug.Log("haitta");
            finish = true;
        }

        brink = true;

        // 次のセリフをセットする。
        talkNum = talkNum + 1;
    }
}
