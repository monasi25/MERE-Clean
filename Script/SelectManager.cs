using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*レースゲームの仕様
・全11頭いるミニレナの内ランダムに4頭が選出される
・プレイヤーはこの4頭のうち一着(単勝)を予想する
・最低BET額は100円から
・4頭のうち単勝を購入できるのは2頭まで（オッズによっては全賭けする事でプレイヤーが必ず得をする事があるため）
・所持金を使い果たしてメインゲームシーンでチップを購入する事ができない詰みを防ぐため全体でのBET上限額は500円
・レースゲームのシーン遷移の順番はBETシーン→レースシーン→結果シーンの順
*/


//ミニレナにBETする等の処理行うクラス   BETシーンで使用されるクラス
public class SelectManager : MonoBehaviour
{
    [SerializeField] private GameObject RacePlayer_Parent;      //全てのミニレナをまとめる親オブジェクト
    private List<GameObject> RacePlayer = new List<GameObject>();   //ミニレナを入れるリスト
    private List<GameObject> usePlayer = new List<GameObject>();    //ミニゲームに登場するミニレナを入れるリスト
    private GameObject CenterPlayer;        //中央に位置するミニレナが入る

    private GameObject Obj;
 
    private int Count = 0;      //ミニレナの移動位置を決めるための変数
    private int Count2 = 0;     //BETした頭数をカウントする変数
    private int ChoiceNum;      //選ばれたミニレナの番号を保持する変数

    [SerializeField] private Transform[] MovePos; //0が真ん中,１が右,2が後ろ,3が左

    private Vector3[] target;       //各ミニレナが移動する位置を持つ変数

    private int BetMoney;   //全体BET額

    [SerializeField] private Text AllmoneyText,BETmoneyText,PiecemoneyText,speedText,DangerText,LuckText,NameText,OddsText; //テキスト
    [SerializeField] private GameObject KakuteiPanel;   //BET確定画面

    public static string[] ObjName;     //レースシーンで使用するミニレナの名前を保存する変数

    [SerializeField] private GameObject Panel;      //ミニゲーム中断画面
    [SerializeField] private GameObject[] ExplainPanel; //チュートリアル画面
    private bool flag;      //BET等の処理を止めるためのフラグ

    private AudioSource aud;
    [SerializeField] private AudioClip Click,BETUP,BETDOWN,PAGEOPEN,PAGECLOSE;      //効果音
    
    void Start()
    {
        aud = this.GetComponent<AudioSource>();
        ObjName = new string[4];
        target = new Vector3[4];

        for(int i = 0; i < RacePlayer_Parent.transform.childCount; i++)
        {
            RacePlayer.Add(RacePlayer_Parent.transform.GetChild(i).gameObject); //親オブジェクトの下にある子オブジェクトを一つずつ格納
        }

        for(int j = 0; j < 4; j++)
        {
            Obj = RacePlayer[Random.Range(0, RacePlayer.Count)];        //全11頭の内、出現させるのは4頭のみ 出現させるミニレナはランダム
            usePlayer.Add(Obj);                                         //出現させたミニレナをusePlayerリストに追加
            ChoiceNum = RacePlayer.IndexOf(Obj);                        
            RacePlayer.RemoveAt(ChoiceNum);                             //出現させたミニレナが再度ランダム関数で出現しないようにリストから削除

            usePlayer[j].SetActive(true);                               //選んだミニレナを有効化
            usePlayer[j].transform.position = MovePos[j].position;      //出現させたミニレナの位置を指定の場所に設定
            target[j] = MovePos[j].position;                            
        }
    }

    // Update is called once per frame
    void Update()
    {
        float step = 0.5f * Time.deltaTime;     

        for(int i = 0; i < 4; i++)
        {
            //何頭に現在BETしているかをカウント
            if (usePlayer[i].GetComponent<RacePlayerStatus>().Betflag == true)
            {
                Count2++;       
            }
        }

        //ミニレナが移動中でなく かつ チュートリアル画面等が表示されいなければ処理される
        if ((usePlayer[0].transform.position == target[0]) && (flag == false))
        {
            TargetChange();
            BetPlus();
            BetMinus();
            KakuteiUIappear();
            TextDisplay();
            CancelUIappear();
        }

        //ミニレナの位置を指定した場所に移動
        usePlayer[0].transform.position = Vector3.MoveTowards(usePlayer[0].transform.position, target[0], step);
        usePlayer[1].transform.position = Vector3.MoveTowards(usePlayer[1].transform.position, target[1], step);
        usePlayer[2].transform.position = Vector3.MoveTowards(usePlayer[2].transform.position, target[2], step);
        usePlayer[3].transform.position = Vector3.MoveTowards(usePlayer[3].transform.position, target[3], step);

        Count2 = 0;    
    }

