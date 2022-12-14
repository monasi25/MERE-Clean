using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//メインゲームシーンのテキストや状態管理などを行うクラス
public class GameDirector : MonoBehaviour
{
    private double Percent;     //綺麗度

    [SerializeField] private GameObject Player;     //プレイヤーオブジェクト

    [SerializeField]
    private GameObject moneytext;                   //所持金テキスト
    double yogore = 0;                              //ヨゴレをいくつ掃除したか
    [SerializeField]
    private GameObject kireidotext;                 //綺麗度テキスト

    [SerializeField] private GameObject nabitext;   //ナビゲーションテキスト

    double hokori = 0;                              //ホコリをいくつ掃除したか
    public static int money = 5000;                 //所持金
    private AudioSource aud;

    [SerializeField] private AudioClip CleanPerfect,allPerfect;

    [SerializeField] private Text PerfectText;      //クリア時テキスト
    public cleaner_Move cleaner_Move;

    [SerializeField] movetest4 movetest4;
    [SerializeField] CleanerSwitch CleanerSwitch;
    [SerializeField] kabenobori1 kabenobori1;
    [SerializeField] Talk_Ray Talk_Ray;

    [SerializeField] ShopManager ShopManager;
    [SerializeField] Souji_Manager Souji_Manager;
    [SerializeField] Brush brush;

    private double moneyplus = 18.75;               //綺麗度がこの値を超えると所持金増加

    //プレイヤーの状態を表すenum
    public enum Function_state
    {
        all,
        sojiki,
        kabe,
        talk,
        ON,
        none,
    }

    public Function_state Fn_State;

    //コインに衝突すると所持金を100円加算する関数
    public void GetCoin()
    {
        money += 100;
    }

    //ホコリ掃除完了時に綺麗度を加算する関数
    public void Cleaner()
    {
        aud.PlayOneShot(CleanPerfect);
        hokori += 5.5;
    }

    //ヨゴレ掃除完了時に綺麗度を加算する関数
    public void Yogore()
    {
        aud.PlayOneShot(CleanPerfect);
        yogore += 5.5;
    }
    
    //Fn_Stateの内容によってプレイヤーの機能を無効化、有効化する関数
    public void FunctionState(Function_state off_State)
    {
        switch (off_State)
        {
            //全ての機能を有効化
            case Function_state.ON:                
                movetest4.enabled = true;
                kabenobori1.enabled = true;
                CleanerSwitch.enabled = true;
                Talk_Ray.enabled = true;
                break;

            //kabenobori1以外のクラスを無効化
            case Function_state.kabe:
                movetest4.enabled = false;
                Talk_Ray.enabled = false;
                CleanerSwitch.enabled = false;  
                break;

            //会話と壁登りを無効化
            case Function_state.sojiki:
                Talk_Ray.enabled = false;
                kabenobori1.enabled = false;
                break;

            //会話以外のクラスを無効化        
            case Function_state.talk:
                movetest4.enabled = false;
                kabenobori1.enabled = false;
                CleanerSwitch.enabled = false;
                break;

            //全ての機能を無効化
            case Function_state.all:
                movetest4.enabled = false;
                kabenobori1.enabled = false;
                CleanerSwitch.enabled = false;
                Talk_Ray.enabled = false;
                break;

            //なにもしない
            case Function_state.none:
                break;
        }

    }

    
    void Start()
    {
        Fn_State = Function_state.none;
        aud = GetComponent<AudioSource>();

        //メインゲームシーンを読み込んだ時、Keepがtrueなら復元処理を行う
        if (ValueSave.Keep == true)
        {
            Player.transform.position = ValueSave.PlayerPos_save;       //プレイヤーの位置
            money = ValueSave.money_save;                               //所持金
            hokori = ValueSave.hokori_save;                             //掃除完了したホコリの数
            yogore = ValueSave.yogore_save;                             //掃除完了したヨゴレの数
            ValueSave.yogore_State_save.CopyTo(Souji_Manager.Yogore_DestoryNum, 0);         //Souji_Managerが持つDestroyNum配列にyogore_stateの内容をコピー
            ValueSave.hokori_State_save.CopyTo(Souji_Manager.Hokori_DestroyNum, 0);         //同様
            Souji_Manager.DestroyFn();              //掃除が完了しているオブジェクトをDestroyNumの内容から判断して削除する

            if (ValueSave.WallUp == true)       //壁登りチップを購入している場合、壁登り解放
            {
                ShopManager.wallUPState = ShopManager.WallUPState.sold;
            }

            if(ValueSave.PowerUp == true)           //掃除パワーアップチップを購入している場合、各値を変更
            {
                ShopManager.powerState = ShopManager.PowerState.sold;
                brush.brushWidth = 120;
                brush.brushHeight = 60;
                brush.UpdateBrushColor();
            }

            FunctionState(Function_state.ON);
            ValueSave.Keep = false;
        }
    }

    // Update is called once per frame
    void Update()
    {        
        //壁登りを購入していな場合、ここで機能を制限される
        if (ShopManager.wallUPState == ShopManager.WallUPState.buy)
        {           
            kabenobori1.enabled = false;
        }

        //綺麗度が特定の値を超える度に所持金を増加
        if(moneyplus <= Percent)
        {
            money += 1000;
            moneyplus += 18.75;
        }

        Percent = hokori + yogore;  //ホコリの掃除完了数とヨゴレの掃除完了数を足して綺麗度を計算
        
        //綺麗度99%でクリア
        if(Percent == 99)
        {
            hokori += 1;
            PerfectSyori();
        }

        kireidotext.GetComponent<Text>().text = "お部屋の綺麗度：" + Percent.ToString() + "%";
        moneytext.GetComponent<Text>().text = "所持金：" + money.ToString() + "円";
        nabi(0);

        Cursor.lockState = CursorLockMode.Confined; //カーソルが画面内から出ないように設定
    }

    //クリア時に呼ばれる関数
    void PerfectSyori()
    {
        PerfectText.text = "綺麗度１００％達成おめでとう～";
        aud.PlayOneShot(allPerfect);
        StartCoroutine(FadeOut());
    }


    //クリア時に画面中央に表示される文字を徐々にフェードアウトさせるs
    private IEnumerator FadeOut()
    {
        for (float p = 1f; p >= 0; p -= 0.01f)
        {          
            PerfectText.color = new Color(1f, 1f, 1f, p);
            yield return new WaitForSeconds(0.1f);
        }
    }

    //ナビゲーションテキストをnabiの値に応じて表示させる関数
    public void nabi(int i)
    {
        switch (i)
        {
            case 0:
                nabitext.GetComponent<Text>().text = "";
                break;

            case 1:
                nabitext.GetComponent<Text>().text = "Eを押して話す";
                break;

            case 2:
                nabitext.GetComponent<Text>().text = "Eを押して登る";
                break;

            case 3:
                nabitext.GetComponent<Text>().text = "Spaceで手を離す";
                break;
        }
    }

    public static int Money()
    {
        return money;
    }

    //シーン遷移時に各値をstatic変数に保存する関数
    public void Save()
    {
        ValueSave.PlayerPos_save = Player.transform.position;
        ValueSave.money_save = money;
        ValueSave.yogore_save = yogore;
        ValueSave.hokori_save = hokori;
        Souji_Manager.Yogore_DestoryNum.CopyTo(ValueSave.yogore_State_save, 0);
        Souji_Manager.Hokori_DestroyNum.CopyTo(ValueSave.hokori_State_save, 0);
        ValueSave.Keep = true;
    }

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Application.targetFrameRate = 60;       //フレームレートを60に固定
    }
}
