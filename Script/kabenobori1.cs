using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//壁を登るための一連の処理が記述されています
public class kabenobori1 : MonoBehaviour
{
    
    private float distance = 0.04f;   //0.04
    private RaycastHit hit;
    public GameObject shootpoint;
    private CharacterController Ccon;
    public float speed = 0.2f;      //壁を登る速度
    private Animator anim;
    Vector3 movedirection;  //登る方向
    private float Angle;    //壁との角度調整用変数
    public bool down = false;
    [SerializeField] private GameDirector GameDirector;

    //壁登りの現在の状態を表すenum
    enum WallUp_State
    {
        wait,
        standby,
        move,
    }
    WallUp_State state;

    void Start()
    {
        Ccon = GetComponent<CharacterController>();        
        anim = GetComponent<Animator>();
        state = WallUp_State.wait;
    }
   
    void Update()
    {
        Ray ray = new Ray(shootpoint.transform.position, shootpoint.transform.forward);     //頭の部分からRayをforward方向に飛ばす
        Debug.DrawRay(ray.origin, ray.direction, Color.red);                                //Rayの表示

        if (Physics.Raycast(ray, out hit, distance))                                        //Rayがdistanceの範囲で何かに当たったら呼ばれる
        {
            //タグに応じてAngleを変更して壁とプレイヤーを向かい合わせにする
            if (hit.collider.tag == "upok")                                                 //登れる障害物はupokのタグをつける
            {
                Angle = 90f;
                WallUp(Angle);
            }
                
            if(hit.collider.tag == "upok2")
            {
                Angle = 270f;
                WallUp(Angle);
            }

            if ((hit.collider.tag == "climb"))                                              //頂上まで登った時の壁を登りきる処理
            {                
                state = WallUp_State.wait;
                Ccon.enabled = false;
                anim.applyRootMotion = true;                                                //applyRootMotionをtrueにしアニメーションの動きに合わせてオブジェクトを移動させる（普段はオフ）
                anim.SetBool("climb", true);                                                //壁よじ登りアニメーション
                Invoke("Climbs", 3.5f);                                                     //アニメーションが再生終了するまで待機してから壁登り解除処理を行う
            }

        }

        else if(state == WallUp_State.move)                                                 //壁の端に寄りすぎた時は壁登りを解除
        {
            WallUp_Kaijo();           
        }
    }

    void WallUp(float angle)
    {
        switch (state)
        {
            //登れる壁の前に立っている状態
            case WallUp_State.wait:
                GameDirector.nabi(2);
                if (Input.GetKeyDown(KeyCode.E))                                    //誤作動防止のため他の移動系スクリプト全てをfalseにする
                {
                    GameDirector.FunctionState(GameDirector.Fn_State = GameDirector.Function_state.kabe);                   
                    state = WallUp_State.standby;                    
                }
                break;

            //Fキーを押して最初に行われる処理
            case WallUp_State.standby:                                              //壁に飛びつく・体の向きを変更する等の準備を行う
                anim.SetBool("iswall", true);
                transform.rotation = hit.collider.gameObject.transform.rotation;    //ここで壁のrotationと同じにする
                transform.rotation = Quaternion.Euler(0f, angle, 0f);               //上の処理だと真逆に向くためここで壁と体が向かい合うように調整                                                            
                anim.SetBool("iswalldefo", true);
                state = WallUp_State.move;
                break;

            //壁にしがみついている間に行われる処理
            case WallUp_State.move:                                                 //壁を移動する処理を行う
                GameDirector.nabi(3);
                if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)))
                {
                    //各アニメーション
                    anim.SetBool("wallup", false);
                    anim.SetBool("walldown", false);
                    anim.SetBool("wallside", true);
                    movedirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0); //横方向の移動
                }

                else if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S)))    //elseにしているのはななめ移動を禁止するため
                {
                    if (Input.GetKey(KeyCode.W))
                    {
                        anim.SetBool("wallside", false);
                        anim.SetBool("walldown", false);
                        anim.SetBool("wallup", true);
                        down = false;
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        anim.SetBool("wallside", false);
                        anim.SetBool("wallup", false);
                        anim.SetBool("walldown", true);
                        down = true;
                    }
                    movedirection = new Vector3(0, Input.GetAxis("Vertical"), 0);   //縦方向の移動
                }

                else                                                                //入力がされていない時のデフォルト状態
                {
                    anim.SetBool("wallside", false);
                    anim.SetBool("wallup", false);
                    anim.SetBool("walldown", false);
                }

                if ((Input.GetKey(KeyCode.Space)))                                  //壁登り解除
                {
                    WallUp_Kaijo();
                    GameDirector.nabi(0);
                }

                Move();                                                             //移動を行う関数                     

                break;
        }
    }

    void Move()
    {
        movedirection = transform.TransformDirection(movedirection);    //ローカルからワールド座標に変換
        movedirection *= speed;
        Ccon.Move(movedirection * Time.deltaTime);
    }

    //壁を登りきった時に呼ばれる関数
    void Climbs()
    {
        anim.applyRootMotion = false;
        Ccon.enabled = true;
        anim.SetBool("iswalldefo", false);
        anim.SetBool("iswall", false);
        anim.SetBool("wallup", false);
        anim.SetBool("walldown", false);
        anim.SetBool("wallside", false);
        anim.SetBool("climb", false);
        GameDirector.FunctionState(GameDirector.Fn_State = GameDirector.Function_state.ON);
    }

    //壁登りを解除する時に呼ぶ関数
    void WallUp_Kaijo()
    {
        anim.SetBool("iswalldefo", false);
        anim.SetBool("iswall", false);
        anim.SetBool("wallup", false);
        anim.SetBool("walldown", false);
        anim.SetBool("wallside", false);
        GameDirector.FunctionState(GameDirector.Fn_State = GameDirector.Function_state.ON); //プレイヤーの状態を通常状態に
        state = WallUp_State.wait;
    }

}
  



