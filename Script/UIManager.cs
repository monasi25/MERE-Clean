using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] AudioClip clip1;

    [SerializeField] private GameObject ShopUI,Option;
    [SerializeField] private ShopScript ShopScript;

    [SerializeField] private GameObject Tutorial1,Turorial2;

    [SerializeField] private GameDirector GameDirector;

    [SerializeField] private Button OptionButton,tutorialButton;

    [SerializeField] private movetest4 movetest4;

    [SerializeField] private GameObject CameraStoped;

    [SerializeField] private CleanerSwitch CleanerSwitch;
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
    public  void ShopAppear()
    {
        ShopUI.SetActive(true);
        State = UIState.appear;
    }

    public void ShopExit()
    {
        source.PlayOneShot(clip1);
        ShopUI.SetActive(false);
        ShopScript.MouseButtonClicked();
        State = UIState.none;
    }

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

    public void OptionExit()
    {
        source.PlayOneShot(clip1);
        Option.SetActive(false);
        GameDirector.FunctionState(GameDirector.Function_state.ON);
        State = UIState.none;
        CameraStoped.SetActive(true);
    } 

    public void TutorialAppear()
    {
        source.PlayOneShot(clip1);
        Tutorial1.SetActive(true);
        movetest4.WalkFalse();
        GameDirector.FunctionState(GameDirector.Function_state.all);
        State = UIState.appear;
        CameraStoped.SetActive(false);
        if (CleanerSwitch.Cleaners == true)
        {
            CleanerSwitch.FunctonOff();
        }
    }

    public void sinkou()
    {
        source.PlayOneShot(clip1);
        Tutorial1.SetActive(false);
        Turorial2.SetActive(true);
    }

    public void TutorialExit()
    {
        source.PlayOneShot(clip1);
        Turorial2.SetActive(false);
        GameDirector.FunctionState(GameDirector.Function_state.ON);
        State = UIState.none;
        CameraStoped.SetActive(true);
    }

    public void quit()
    {
        FadeManager.Instance.LoadScene("Title", 1.0f);
    }
}