    //ミニレナの移動位置を決定する関数
    void TargetChange()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            //Countの値に応じて移動位置を決定    時計回りにミニレナが移動
            switch (Count)
            {
                case 0:
                    target[0] = MovePos[1].position;
                    target[1] = MovePos[2].position;
                    target[2] = MovePos[3].position;
                    target[3] = MovePos[0].position;
                    Count++;
                    break;

                case 1:
                    target[0] = MovePos[2].position;
                    target[1] = MovePos[3].position;
                    target[2] = MovePos[0].position;
                    target[3] = MovePos[1].position;
                    Count++;
                    break;

                case 2:
                    target[0] = MovePos[3].position;
                    target[1] = MovePos[0].position;
                    target[2] = MovePos[1].position;
                    target[3] = MovePos[2].position;
                    Count++;
                    break;

                case 3:
                    target[0] = MovePos[0].position;
                    target[1] = MovePos[1].position;
                    target[2] = MovePos[2].position;
                    target[3] = MovePos[3].position;
                    Count = 0;
                    break;
            }
            
        }

        //反時計回りに移動
        if (Input.GetKeyDown(KeyCode.A))
        {
            switch (Count)
            {
                case 0:
                    target[0] = MovePos[3].position;
                    target[1] = MovePos[0].position;
                    target[2] = MovePos[1].position;
                    target[3] = MovePos[2].position;
                    Count = 3;
                    break;
                
                case 1:
                    target[0] = MovePos[0].position;
                    target[1] = MovePos[1].position;
                    target[2] = MovePos[2].position;
                    target[3] = MovePos[3].position;
                    Count--;
                    break;

                case 2:
                    target[0] = MovePos[1].position;
                    target[1] = MovePos[2].position;
                    target[2] = MovePos[3].position;
                    target[3] = MovePos[0].position;
                    Count--;
                    break;

                case 3:
                    target[0] = MovePos[2].position;
                    target[1] = MovePos[3].position;
                    target[2] = MovePos[0].position;
                    target[3] = MovePos[1].position;
                    Count--;
                    break;
            }
            
        }
    }

    //ミニレナにBETする関数
    void BetPlus()
    {
        //所持金が100円以上であり全体でのBET額が500円以下      ※BETしすぎて詰むことを防ぐため全体でのBET額を500円に設定
        if (ValueSave.money_save >= 100 && BetMoney < 500)
        {
            CenterPlayer = CenterPlayerInfo();      //中央にいるミニレナの情報を取得

            //WキーでBET
            if (Input.GetKeyDown(KeyCode.W))
            {
                //全体でのBETした頭数が2頭以下もしくは既にBETを100円以上しているミニレナはBETできる
                if (((Count2 < 2) || (CenterPlayer.GetComponent<RacePlayerStatus>().Betflag == true)))
                {
                    BetMoney += 100;    //BETは100円から
                    ValueSave.money_save -= 100;    //所持金を100円マイナス
                    aud.PlayOneShot(BETUP);     //効果音

                    CenterPlayer.GetComponent<RacePlayerStatus>().money += 100;     //ミニレナが持つステータスに100円プラス
                    CenterPlayer.GetComponent<RacePlayerStatus>().Betflag = true;   //一度BETされたことを示すflagをtrueに
                }
            }
        }
    }

    //BET額をマイナスにする関数
    void BetMinus()
    {
        //そもそもBETされていない場合は処理しない
        if (BetMoney > 0)
        {
            //Sキーでマイナス
            if (Input.GetKeyDown(KeyCode.S))
            {
                CenterPlayer = CenterPlayerInfo();
                
                //マイナスされる対象のBET額が0円なら処理しない
                if((CenterPlayer.GetComponent<RacePlayerStatus>().money > 0))
                {
                    aud.PlayOneShot(BETDOWN);
                    //対象のBET額を100円マイナス
                    CenterPlayer.GetComponent<RacePlayerStatus>().money -= 100;
                    BetMoney -= 100;            //全体BET額を100円マイナス
                    ValueSave.money_save += 100;    //所持金を100円加算

                    //もし減算した結果、対象のBET額が0になったらBETflagをfalseにしておく
                    if(CenterPlayer.GetComponent<RacePlayerStatus>().money == 0)
                    {
                        CenterPlayer.GetComponent<RacePlayerStatus>().Betflag = false;
                    }
                }                           
            }
        }
    }

    //ミニゲームのチュートリアル画面を表示させる関数
    public void ExplainUIappear()
    {
        aud.PlayOneShot(PAGEOPEN);
        ExplainPanel[0].SetActive(true);
        flag = true;    //チュートリアル画面を表示したためBET等の処理を止める
    }

    //チュートリアル画面の次のページを表示させる関数
    public void NextPage()
    {
        aud.PlayOneShot(Click);
        ExplainPanel[0].SetActive(false);
        ExplainPanel[1].SetActive(true);
    }

    //チュートリアル画面のページ切り替えを行う関数
    public void BackPage()
    {
        aud.PlayOneShot(Click);
        ExplainPanel[1].SetActive(false);
        ExplainPanel[0].SetActive(true);
    }

    //チュートリアル画面を非表示にする関数
    public void ExpainUIClose()
    {
        aud.PlayOneShot(PAGECLOSE);
        ExplainPanel[0].SetActive(false);
        ExplainPanel[1].SetActive(false);
        flag = false;       //チュートリアル画面を非表示にしたためBET等の処理を再開
    }

    //ミニゲームを中断するかどうかを確認する画面を表示
    void CancelUIappear()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            aud.PlayOneShot(Click);
            Panel.SetActive(true);
            flag = true;
        }
    }

    //キャンセルを選択した場合メインゲームシーンに遷移する
    public void Cancel()
    {
        aud.PlayOneShot(Click);
        ValueSave.money_save += BetMoney;       //BETしたお金を所持金に戻す
        FadeManager.Instance.LoadScene("Apartment", 2.0f);      //遷移
    }

    //中断確認画面を非表示にする関数
    public void CancelUIhide()
    {
        aud.PlayOneShot(Click);
        Panel.SetActive(false);
        flag = false;
    }

    //BETを確定する場合に確認画面を表示させる関数
    void KakuteiUIappear()
    {
        //1円もBETしていなければ確認画面は表示しない
        if(BetMoney > 0)
        {
            //Spaceで確認画面表示
            if (Input.GetKeyDown(KeyCode.Space))
            {
                aud.PlayOneShot(Click);
                KakuteiPanel.SetActive(true);
                flag = true;    //BET等の処理を止める
            }
        }
    }

    //確認画面を非表示にする関数
    public void KakuteiUIhide()
    {
        aud.PlayOneShot(Click);
        KakuteiPanel.SetActive(false);
        flag = false;       //BET等の処理を再開
    }

    //確認画面で確定ボタンを押した場合に処理される関数
    public void Kakutei()
    {
        for(int i = 0; i < 4; i++)
        {
            ObjName[i] = usePlayer[i].name;     //レースに参加するミニレナの名前を保存
            usePlayer[i].transform.parent = null;   //レースに参加するミニレナは親オブジェクトから切り離す
            DontDestroyOnLoad(usePlayer[i]);           //参加するミニレナがシーン遷移を行っても削除されないようにする
        }
        aud.PlayOneShot(Click);
        FadeManager.Instance.LoadScene("Race", 2.0f);       //レースシーン遷移
    }

    //BET画面でのあらゆる情報を画面に表示する関数
    void TextDisplay()
    {
        CenterPlayer = CenterPlayerInfo();
        AllmoneyText.text = "所持金：" + ValueSave.money_save.ToString() + "円";
        BETmoneyText.text = "合計BET額：" + BetMoney.ToString() + "円";
        PiecemoneyText.text = "BETした金額：" + CenterPlayer.GetComponent<RacePlayerStatus>().money.ToString() + "円";
        speedText.text = "スピード：" + (CenterPlayer.GetComponent<RacePlayerStatus>().speed * 100f).ToString();
        DangerText.text = "悪運：" + (CenterPlayer.GetComponent<RacePlayerStatus>().DangerValue - CenterPlayer.GetComponent<RacePlayerStatus>().SafeValue).ToString();
        LuckText.text = "テクニック：" + (100 - CenterPlayer.GetComponent<RacePlayerStatus>().DangerValue).ToString();
        NameText.text = "名前：" + CenterPlayer.GetComponent<RacePlayerStatus>().Name;
        OddsText.text = "オッズ：" + CenterPlayer.GetComponent<RacePlayerStatus>().Odds.ToString();
    }

    //中央のミニレナの情報を返す関数
    GameObject CenterPlayerInfo()
    {
        for(int i = 0; i < 4; i++)
        {
            if(target[i] == MovePos[0].position)
            {
                return usePlayer[i];
            }
        }

        return null;
    }
}
