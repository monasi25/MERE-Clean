using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerSwitch : MonoBehaviour
{
    [SerializeField] private GameDirector GameDirector;
    [SerializeField] private CleanerSound CleanerSound;
    [SerializeField] private movetest4 movetest4;
    public int co = 0;
    public bool Cleaners = false;
    private bool buttonEnabled = true;

    // Update is called once per frame
    void Update()
    {
        if (buttonEnabled == true)
        {
            if ((Input.GetKeyDown(KeyCode.F)) && (Cleaners == true))
            {
                Debug.Log("Switch_OFF");
                movetest4.kaitenSpeed = 800f;
                FunctonOff();
            }

            else if ((Input.GetKeyDown(KeyCode.F)) && (Cleaners == false))
            {
                Debug.Log("Switch_ON");
                co = 2;
                GameDirector.FunctionState(GameDirector.Fn_State = GameDirector.Function_state.sojiki);
                Cleaners = true;
                buttonEnabled = false;
                CleanerSound.Toridasu_Sound();
                StartCoroutine(EnableButton());
                movetest4.kaitenSpeed = 300f;
            }
        }

        else
        {
            co = 0;
        }
    }

    private IEnumerator EnableButton()
    {
        // 3秒後に解除         
        yield return new WaitForSeconds(3f);
        buttonEnabled = true;
    }

    public void FunctonOff()
    {
        co = 1;
        GameDirector.FunctionState(GameDirector.Fn_State = GameDirector.Function_state.ON);
        Cleaners = false;
        buttonEnabled = false;
        CleanerSound.Simau_Sound();
        StartCoroutine(EnableButton());
    }

}
