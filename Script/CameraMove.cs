using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//全体カメラにアタッチされている移動クラス
public class CameraMove : MonoBehaviour
{
    [SerializeField] private GameObject Goal;       //ゴール板
    [SerializeField] private RaceManager RaceManager;
    private float speed;
   
    void Update()
    {
        speed = (((RaceManager.AllSpeed)/4) - 0.06f) * Time.deltaTime;      //全てのミニレナの速度の平均値で移動
        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y, Goal.transform.position.z), speed);
    }
}
