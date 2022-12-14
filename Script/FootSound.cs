using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//このスクリプトではプレイヤーの走るタイミングに合わせて足音を鳴らす処理を記述しています.

public class FootSound : MonoBehaviour
{
    private AudioSource AudioSource;

    [SerializeField] private kabenobori1 kabenobori1;

    private int i = 0,j=7;

    [SerializeField] private AudioClip[] Foot;  //ドレミファソラシドの音が一つずつ入っている
    
    void Start()
    {
        AudioSource = this.GetComponent<AudioSource>();
    }

    public void FootStep()
    {
        AudioSource.pitch = 2;      //音の高さを上げてピコピコ
        AudioSource.PlayOneShot(Foot[Random.Range(0, Foot.Length)]);        //ドレミファソラシドの中からランダムにひとつ流れる
    }

    public void SojikiStep()
    {
        AudioSource.pitch = 1;      //掃除機を持っている時は重い感じを演出するため音の高さを低く
        AudioSource.PlayOneShot(Foot[Random.Range(0, Foot.Length)]);
    }

    public void WallupSound()
    {
        //下るときは低い音
        if(kabenobori1.down == true)
        {
            AudioSource.pitch = 1;
            AudioSource.PlayOneShot(Foot[j]);   //ドシラソファミレドの順で流れる
            j--;
            if (j == -1)    //低い方のドまで行くと高い方のドに戻す
            {
                j = 7;
            }
        }

        //登るときは高い音
        else
        {
            AudioSource.pitch = 2;
            AudioSource.PlayOneShot(Foot[i]);   //ドレミファソラシドの順で流れる
            i++;
            if (i == 8) //高い方のドまで行くと低い方のドに戻す
            {
                i = 0;
            }
        }        
    }
}
