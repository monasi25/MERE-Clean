using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ミニレナのステータスを管理するクラス　各ミニレナに一つずつアタッチ　値はインスペクターで設定
public class RacePlayerStatus : MonoBehaviour
{
    public int money;           //いくらBETされたかを保持する
    public float speed;         //レースで走る速度
    public int DangerValue;     //転ぶ確率
    public int SafeValue;       //安全に走れる確率
    public float Kasoku;        //加速率
    public string Name;         //ミニレナの名前
    public float Odds;          //オッズ
    public int rank;            //レースでの順位
    public bool Betflag;        //一度でもBETされたか
}
