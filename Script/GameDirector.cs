using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    private double Percent;

    [SerializeField]
    private GameObject moneytext;
    double yogore = 0;
    [SerializeField]
    private GameObject kireidotext;

    [SerializeField] private GameObject nabitext;

    double hokori = 0;
    public int money = 0;
    private AudioSource aud;

    [SerializeField] private AudioClip CleanPerfect,allPerfect;

    [SerializeField] private Text PerfectText;
    public cleaner_Move cleaner_Move;

    [SerializeField] movetest4 movetest4;
    [SerializeField] CleanerSwitch CleanerSwitch;
    [SerializeField] kabenobori1 kabenobori1;
    [SerializeField] Talk_Ray Talk_Ray;

    [SerializeField] ShopManager ShopManager;

    private double moneyplus = 18.75;
    public enum Function_state
    {
        all,
        sojiki,
        kabe,
        shop,
        ON,
        none,
    }

    public Function_state Fn_State;

    public void GetCoin()
    {
        money += 100;
    }

    public void Cleaner()
    {
        aud.PlayOneShot(CleanPerfect);
        hokori += 5.5;
    }

    public void Yogore()
    {
        aud.PlayOneShot(CleanPerfect);
        yogore += 5.5;
    }
    
    public void FunctionState(Function_state off_State)
    {
        switch (off_State)
        {
            case Function_state.ON:                
                movetest4.enabled = true;
                kabenobori1.enabled = true;
                CleanerSwitch.enabled = true;
                Talk_Ray.enabled = true;
                break;

            case Function_state.kabe:
                movetest4.enabled = false;
                Talk_Ray.enabled = false;
                CleanerSwitch.enabled = false;  
                break;

            case Function_state.sojiki:
                Talk_Ray.enabled = false;
                kabenobori1.enabled = false;
                break;

            case Function_state.shop:
                movetest4.enabled = false;
                kabenobori1.enabled = false;
                CleanerSwitch.enabled = false;
                break;

            case Function_state.all:
                movetest4.enabled = false;
                kabenobori1.enabled = false;
                CleanerSwitch.enabled = false;
                Talk_Ray.enabled = false;
                break;

            case Function_state.none:
                break;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        Fn_State = Function_state.none;
        money = 3000;
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ShopManager.wallUPState == ShopManager.WallUPState.buy)
        {
            Debug.Log("ofof");
            kabenobori1.enabled = false;
        }

        if(moneyplus <= Percent)
        {
            money += 1000;
            moneyplus += 18.75;
        }

        Percent = hokori + yogore;
        
        if(Percent == 99)
        {
            hokori += 1;
            PerfectSyori();
        }

        kireidotext.GetComponent<Text>().text = "お部屋の綺麗度：" + Percent.ToString() + "%";
        moneytext.GetComponent<Text>().text = "所持金：" + money.ToString() + "円";
        nabi(0);

        Cursor.lockState = CursorLockMode.Confined;
    }

    void PerfectSyori()
    {
        PerfectText.text = "綺麗度１００％達成おめでとう～";
        aud.PlayOneShot(allPerfect);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        for (float p = 1f; p >= 0; p -= 0.01f)
        {          
            PerfectText.color = new Color(1f, 1f, 1f, p);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void nabi(int i)
    {
        switch (i)
        {
            case 0:
                nabitext.GetComponent<Text>().text = "";
                break;

            case 1:
                nabitext.GetComponent<Text>().text = "Eを押して話す";
                break;

            case 2:
                nabitext.GetComponent<Text>().text = "Eを押して登る";
                break;

            case 3:
                nabitext.GetComponent<Text>().text = "Spaceで手を離す";
                break;
        }
    }

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Application.targetFrameRate = 60;
    }
}
