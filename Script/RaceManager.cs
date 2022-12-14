using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//レースのカウントダウン、着順記録等を行うクラス
public class RaceManager : MonoBehaviour
{
    Stack<GameObject> GoalNum = new Stack<GameObject>();    //着順記録
    
    public GameObject[] Gate_Child;         //各ミニレナ
    private GameObject[] GoalNumber;        //着順にミニレナを格納するようの配列

    [SerializeField] private Transform[] GatePos;       //各スタート位置
    private string[] PlayerName;            //3Dテキストに表示する用の名前が入る

    private bool start = false;             //カウントダウン終了判定フラグ

    [SerializeField] private GameObject[] GateCam;      //個別カメラが入る
    [SerializeField] private GameObject AllCam;         //全体カメラ
    [SerializeField] private GameObject[] Names;        //3Dテキスト
    [SerializeField] private Text Bettext;              //Betしたミニレナを表示するテキスト
    [SerializeField] private GameObject[] CountDownObj;     //カウントダウンUI
    private bool once;
    private int NowCamNum = 0;      //カメラ切り替え用変数
    public float AllSpeed;          //全員のスピードを足したスピード
    private bool CameraSwitch = false;  //全体カメラとの切り替え変数　trueで全体カメラになる

    private AudioSource audRace;
    [SerializeField] private AudioClip BGM, HUE, COUNT, PII;
    // Start is called before the first frame update
    void Start()
    {
        PlayerName = new string[4];
        GoalNumber = new GameObject[4];     
        audRace = this.GetComponent<AudioSource>();

        for(int i = 0; i < 4; i++)
        {
            PlayerName[i] = SelectManager.ObjName[i];
            Gate_Child[i] = GameObject.Find(PlayerName[i]);     //PlayerNameを元にミニレナを検索し格納
            Gate_Child[i].transform.position = GatePos[i].position;         //ミニレナをBETシーンの位置からスタート位置に移動させる
            Gate_Child[i].transform.rotation = GatePos[i].rotation;
            GateCam[i].transform.parent = Gate_Child[i].transform;          //各ミニレナの専属カメラが追従するように子オブジェクトに設定
            Names[i].transform.parent = Gate_Child[i].transform;            //3Dテキストがミニレナに追従するように設定
            Names[i].GetComponent<TextMesh>().text = Gate_Child[i].GetComponent<RacePlayerStatus>().Name;       //3Dテキストのテキストを各ミニレナの名前に
            AllSpeed += Gate_Child[i].GetComponent<RacePlayerStatus>().speed;           //全てのミニレナのスピードを足す　※全体カメラの移動速度決定に使用

            //BETしたミニレナをテキスト表示する
            if(Gate_Child[i].GetComponent<RacePlayerStatus>().money > 0)
            {
                Bettext.text += Gate_Child[i].GetComponent<RacePlayerStatus>().Name + "\n";
            }
        }
        
        StartCoroutine(CountDown());        //レースカウントダウンスタート
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(CameraSwitch);

        //カウントダウンが終了するとミニレナと全体カメラの移動を開始
        //一度だけ処理される
        if (start == true)
        {
            for (int i = 0; i < 4; i++)
            {
                Gate_Child[i].GetComponent<RacePlayer>().enabled = true;    //各ミニレナの移動・抽選クラスを有効化
            }

            AllCam.GetComponent<CameraMove>().enabled = true;   //全体カメラの移動クラスを有効化

            start = false;  
        }

        if (CameraSwitch == false)
        {
            //Dで右のミニレナにカメラを切り替え
            if (Input.GetKeyDown(KeyCode.D))
            {
                //一番端まで来たら最左のミニレナのカメラに切り替える
                if (NowCamNum == 3)
                {
                    GateCam[0].GetComponent<Camera>().enabled = true;           //次のカメラを有効化
                    GateCam[NowCamNum].GetComponent<Camera>().enabled = false;  //現在のカメラを無効化
                    NowCamNum = 0;
                }

                //順番にカメラを切り替える
                else
                {
                    GateCam[NowCamNum + 1].GetComponent<Camera>().enabled = true;   //次のカメラを有効化
                    GateCam[NowCamNum].GetComponent<Camera>().enabled = false;      //現在のカメラを無効化
                    NowCamNum++;
                }
            }

            //Aで左のミニレナにカメラを切り替える
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (NowCamNum == 0)
                {
                    GateCam[3].GetComponent<Camera>().enabled = true;
                    GateCam[NowCamNum].GetComponent<Camera>().enabled = false;
                    NowCamNum = 3;
                }

                else
                {
                    GateCam[NowCamNum - 1].GetComponent<Camera>().enabled = true;
                    GateCam[NowCamNum].GetComponent<Camera>().enabled = false;
                    NowCamNum--;
                }
            }
        }

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //既に全体カメラなら個別カメラに切り替え
            if (CameraSwitch == true)
            {
                GateCam[NowCamNum].GetComponent<Camera>().enabled = true;
                AllCam.GetComponent<Camera>().enabled = false;
                CameraSwitch = false;
            }

            //全体カメラに切り替える
            else
            {
                AllCam.GetComponent<Camera>().enabled = true;
                GateCam[NowCamNum].GetComponent<Camera>().enabled = false;
                CameraSwitch = true;
            }
        }

        //ゴール確認処理
        for(int i=0;i<4; i++)
        {
            if(Gate_Child[i].GetComponent<RacePlayer>().GoalFlag == true)
            {
                GoalNum.Push(Gate_Child[i]);    //ゴールした順番にスタックに詰め込む
                Gate_Child[i].GetComponent<RacePlayer>().GoalFlag = false;
            }
        }
        
        //全員がゴールすると処理される
        if(GoalNum.Count == 4 && once == false)
        {           
            GoalNumber = GoalNum.ToArray(); //スタックの中身を配列に移す
            audRace.PlayOneShot(HUE);
            for (int i = 0; i < 4; i++)
            {
                Debug.Log(i + GoalNumber[i].name); //配列番号3が1位、0が4位
                GoalNumber[i].GetComponent<RacePlayerStatus>().rank = i;    //ミニレナに順位を設定
                GoalNumber[i].GetComponent<RacePlayer>().enabled = false;   //移動クラスを無効化
                Names[i].transform.rotation = Quaternion.Euler(0f,0f,0f);   //次のシーンで正面からミニレナを見るため3Dテキストを反転
                DontDestroyOnLoad(GoalNumber[i]);   //ミニレナをそのまま保持
            }

            once = true;
            FadeManager.Instance.LoadScene("Commendation", 2.0f);   //結果確認シーンに遷移
        }
    }

    //カウントダウンとカウントダウンアニメーションを行うコルーチン
    private IEnumerator CountDown()
    {
        CountDownObj[0].SetActive(true);
        audRace.PlayOneShot(COUNT);
        yield return new WaitForSeconds(1f);
        CountDownObj[0].SetActive(false);
        CountDownObj[1].SetActive(true);
        audRace.PlayOneShot(COUNT);
        yield return new WaitForSeconds(1f);
        CountDownObj[1].SetActive(false);
        CountDownObj[2].SetActive(true);
        audRace.PlayOneShot(COUNT);
        yield return new WaitForSeconds(1f);
        CountDownObj[2].SetActive(false);
        CountDownObj[3].SetActive(true);
        audRace.PlayOneShot(PII);
        yield return new WaitForSeconds(1f);
        CountDownObj[3].SetActive(false);
        audRace.PlayOneShot(BGM);
        start = true;
    }

    //レースゲームのように現在の順位は表示しない
}
