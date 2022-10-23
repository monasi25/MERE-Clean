using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk_Ray : MonoBehaviour
{
    [SerializeField] private GameObject shootpoint;

    [SerializeField] private ShopScript ShopScript;

    [SerializeField] private GameDirector GameDirector;

    [SerializeField] private GameObject CameraStop;
    private float distance = 0.15f;
    private RaycastHit hit;
    public bool end = false;

    private Animator animCon;
    enum Talk_State
    {
        standby,
        Talking,
    }
    Talk_State state;

    // Start is called before the first frame update
    void Start()
    {
        animCon = GetComponent<Animator>();
        state = Talk_State.standby;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(shootpoint.transform.position, shootpoint.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        if (Physics.Raycast(ray, out hit, distance))                                        //Rayがdistanceの範囲で何かに当たったら呼ばれる
        {
            if (hit.collider.tag == "shop")                                                 
            {                
                switch (state)
                {
                    case Talk_State.standby:
                        GameDirector.nabi(1);
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            GameDirector.FunctionState(GameDirector.Fn_State = GameDirector.Function_state.shop);                          
                            ShopScript.enabled = true;
                            ShopScript.talkNum = 0;
                            ShopScript.MouseButtonClicked();
                            animCon.SetBool("iswalk", false);
                            state = Talk_State.Talking;
                            GameDirector.nabi(0);
                            CameraStop.SetActive(false);
                        }
                        break;

                    case Talk_State.Talking:
                        if(end == true)
                        {
                            GameDirector.FunctionState(GameDirector.Fn_State = GameDirector.Function_state.ON);
                            ShopScript.enabled = false;
                            state = Talk_State.standby;
                            end = false;
                            CameraStop.SetActive(true);
                        }
                        break;
                }
            }           
        }
    }
}
