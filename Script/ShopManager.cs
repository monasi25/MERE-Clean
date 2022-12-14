using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ショップの商品の状態を扱うクラス
public class ShopManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip buySound;
    [SerializeField] private GameDirector GameDirector;
    [SerializeField] private Brush brush;

    [SerializeField] private Button Power;  //購入ボタン
    [SerializeField] private Button Wall;

    [SerializeField] private int PowerUPprice;  //パワーアップチップの値段
    [SerializeField] private int WallUPprice;   //壁登りチップの値段

    //パワーアップチップの状態enum
    public enum PowerState
    {
        buy,
        sold,
    }

    //壁登りチップの状態enum
    public enum WallUPState
    {
        buy,
        sold,
    }

    public WallUPState wallUPState = WallUPState.buy;
    public  PowerState powerState = PowerState.buy;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //お金が足りていない もしくは 既に購入済みの場合購入ボタンを無効化
        if((GameDirector.money < PowerUPprice)||(powerState == PowerState.sold))
        {
            Power.interactable = false;
        }

        //上と同様
        if((GameDirector.money < WallUPprice)||(wallUPState == WallUPState.sold))
        {
            Wall.interactable = false;
        }

        //お金が足りている かつ まだ購入していな場合ボタンを有効化
        if((GameDirector.money >= PowerUPprice) && (powerState == PowerState.buy))
        {
            Power.interactable = true;
        }

        //上と同様
        if ((GameDirector.money >= WallUPprice) && (wallUPState == WallUPState.buy))
        {
            Wall.interactable = true;
        }
    }

    //パワーアップチップ購入時に各種値を変更する関数
    public void PowerUp()
    {
        audioSource.PlayOneShot(buySound);
        GameDirector.money = GameDirector.money - PowerUPprice;     //所持金からチップの値段を引く
        brush.brushWidth = 120;     //吸引範囲を大きくする
        brush.brushHeight = 60;
        brush.UpdateBrushColor();   //ブラシをアップデート
        powerState = PowerState.sold;
        ValueSave.PowerUp = true;       //セーブ用クラスに購入済フラグを立てる
    }

    //壁登りチップ購入時に壁登りを解禁する関数
    public void WallUPavailable()
    {
        audioSource.PlayOneShot(buySound);
        GameDirector.money = GameDirector.money - WallUPprice;
        wallUPState = WallUPState.sold;
        ValueSave.WallUp = true;
    }

}
