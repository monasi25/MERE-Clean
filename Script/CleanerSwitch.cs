using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//このスクリプトでは掃除機の状態に応じてプレイヤーの状態を変更します.

public class CleanerSwitch : MonoBehaviour
{
    [SerializeField] private GameDirector GameDirector;
    [SerializeField] private CleanerSound CleanerSound;
    [SerializeField] private movetest4 movetest4;
    public int co = 0;
    public bool Cleaners = false;  //Fキーが押された事を他のスクリプトに知らせるための変数
    
    private bool buttonEnabled = true; //取り出し・収納エフェクト再生中に連打されるのを防ぐための変数 trueで使用可能状態

    // Update is called once per frame
    void Update()
    {
        //Fキーを押せる状態
        if (buttonEnabled == true)
        {
            //掃除機収納処理
            if ((Input.GetKeyDown(KeyCode.F)) && (Cleaners == true))
            {
                Debug.Log("Switch_OFF");
                movetest4.kaitenSpeed = 800f; //プレイヤーの回転速度を通常に戻す
                FunctonOff(); 
            }

            //掃除機取り出し処理
            else if ((Input.GetKeyDown(KeyCode.F)) && (Cleaners == false))
            {
                Debug.Log("Switch_ON");
                co = 2;
                GameDirector.FunctionState(GameDirector.Fn_State = GameDirector.Function_state.sojiki); //ゲームディレクターに掃除機状態を伝える
                Cleaners = true; //cleaner_Moveが動作する
                buttonEnabled = false; //Fキーを押せない状態にする
                CleanerSound.Toridasu_Sound();  //効果音再生
                StartCoroutine(EnableButton()); //コルーチンを使用してFキーを押せるまでのカウントスタート
                movetest4.kaitenSpeed = 300f; //掃除機使用中にプレイヤーがカクカクした動きをしないように回転速度をゆっくりに
            }
        }

        else
        {
            co = 0;
        }
    }

    private IEnumerator EnableButton() //掃除機の取り出しボタンの連打防止
    {
        // 3秒後に解除         
        yield return new WaitForSeconds(3f);
        buttonEnabled = true;
    }

    //掃除機を収納する際のもろもろの処理を行う関数
    public void FunctonOff()
    {
        co = 1;
        GameDirector.FunctionState(GameDirector.Fn_State = GameDirector.Function_state.ON); //ゲームディレクターに通常状態と伝える
        Cleaners = false; //掃除機のスイッチを切る
        buttonEnabled = false; 
        CleanerSound.Simau_Sound();
        StartCoroutine(EnableButton());
    }

}
