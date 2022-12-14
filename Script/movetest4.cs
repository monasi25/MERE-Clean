using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//このスクリプトではプレイヤーの移動処理等を扱っています

public class movetest4 : MonoBehaviour
{
    public AudioClip coinSE; //効果音系変数
    AudioSource aud;

    public bool on = false;


    GameObject director;   //UI更新

    private CharacterController charaCon;
    private Animator animCon;  //  アニメーションするための変数
    private Vector3 moveDirection = Vector3.zero;   //  移動する方向とベクトルの変数
    
    public float idoSpeed = 0.5f;         // 移動速度
    [SerializeField]
    private float rotateSpeed = 3.0F;     // 向きを変える速度

    
    public float kaitenSpeed = 800.0f;   // プレイヤーの回転速度
    private float gravity = 20.0F;   //重力の強さ
    
    //public float jumpPower = 6.0F;

    private Transform pare;  //ルンバを親にする時の変数
    
    public GameObject cube;　//テスト用キューブ

    public bool take = false;　//ルンバのスクリプト引継ぎ時のbool変数

    // Start is called before the first frame update
    void Start()
    {
        charaCon = GetComponent<CharacterController>();
        animCon = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        director = GameObject.Find("GameDirector");
        //pare = GameObject.Find("runba").transform;    
    }

    // Update is called once per frame
    void Update()
    {
        var cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;  //  カメラが追従するための動作
        Vector3 direction = cameraForward * Input.GetAxis("Vertical") + Camera.main.transform.right * Input.GetAxis("Horizontal");  //  テンキーや3Dスティックの入力（GetAxis）があるとdirectionに値を返す

        charaCon.Move(moveDirection * Time.deltaTime);  //CharacterControllerの付いているこのオブジェクトを移動させる処理

        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)  //  テンキーや3Dスティックの入力（GetAxis）がゼロの時の動作
        {
            on = false;
            aud.Stop();
            animCon.SetBool("iswalk", false);  //  Runモーションしない
        }

        /*if (Input.GetKeyDown(KeyCode.P))
        {
            FadeManager.Instance.LoadScene("RacePlayerSelect", 2.0f);
        }*/

        else //  テンキーや3Dスティックの入力（GetAxis）がゼロではない時の動作
        {
            on = true;
            MukiWoKaeru(direction);  //  向きを変える動作の処理を実行する
            animCon.SetBool("iswalk", true);  //  Runモーションする
        }


        // 落下処理
        if (charaCon.isGrounded)    //CharacterControllerの付いているこのオブジェクトが接地している場合の処理
        {
            moveDirection.y = 0f;  //Y方向への速度をゼロにする
            moveDirection = direction * idoSpeed;  //移動スピードを向いている方向に与える

            //  このスケール感のゲームにジャンプ機能載せると酔う 実装取りやめ

            /*  if (Input.GetKeyDown("space") || Input.GetButtonDown("Jump")) //Spaceキーorジャンプボタンが押されている場合
             {
                 // moveDirection.y = jumpPower; //Y方向への速度に「ジャンプパワー」の変数を代入する
                 
             }*/
            /* else //Spaceキーorジャンプボタンが押されていない場合
             {
                 moveDirection.y -= gravity * Time.deltaTime; //マイナスのY方向（下向き）に重力を与える（これを入れるとなぜかジャンプが安定する）
             }*/


        }
        else  //CharacterControllerの付いているこのオブジェクトが接地していない場合の処理
        {
            moveDirection.y -= gravity * Time.deltaTime;  //マイナスのY方向（下向き）に重力を与える
        }


    }

    void OnTriggerEnter(Collider other)   //コインとのあたり判定
    {
        if (other.gameObject.tag == "coin")
        {
            director.GetComponent<GameDirector>().GetCoin(); //コインと衝突したら100円プラスする
            aud.PlayOneShot(coinSE); //コインSE再生
            Debug.Log("coin get");
            Destroy(other.gameObject); //コインを取得後は削除
        }

    }

    void OnTriggerStay(Collider other)   //ルンバ乗り込み  //実装取りやめ
    {
        if (other.gameObject.tag == "runba")　
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                this.transform.SetParent(pare);　//ルンバを親に設定
               　
                take = true;　//ルンバに引継ぎ               
            }
        }
    }

    void MukiWoKaeru(Vector3 mukitaiHoukou)
    {
        Quaternion q = Quaternion.LookRotation(mukitaiHoukou);          // 向きたい方角をQuaternion型に直す
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, kaitenSpeed * Time.deltaTime);   // 向きを q に向けて変化させる.
    }
   
    public void WalkFalse()
    {
        animCon.SetBool("iswalk", false);
    }
}
