using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip buySound;
    [SerializeField] private GameDirector GameDirector;
    [SerializeField] private Brush brush;
    [SerializeField] private Button Power;
    [SerializeField] private Button Wall;

    [SerializeField] private int PowerUPprice;
    [SerializeField] private int WallUPprice;
    enum PowerState
    {
        buy,
        sold,
    }

    public enum WallUPState
    {
        buy,
        sold,
    }

    public WallUPState wallUPState;
    PowerState powerState;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        powerState = PowerState.buy;
        wallUPState = WallUPState.buy;
    }

    // Update is called once per frame
    void Update()
    {
        if((GameDirector.money < PowerUPprice)||(powerState == PowerState.sold))
        {
            Power.interactable = false;
        }

        if((GameDirector.money < WallUPprice)||(wallUPState == WallUPState.sold))
        {
            Wall.interactable = false;
        }

        if((GameDirector.money >= PowerUPprice) && (powerState == PowerState.buy))
        {
            Power.interactable = true;
        }

        if ((GameDirector.money >= WallUPprice) && (wallUPState == WallUPState.buy))
        {
            Wall.interactable = true;
        }
    }

    public void PowerUp()
    {
        audioSource.PlayOneShot(buySound);
        GameDirector.money = GameDirector.money - PowerUPprice;
        brush.brushWidth = 120;
        brush.brushHeight = 60;
        brush.UpdateBrushColor();
        powerState = PowerState.sold;
    }

    public void WallUPavailable()
    {
        audioSource.PlayOneShot(buySound);
        GameDirector.money = GameDirector.money - WallUPprice;
        wallUPState = WallUPState.sold;
    }

}
