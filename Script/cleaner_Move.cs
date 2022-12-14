using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cleaner_Move : MonoBehaviour
{

    public movetest4 movetest4;
    public CleanerSwitch cleanerswitch;
    public kabenobori1 kabenobori1;

    [SerializeField] CleanerSound CleanerSound;
    
    [SerializeField]
    private ParticleSystem Trail; //くるくるパーティクルが入る

    [SerializeField]
    private ParticleSystem suikomi; //吸引パーティクルが入る

    [SerializeField]
    private GameObject suiko; //吸引パーティクルのオブジェクト

    [SerializeField]
    private GameObject parti; //くるくるパーティクルのオブジェクト

    [SerializeField]
    private ParticleSystem Smoke; //スモークパーティクル

    [SerializeField]
    private GameObject smok; //スモークパーティクルオブジェクト

    public Transform onG; //地面につけるときの掃除機の位置
    public Transform taikiG; //掃除機を地面につけずに持っている時の掃除機の位置

    public bool flag = false; //Souji_Managerに掃除機が地面についているかどうかを知らせるための変数

    private Animator anim;

    private int count = -1; //掃除機を取り出した時に処理の順番を表す変数

    public GameObject so; //掃除機オブジェクトが入る変数

    private float i = 0; //for文用

    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //audi = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchON();       
    }

    private void SwitchON()
    {
        //掃除機を使用中の処理
        if (cleanerswitch.Cleaners == true)
        {
            //以下基本的に上から順番に処理されていく
            switch (count)
            {
                //掃除機を取り出す処理
                case -1:
                    StartCoroutine("wait");
                    StartCoroutine("toridasu");                   
                    //audi.PlayOneShot(hassan);
                    break;

                //ソージキを構えるための最初の処理 
                case 0:                                                            
                    anim.SetBool("isCleaner_taiki", true);
                    movetest4.idoSpeed = 0.27f;
                    so.transform.position = taikiG.position;        //ソージキのポジションとrotationを待機ポジに
                    so.transform.rotation = taikiG.rotation;                   
                    count = 1;                                      //countを1にしてcase1に遷移
                    break;

                //掃除機を地面につけず持っている時の処理
                case 1:                                             
                     movetest4.idoSpeed = 0.27f;  //掃除機を持っている時は移動速度をゆっくりに

                    //左クリックで掃除機を地面につける処理を行う
                    if ((Input.GetMouseButtonDown(0)))
                    {
                        //audi.PlayOneShot(kirikae);
                        anim.SetBool("isCleaner_IDLE", true);   //掃除機持ち待機状態
                        so.transform.position = onG.position;       //ソージキのポジションとrotationを構えるポジションに
                        so.transform.rotation = onG.rotation;
                        movetest4.idoSpeed = 0.2f;                  //移動速度をさらに遅くして掃除をしてる感を出す
                        flag = true; //掃除機を地面につけた事をSouji_Managerクラスに伝える
                        suiko.SetActive(true); //吸い込みパーティクルオブジェクトを有効化
                        suikomi.Play(); //再生
                        smok.SetActive(true); //掃除機の後ろの排気口から煙
                        Smoke.Play();
                        CleanerSound.BGM_Play(); //掃除中はBGMを再生
                        //audi2.Play();
                        count = 2;                                  //countを2にしてcase2に遷移
                    }

                    //プレイヤーが掃除機を地面につけずに移動している時に処理される
                    else if (movetest4.on == true) 
                    {
                        anim.SetBool("isCleaner_taikiwalk", true); //掃除機を持って移動するアニメーション
                    }

                    else
                    {
                        anim.SetBool("isCleaner_taikiwalk", false);
                    }

                    break;

                //掃除機が地面についている状態
                case 2:
                    if (movetest4.on == true)
                    {
                        anim.SetBool("isCleaner_IDO", true); //掃除機を地面につけて移動するアニメーション
                    }

                    if (movetest4.on == false)
                    {
                        anim.SetBool("isCleaner_IDO", false);
                    }

                    //左クリックすると掃除機を地面から離す処理
                    if ((Input.GetMouseButtonDown(0)))
                    {
                        //audi2.Stop();
                        //audi.PlayOneShot(kirikae);
                        anim.SetBool("isCleaner_IDLE", false);
                        so.transform.position = taikiG.position; //掃除機を地面から離した状態の態勢にポジションを変更
                        so.transform.rotation = taikiG.rotation;
                        
                        flag = false; //掃除機が地面から離れた事をSouji_Managerに伝える
                        suiko.SetActive(false);
                        smok.SetActive(false);
                        CleanerSound.BGM_Stop();
                        count = 1; //上に戻る
                    }

                    break;
            }
        }

        //OFF処理
        if ((cleanerswitch.co == 1))                                
        {
            //audi.PlayOneShot(syuusoku);
            parti.SetActive(true);
            Trail.Play();
            suiko.SetActive(false);
            smok.SetActive(false);
            StartCoroutine("simau");
            anim.SetBool("isCleaner_IDO", false);
            anim.SetBool("isCleaner_IDLE", false);
            anim.SetBool("isCleaner_taikiwalk", false);
            anim.SetBool("isCleaner_taiki", false);
            anim.SetBool("isCleaner_ONOFF", false);
            movetest4.idoSpeed = 0.5f;
            flag = false;
            kabenobori1.enabled = true;
            CleanerSound.BGM_Stop();
            count = -1; //countを初期状態に戻す
        }
    }


    //localscaleを0.01秒に一回0.01加算してアイテムを取り出している感を演出
    private IEnumerator toridasu()                                  
    {
        for (i = 0; i < 0.07f; i += 0.01f)
        {
            so.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.5f);
        }
    }

    //上の逆版
    private IEnumerator simau()
    {
        for (i = 0.07f; i >= 0f; i -= 0.01f)
        {           
            so.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.03f);
        }
    }

    //掃除機を取り出す際のエフェクト・プレイヤーのアニメーションを行うコルーチン
    private IEnumerator wait()
    {
        parti.SetActive(true); //パーティクルオブジェクトを有効化 オブジェクトの位置は既にプレイヤーの手元に固定されている
        Trail.Play();  //有効化したパーティクルを再生 くるくるした感じのが再生される
        anim.SetBool("isCleaner_ONOFF", true); //プレイヤーの掃除機を取り出すアニメーション再生
       
        //即座にcount=0に移行すると取り出すアニメーションの再生が終了する前に次のアニメーションが再生されてしまうため少し待つ
        yield return new WaitForSeconds(0.5f);  
        count = 0;
    }

}

