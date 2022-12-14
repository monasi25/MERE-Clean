using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UIの表示、非表示を行うクラス
public class UIManager : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] AudioClip clip1;   //クリック効果音

    [SerializeField] private GameObject ShopUI,Option;      //ショップ画面とオプション画面
    [SerializeField] private ShopScript ShopScript;

    [SerializeField] private GameObject Tutorial1,Turorial2;    //チュートリアル画面1と2

    [SerializeField] private GameDirector GameDirector;

    [SerializeField] private Button OptionButton,tutorialButton;    //オプション画面表示ボタンとチュートリアル画面表示ボタン

    [SerializeField] private movetest4 movetest4;

    [SerializeField] private GameObject CameraStoped;       //カメラ

    [SerializeField] private CleanerSwitch CleanerSwitch;

    //UIが表示もしくは非表示かを判断するenum
    enum UIState
    {
        appear,
        none,
    }
    UIState State = UIState.none;
    void Start()
    {
        source = this.GetComponent<AudioSource>();    
    }

    void Update()
    {
        //メインゲームシーンに常に露出しているボタンは何らかのUIが表示している間は無効化する
        if(State == UIState.appear)
        {
            OptionButton.interactable = false;
            tutorialButton.interactable = false;
        }

        if(State == UIState.none)
        {
            OptionButton.interactable = true;
            tutorialButton.interactable = true;
        }
    }

    //ショップ画面表示
    public  void ShopAppear()
    {
        ShopUI.SetActive(true);
        State = UIState.appear;
    }

    //ショップ画面非表示
    public void ShopExit()
    {
        source.PlayOneShot(clip1);
        ShopUI.SetActive(false);
        ShopScript.MouseButtonClicked();
        State = UIState.none;
    }

    //オプション画面表示
    public void OptionAppear()
    {
        source.PlayOneShot(clip1);
        Option.SetActive(true);
        movetest4.WalkFalse();
        GameDirector.FunctionState(GameDirector.Function_state.all);
        State = UIState.appear;
        CameraStoped.SetActive(false);
        if (CleanerSwitch.Cleaners == true)
        {
            CleanerSwitch.FunctonOff();
        }
    }

    //オプション画面非表示
    public void OptionExit()
    {
        source.PlayOneShot(clip1);
        Option.SetActive(false);
        GameDirector.FunctionState(GameDirector.Function_state.ON);
        State = UIState.none;
        CameraStoped.SetActive(true);
    } 

    //チュートリアル画面表示
    public void TutorialAppear()
    {
        source.PlayOneShot(clip1);
        Tutorial1.SetActive(true);
        movetest4.WalkFalse();
        GameDirector.FunctionState(GameDirector.Function_state.all);        //プレイヤーの全ての機能を停止
        State = UIState.appear;
        CameraStoped.SetActive(false);
        if (CleanerSwitch.Cleaners == true) //掃除中にチュートリアル画面を表示した場合、掃除機をしまう
        {
            CleanerSwitch.FunctonOff();
        }
    }

    //チュートリアル画面のページ切り替え
    public void sinkou()
    {
        source.PlayOneShot(clip1);
        Tutorial1.SetActive(false);
        Turorial2.SetActive(true);
    }

    //チュートリアル画面非表示
    public void TutorialExit()
    {
        source.PlayOneShot(clip1);
        Turorial2.SetActive(false);
        GameDirector.FunctionState(GameDirector.Function_state.ON);
        State = UIState.none;
        CameraStoped.SetActive(true);
    }

    //タイトルに戻るボタンを押した際に呼ばれる関数
    public void quit()
    {
        FadeManager.Instance.LoadScene("Title", 1.0f);
    }
}
