using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*レースの仕様
・ミニレナはゴールに向かって真っ直ぐ走る
・ゴールに向かう道中、いくつかのチェックポイントが設置されており、そのチェックポイントの範囲内に入る事で抽選が行われる
・抽選される内容は加速、転ぶ、何も起こらないの3つ
・各確率はRacePlayerStatusで設定
*/


//レース中の移動、抽選を行うクラス　各ミニレナにアタッチ
public class RacePlayer : MonoBehaviour
{
    private GameObject GoalPos;     //ゴール板
    public Vector3 NowPos;          
    
    private float speed;
    private int Rnd;        //抽選変数

    private Animator anim;

    private GameObject CheckPoint_Parent;           //全てのチェックポイントの親
    private GameObject[] CheckPoint_Child;          //各チェックポイント
    private bool[] Check;       //チェックポイントを二重に通過するのを防ぐための配列bool変数

    private int Safe,Danger,Lucky;          //各抽選確率

    public bool GoalFlag;                   //ゴールしたことを知らせるフラグ
    private bool once,LowStop;
    // Start is called before the first frame update
    void Start()
    {
        CheckPoint_Parent = GameObject.Find("CheckPoint");          //レースシーンからチェックポイントオブジェクトを検索し格納
        CheckPoint_Child = new GameObject[CheckPoint_Parent.transform.childCount];      //サイズ確保
        Check = new bool[CheckPoint_Parent.transform.childCount];           

        for (int i = 0; i < CheckPoint_Parent.transform.childCount; i++)
        {
            CheckPoint_Child[i] = CheckPoint_Parent.transform.GetChild(i).gameObject;       //チェックポイント親オブジェクトの子オブジェクトを格納
        }

        //ミニレナが持つステータスを使用して各値を設定
        speed = this.GetComponent<RacePlayerStatus>().speed;
        Danger = this.GetComponent<RacePlayerStatus>().DangerValue;
        Safe = this.GetComponent<RacePlayerStatus>().SafeValue;

        anim = this.GetComponent<Animator>();

        anim.SetBool("isStart", true);

        GoalPos = GameObject.Find("Goal");      //ゴール版を検索し格納
    }

    // Update is called once per frame
    void Update()
    {
        NowPos = this.transform.position;
        float step = speed * Time.deltaTime;
        
        //抽選処理
        for(int i=0;i < CheckPoint_Parent.transform.childCount; i++)
        {
            if (Check[i] == false) //二重にチェックポイントを通過するのを防ぐ
            {
                //ミニレナの位置がチェックポイントの範囲内に入ると抽選開始
                if ((this.transform.position.z > (CheckPoint_Child[i].transform.position.z) - 0.04) && (this.transform.position.z < (CheckPoint_Child[i].transform.position.z) + 0.04)) 
                {
                    Rnd = Random.Range(1, 101);     //1～100の間でランダムに値を得る

                    if (Rnd <= Safe)
                    {
                        Debug.Log("Safe"); //特になにも起こらない
                    }

                    if ((Rnd > Safe) && (Rnd <= Danger))
                    {
                        StartCoroutine(LowSpeed());     //転ぶ speedは0
                        Debug.Log("Danger");
                    }

                    if (Rnd > Danger)
                    {
                        StartCoroutine(HighSpeed());        //設定されている加速率に応じて加速
                        Debug.Log("Lucky");
                    }

                    Check[i] = true;        //通過したチェックポイントを二回通過しないようにフラグを立てる
                }
            }
        }

        //ゴールに到達すると処理される
        if(this.transform.position.z == GoalPos.transform.position.z && once == false)
        {
            GoalFlag = true;
            once = true;    //一度だけ処理するためのフラグを立てる
        }

        //ゴールに向かって直線的に進む
        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.transform.position.x,this.transform.position.y,GoalPos.transform.position.z), step);
    }

    //転んだ時の処理
    private IEnumerator LowSpeed()
    {
        //加速中は転ばない
        if (LowStop == false)
        {
            speed = 0f;
            anim.SetBool("isDanger", true);         //転ぶアニメーション
            yield return new WaitForSeconds(1.5f);  //一定時間転ぶ
            anim.SetBool("isDanger", false);    
            speed = this.GetComponent<RacePlayerStatus>().speed;        //speedを元に戻す
        }
    }
    private IEnumerator HighSpeed()
    {
        LowStop = true;     //加速中に転ばないようにする
        speed = this.GetComponent<RacePlayerStatus>().speed + this.GetComponent<RacePlayerStatus>().Kasoku;     //事前に設定した加速率を加算
        anim.SetBool("isRun",true);         //加速アニメーション
        yield return new WaitForSeconds(1.3f);      //一定時間加速モード
        anim.SetBool("isRun", false);
        speed = this.GetComponent<RacePlayerStatus>().speed;
        LowStop = false;
    }
}
