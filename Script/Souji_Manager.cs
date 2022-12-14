using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//掃除中にどのホコリ・ヨゴレに触れたかを判定するクラス

public class Souji_Manager : MonoBehaviour
{
    public bool[] Hokori_Detection;     //ホコリにノズルが触れた時のフラグ用bool変数
    public bool[] Yogore_Detection;     //ヨゴレにノズルが触れた時のフラグ用bool変数
    public bool[] Hokori_DestroyNum;    //既に掃除完了したホコリの番号のフラグを立てる
    public bool[] Yogore_DestoryNum;    //同様

    public bool Ground_Detection;

    private float Nozzle_distance = 1f;

    public GameObject Hokori_Parent;    //全てのホコリオブジェクトをまとめる親オブジェクト
    public GameObject[] Hokori_Child;   //各ホコリオブジェクト
    public GameObject Yogore_Parent;    //同様
    public GameObject[] Yogore_Child;   //同様

    [SerializeField] private GameObject NozzleObj;

    [SerializeField] private cleaner_Move cleaner;

    public Vector2 pixelUV;

    private void Awake()
    {
        Hokori_Child = new GameObject[Hokori_Parent.transform.childCount];  //配列のサイズを子オブジェクトの数だけ確保
        Hokori_Detection = new bool[Hokori_Parent.transform.childCount];    
        Hokori_DestroyNum = new bool[Hokori_Parent.transform.childCount];
        
        Yogore_Child = new GameObject[Yogore_Parent.transform.childCount];  
        Yogore_Detection = new bool[Yogore_Parent.transform.childCount];
        Yogore_DestoryNum = new bool[Yogore_Parent.transform.childCount];

        //メインシーンが読み込まれた初回に一度だけValueSaveクラスのヨゴレとホコリの状態を保存する配列のサイズを確保
        if(ValueSave.Once == false)
        {
            ValueSave.yogore_State_save = new bool[Yogore_Parent.transform.childCount];
            ValueSave.hokori_State_save = new bool[Hokori_Parent.transform.childCount];
            ValueSave.Once = true;
        }

        //配列に各ホコリオブジェクトを代入
        for(int i = 0; i < Hokori_Parent.transform.childCount; i++)
        {
            Hokori_Child[i] = Hokori_Parent.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < Yogore_Parent.transform.childCount; i++)
        {
            Yogore_Child[i] = Yogore_Parent.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var ray = new Ray(NozzleObj.transform.position, new Vector3(0, -1, 0));
        RaycastHit hit;

        if (cleaner.flag == true) //掃除機のスイッチがONなら
        {
            if (Physics.Raycast(ray, out hit, Nozzle_distance)) //掃除機のノズルがオブジェクトに触れているかどうか
            {
                //ホコリ検知時の処理
                if (hit.collider.tag == "dust") //掃除機のノズルがホコリに触れているかどうか
                {
                    Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
                    MeshCollider meshCollider = hit.collider as MeshCollider;

                    if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || meshCollider == null)
                    {
                        Debug.Log("NULL");
                        return;
                    }
                    
                    for(int i = 0; i < Hokori_Parent.transform.childCount; i++)
                    {
                        //当たったホコリが配列の何番目のホコリかチェック
                        if (hit.collider.gameObject == Hokori_Child[i])
                        {
                            Hokori_Detection[i] = true; //ホコリ検知フラグを立てる
                        }

                        else
                        {
                            Hokori_Detection[i] = false;
                        }
                    }

                    pixelUV = hit.textureCoord; //当たったホコリの場所のUV座標取得
                }

                //ヨゴレ検知時の処理
                if(hit.collider.tag == "dirt")
                {
                    for (int i = 0; i < Yogore_Parent.transform.childCount; i++) //当たったヨゴレが配列の何番目のヨゴレかチェック
                    {
                        if (hit.collider.gameObject == Yogore_Child[i])
                        {
                            Yogore_Detection[i] = true; //ヨゴレ検知フラグを立てる
                        }

                        else
                        {
                            Yogore_Detection[i] = false;
                        }
                    }
                }
            }
        }
    }

    //他のシーンからメインシーンに遷移した時、既に掃除完了しているホコリ・ヨゴレを無効化する関数 GameDirectorクラスから呼び出す
    public void DestroyFn()
    {
        for (int i = 0; i < Hokori_Parent.transform.childCount; i++)
        {
            if (Hokori_DestroyNum[i] == true)
            {
                Hokori_Child[i].SetActive(false);
            }
        }

        for (int i = 0; i < Yogore_Parent.transform.childCount; i++)
        {
            if (Yogore_DestoryNum[i] == true)
            {
                Yogore_Child[i].SetActive(false);
            }
        }
    } 
}

