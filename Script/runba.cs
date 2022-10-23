using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runba : MonoBehaviour
{
    public movetest4 move;
    public GameObject pla;
    private Rigidbody rb; 

    private bool ride = false;
    private bool riding = false;

    float h;
    float v;
    float moveSpeed = 1.5f;
    float rotatespeed = 800f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
           if(move.take == true)
           {
                pla.GetComponent<movetest4>().enabled = false;　//プレイヤーのmove残したままだとlocalpositionが上手く機能しない
                pla.GetComponent<kabenobori1>().enabled = false;
                pla.transform.rotation = Quaternion.LookRotation(transform.forward);　//ルンバの前方に体の向きを変更
                pla.transform.localPosition = new Vector3(0f, 0.313f, 0f);　//ルンバ乗り込み
            
                move.take = false;
            
                ride = true; //運転可能判定フラグ
           }

           if(ride == true)
           {
                h = Input.GetAxisRaw("Horizontal");　
                v = Input.GetAxisRaw("Vertical");
           }

           if((Input.GetKeyDown(KeyCode.Space)) && (ride == true)) //ルンバモード解除
           {
                rb.isKinematic = true; //物理動作停止
                ride = false;　
                pla.transform.parent = null; //playerオブジェクトをルンバから切り離す
                pla.GetComponent<kabenobori1>().enabled = true;　//各プログラムを有効化
                pla.GetComponent<movetest4>().enabled = true;
           }
    }

    void FixedUpdate()  //ridigbodyのような物理演算を伴う時は固定フレームのFixedUpdateの方が良いらしい　理由は知らんけど
    {
        if (ride == true)
        {
            rb.isKinematic = false;　//物理演算有効化

            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;　//カメラの向きを取得

            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * v + Camera.main.transform.right * h;

            // 移動方向にスピードを掛ける。
            rb.velocity = moveForward * moveSpeed;

            // キャラクターの向きを進行方向に
            if (moveForward != Vector3.zero)
            {
                Quaternion q = Quaternion.LookRotation(moveForward);          // 向きたい方角をQuaternion型に直す
                transform.rotation = Quaternion.RotateTowards(transform.rotation, q,rotatespeed * Time.deltaTime);   // 向きを q に向けてじわ～っと変化させる.   プレイヤーの回転と同じ              
            }
            
        }

    }
}

        //掃除の処理がいまいち分からん　検討しているのはcubeオブジェクトを荒い解像度で大量に出力してほこりを再現　それを通常通りのコライダ判定で掃除機の感じを再現
        //けどメモリ使用量がヤバくなりそう　2Dならスプライトマスクがあるけど、3Dでそれが使えるか分からん
        //ちびロボでいうヨゴレはSpriteで出力して、透過度を下げるだけでいけそう
        //あとアニメーション無限に作らないかんのダルすぎ　プログラム書くより時間かかるとか意味わからんて