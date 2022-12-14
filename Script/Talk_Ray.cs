using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//会話可能なMOBとの検知処理、会話を行うための準備を行うスクリプト
public class Talk_Ray : MonoBehaviour
{
    [SerializeField] private GameObject shootpoint; //Rayをこのオブジェクトから飛ばす

    [SerializeField] private ShopScript ShopScript;     //ショップ店員との会話内容に関する処理を行うスクリプト

    [SerializeField] private GameDirector GameDirector;

    [SerializeField] private GameObject CameraStop;     //カメラオブジェクト

    [SerializeField] private CasinoTrans CasinoTrans;   //ミニゲームシーンに遷移するための処理を行うスクリプト    
    private float distance = 0.15f;         //レイの衝突を検知する距離
    private RaycastHit hit;
    public bool end = false;    //会話終了時にtrue

    private Animator animCon;

    //会話中かどうかを判断するenum
    enum Talk_State
    {
        standby,
        Talking,
    }
    Talk_State state;

    // Start is called before the first frame update
    void Start()
    {
        animCon = GetComponent<Animator>();
        state = Talk_State.standby;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(shootpoint.transform.position, shootpoint.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        if (Physics.Raycast(ray, out hit, distance))                                        //Rayがdistanceの範囲で何かに当たったら呼ばれる
        {
            //ショップ店員との会話時に処理される
            if (hit.collider.tag == "shop")                                                 
            {                
                switch (state)
                {
                    //レイがshopに当たっているだけの時に処理される
                    case Talk_State.standby:
                        GameDirector.nabi(1);       //「Eを押して会話」とUIに表示させる
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            GameDirector.FunctionState(GameDirector.Fn_State = GameDirector.Function_state.talk);   //プレイヤーの状態をトークモードに変更                       
                            ShopScript.enabled = true;      //ショップ店員にアタッチされているスクリプトを有効化し会話を始める
                            ShopScript.talkNum = 0;         //talkNumの値によって会話内容が変わる
                            ShopScript.MouseButtonClicked();    //マウスを押した時に呼ばれる関数をあらかじめ呼び出し最初の会話を出現させる
                            animCon.SetBool("iswalk", false);   //プレイヤーアニメーションをIDLE状態に
                            state = Talk_State.Talking;         //会話中に変更
                            GameDirector.nabi(0);               //UIからナビゲーションテキストを消す
                            CameraStop.SetActive(false);        //カメラの操作を停止
                        }
                        break;

                    //会話中に処理される
                    case Talk_State.Talking:

                        //会話が終了するとShopScriptがend変数をtrueに変更する
                        if(end == true) 
                        {
                            GameDirector.FunctionState(GameDirector.Fn_State = GameDirector.Function_state.ON);     //プレイヤーを通常モードに変更
                            ShopScript.enabled = false;     //ShopScriptを無効化し会話を完全に終了
                            state = Talk_State.standby;     
                            end = false;
                            CameraStop.SetActive(true);     //カメラを操作可能に
                        }
                        break;
                }
            }
            
            //ミニレナ(ミニゲーム)を行う時の確認会話時に処理される
            if(hit.collider.tag == "casino")
            {
                switch (state)
                {
                    case Talk_State.standby:
                        GameDirector.nabi(1);
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            GameDirector.FunctionState(GameDirector.Fn_State = GameDirector.Function_state.talk);
                            CasinoTrans.enabled = true;         //ミニレナにアタッチされているスクリプトを有効化し会話を始める
                            CasinoTrans.talkNum = 0;            //以下同様
                            CasinoTrans.MouseButtonClicked2();
                            animCon.SetBool("iswalk", false);
                            state = Talk_State.Talking;
                            GameDirector.nabi(0);
                            CameraStop.SetActive(false);
                        }
                        break;

                    case Talk_State.Talking:

                        //ミニゲームシーンに遷移しない場合endがtrueになる
                        if (end == true)
                        {
                            GameDirector.FunctionState(GameDirector.Fn_State = GameDirector.Function_state.ON);
                            CasinoTrans.enabled = false;
                            state = Talk_State.standby;
                            end = false;
                            CameraStop.SetActive(true);
                        }
                        break;
                }
            }
        }
    }
}
