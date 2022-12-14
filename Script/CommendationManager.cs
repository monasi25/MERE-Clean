using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//払戻金計算など結果を処理するクラス
public class CommendationManager : MonoBehaviour
{
    [SerializeField] private Transform[] Rank;  //順位に応じた位置

    private GameObject[] Obj;       //ミニレナが入る
    private GameObject First;       //1位のミニレナが入る
    [SerializeField] private GameObject LastUI;     //払戻金などを確認する結果画面
    [SerializeField] private Text Pay,Allmoney;     //テキスト
    private float Paymoney;     //払戻金
    private AudioSource aud;
    [SerializeField] private AudioClip Click, BUU;

    private bool once;

    // Start is called before the first frame update
    void Start()
    {
        aud = this.GetComponent<AudioSource>();
        Obj = new GameObject[4];
        for(int i = 0; i < 4; i++)
        {
            Obj[i] = GameObject.Find(SelectManager.ObjName[i]);     //レースシーンから遷移してきたミニレナを格納

            //ミニレナが持つ順位に応じて位置を決定
            switch (Obj[i].GetComponent<RacePlayerStatus>().rank)
            {
                //4位
                case 0:
                    Obj[i].transform.position = Rank[0].position;
                    Obj[i].transform.rotation = Rank[0].rotation;
                    break;

                case 1:
                    Obj[i].transform.position = Rank[1].position;
                    Obj[i].transform.rotation = Rank[1].rotation;
                    break;

                case 2:
                    Obj[i].transform.position = Rank[2].position;
                    Obj[i].transform.rotation = Rank[2].rotation;
                    break;

                //1位
                case 3:
                    Obj[i].transform.position = Rank[3].position;
                    Obj[i].transform.rotation = Rank[3].rotation;
                    First = Obj[i];
                    break;
            }
        }

        Paymoney = First.GetComponent<RacePlayerStatus>().money * First.GetComponent<RacePlayerStatus>().Odds;      //払戻金計算
        ValueSave.money_save += (int)Paymoney;      //所持金に払戻金を加算
    }

    // Update is called once per frame
    void Update()
    {
        //結果確認画面表示
        if (Input.GetMouseButtonDown(0) && once == false)
        {
            LastUI.SetActive(true);
            Pay.text = Paymoney.ToString() + "円";       //払戻金表示
            Allmoney.text = ValueSave.money_save.ToString() + "円";      //所持金表示
            once = true;
            aud.PlayOneShot(Click);
        }    
    }

    //もう一度ミニゲームをプレイする場合呼ばれる関数
    public void ReturnSelecet()
    {
        //お金を持っているか
        if (ValueSave.money_save > 100)
        {
            for (int i = 0; i < 4; i++)
            {
                Destroy(Obj[i]);    //今回使用したミニレナを削除
            }
            aud.PlayOneShot(Click);
            FadeManager.Instance.LoadScene("RacePlayerSelect", 2.0f);       //BET画面に遷移
        }

        else
        {
            aud.PlayOneShot(BUU);       //「お金が足りません」SEが流れる
        }
    }

    //メインゲームシーンに戻る場合呼ばれる関数
    public void ReturnGame()
    {
        for(int i = 0; i < 4; i++)
        {
            Destroy(Obj[i]);
        }
        aud.PlayOneShot(Click);
        FadeManager.Instance.LoadScene("Apartment", 2.0f);
    }
}
