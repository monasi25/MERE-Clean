using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//値を保存するためのクラス
public class ValueSave : MonoBehaviour
{
    public static Vector3 PlayerPos_save;       //プレイヤーのポジションを格納
    public static int money_save;               //所持金
    public static double yogore_save;           //ヨゴレをいくつ掃除完了したか
    public static double hokori_save;           //ホコリをいくつ掃除完了したか
    public static bool[] yogore_State_save;     //どのヨゴレを掃除完了したか保持する Souji_Managerでサイズを設定 ①
    public static bool[] hokori_State_save;     //どのホコリを掃除完了したか保持する Souji_Managerでサイズを設定 ②
    public static bool WallUp;                  //ショップで壁登りチップを購入したか
    public static bool PowerUp;                 //ショップで掃除パワーアップチップを購入したか

    public static bool Keep;                    //値がセーブされるとtrue
    public static bool Once;                    //①と②のサイズを決めるためにゲームで一度だけ処理を行うための変数
}
