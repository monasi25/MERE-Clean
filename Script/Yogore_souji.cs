using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ヨゴレの掃除処理を行うクラス　各ヨゴレオブジェクトにアタッチされている
public class Yogore_souji : MonoBehaviour
{
    private GameObject Yogore;

    [SerializeField] private Souji_Manager Souji_Manager;
    [SerializeField] private GameDirector director;
    [SerializeField] private GameObject YogoreEfect;

    private int myNumber;
    private float i;
    private bool once = false;
    // Start is called before the first frame update
    void Start()
    {
        Yogore = this.gameObject;

        //Souji_ManagerのYogore_Child配列に格納されているオブジェクトと自分のオブジェクトをリンクさせる
        for (int i = 0; i < Souji_Manager.Yogore_Parent.transform.childCount; i++)
        {
            if (Yogore == Souji_Manager.Yogore_Child[i])
            {
                myNumber = i;   //自分の番号を保持しておく
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //自分の番号のフラグが立つとヨゴレを掃除する処理を始める
        //もしノズルが掃除中にヨゴレから外れても処理を途中で止めない
        if ((Souji_Manager.Yogore_Detection[myNumber] == true) && (once == false))
        {
            StartCoroutine(syusoku());
            once = true;
        }
       
        //ヨゴレのScaleが0.3以下になると掃除完了
        if (Yogore.transform.localScale.x <= 0.3)
        {
            director.Yogore();  //綺麗度加算
            Instantiate(YogoreEfect, Yogore.transform.position, YogoreEfect.transform.rotation);    //掃除完了エフェクト
            Yogore.SetActive(false);    //掃除完了したヨゴレを無効化
            Souji_Manager.Yogore_DestoryNum[myNumber] = true;   //削除フラグを立ててシーン遷移を行っても消えておくようにする
        }
        
    }

    //0.05秒に一回Scaleを小さくして掃除アニメーションを行う
    private IEnumerator syusoku()
    {
        for (i = 1f; i >= 0f; i -= 0.01f)
        {
            Yogore.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
