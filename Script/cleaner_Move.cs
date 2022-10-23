using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cleaner_Move : MonoBehaviour
{

    public movetest4 movetest4;
    public CleanerSwitch cleanerswitch;
    public kabenobori1 kabenobori1;

    [SerializeField] CleanerSound CleanerSound;

    public AudioSource audi;
    public AudioSource audi2;

    [SerializeField]
    private AudioClip hassan;

    [SerializeField]
    private AudioClip syuusoku;

    [SerializeField]
    private AudioClip kirikae;

    [SerializeField]
    private AudioClip soujityuu;

    [SerializeField]
    private ParticleSystem Trail;

    [SerializeField]
    private ParticleSystem suikomi;

    [SerializeField]
    private GameObject suiko;

    [SerializeField]
    private GameObject parti;

    [SerializeField]
    private ParticleSystem Smoke;

    [SerializeField]
    private GameObject smok;

    public Transform onG;
    public Transform taikiG;

    public bool flag = false;

    private Animator anim;

    private int count = -1;

    public GameObject so;

    private float i = 0;

    private bool once = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //audi = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchON();       
    }

    private void SwitchON()
    {
        if (cleanerswitch.Cleaners == true)
        {
            switch (count)
            {
                case -1:
                    StartCoroutine("wait");
                    StartCoroutine("toridasu");                   
                    //audi.PlayOneShot(hassan);
                    break;

                case 0:                                             //ソージキを構える最初の処理                   
                    anim.SetBool("isCleaner_taiki", true);
                    movetest4.idoSpeed = 0.27f;
                    so.transform.position = taikiG.position;        //ソージキのポジションとrotationを待機ポジに
                    so.transform.rotation = taikiG.rotation;                   
                    count = 1;                                      //countを1にしてcase1に遷移
                    break;

                case 1:                                             //左クリックで掃除モード  待機モーション中は歩行と待機のみ
                    movetest4.idoSpeed = 0.27f;

                    if ((Input.GetMouseButtonDown(0)))
                    {
                        //audi.PlayOneShot(kirikae);
                        anim.SetBool("isCleaner_IDLE", true);
                        so.transform.position = onG.position;       //ソージキのポジションとrotationを構えポジに
                        so.transform.rotation = onG.rotation;
                        movetest4.idoSpeed = 0.2f;                  //移動速度を0.2にして掃除をしてる感を出す
                        flag = true;
                        suiko.SetActive(true);
                        suikomi.Play();
                        smok.SetActive(true);
                        Smoke.Play();
                        CleanerSound.BGM_Play();
                        //audi2.Play();
                        count = 2;                                  //countを2にしてcase2に遷移
                    }

                    else if (movetest4.on == true)
                    {
                        anim.SetBool("isCleaner_taikiwalk", true);
                    }

                    else
                    {
                        anim.SetBool("isCleaner_taikiwalk", false);
                    }

                    break;

                case 2:
                    if (movetest4.on == true)
                    {
                        anim.SetBool("isCleaner_IDO", true);
                    }

                    if (movetest4.on == false)
                    {
                        anim.SetBool("isCleaner_IDO", false);
                    }

                    if ((Input.GetMouseButtonDown(0)))
                    {
                        //audi2.Stop();
                        //audi.PlayOneShot(kirikae);
                        anim.SetBool("isCleaner_IDLE", false);
                        so.transform.position = taikiG.position;
                        so.transform.rotation = taikiG.rotation;
                        movetest4.idoSpeed = 0.5f;
                        flag = false;
                        suiko.SetActive(false);
                        smok.SetActive(false);
                        CleanerSound.BGM_Stop();
                        count = 1;
                    }

                    break;
            }
        }

        if ((cleanerswitch.co == 1))                                //OFF処理
        {
            //audi.PlayOneShot(syuusoku);
            parti.SetActive(true);
            Trail.Play();
            suiko.SetActive(false);
            smok.SetActive(false);
            StartCoroutine("simau");
            anim.SetBool("isCleaner_IDO", false);
            anim.SetBool("isCleaner_IDLE", false);
            anim.SetBool("isCleaner_taikiwalk", false);
            anim.SetBool("isCleaner_taiki", false);
            anim.SetBool("isCleaner_ONOFF", false);
            movetest4.idoSpeed = 0.5f;
            flag = false;
            kabenobori1.enabled = true;
            CleanerSound.BGM_Stop();
            count = -1;
        }
    }

    private IEnumerator toridasu()                                  //localscaleを0.01秒に一回0.01加算してアイテムを取り出している感を演出　　下は逆にしまう演出
    {
        for (i = 0; i < 0.07f; i += 0.01f)
        {
            so.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator simau()
    {
        for (i = 0.07f; i >= 0f; i -= 0.01f)
        {           
            so.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.03f);
        }
    }

    private IEnumerator wait()
    {
        parti.SetActive(true);
        Trail.Play();
        anim.SetBool("isCleaner_ONOFF", true);       
        yield return new WaitForSeconds(0.5f);       
        count = 0;
    }

}

