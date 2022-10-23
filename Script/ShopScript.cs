using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopScript : MonoBehaviour
{
    [SerializeField] private FootSound foot;

    [SerializeField] private string[] talks;
    private string[] words;
    [SerializeField] private Text text;

    [SerializeField] private Talk_Ray Talk_Ray;
    private bool finish = false;
    private bool flag = false;
    private bool brink = false;
    public int talkNum = 0;

    [SerializeField] private GameObject MouseImage;
    [SerializeField] private UIManager UIManager;

   
    void Update()
    {
        if ((Input.GetMouseButtonDown(0)))
        {
            if(flag == false)
            {
                text.text = "";
                UIManager.ShopAppear();
                MouseImage.SetActive(false);
                flag = true;
                brink = false;
            }

            if(finish == true)
            {
                text.text = "";
                Talk_Ray.end = true;
                flag = false;
                finish = false;
                brink = false;
                MouseImage.SetActive(false);
            }
        }

        if(brink == true)
        {
            MouseImage.SetActive(true);
        }
    }

    public void MouseButtonClicked() 
    {
        text.text = "";
        StartCoroutine(Dialogue());
        flag = true;
    }


    IEnumerator Dialogue()
    {
        // 全角スペースで文字を分割する。
        words = talks[talkNum].Split('　');

        foreach (var word in words)
        {            
            text.text = text.text + word;          // 0.1秒刻みで１文字ずつ表示する。            
            foot.FootStep();
            yield return new WaitForSeconds(0.1f);
        }

        // 次のセリフがある場合には、トークボタンを表示する。
        if (talkNum + 1 < talks.Length)
        {
            flag = false;            
        }

        else               //会話終了判定 
        {
            Debug.Log("haitta");
            finish = true;
        }

        brink = true;
        // 次のセリフをセットする。
        talkNum = talkNum + 1;
    }
}
