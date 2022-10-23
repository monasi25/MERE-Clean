using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSound : MonoBehaviour
{
    private AudioSource AudioSource;

    [SerializeField] private kabenobori1 kabenobori1;

    private int i = 0,j=7;

    [SerializeField] private AudioClip[] Foot;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource = this.GetComponent<AudioSource>();
    }

    public void FootStep()
    {
        AudioSource.pitch = 2;
        AudioSource.PlayOneShot(Foot[Random.Range(0, Foot.Length)]);
    }

    public void SojikiStep()
    {
        AudioSource.pitch = 1;
        AudioSource.PlayOneShot(Foot[Random.Range(0, Foot.Length)]);
    }

    public void WallupSound()
    {
        if(kabenobori1.down == true)
        {
            AudioSource.pitch = 1;
            AudioSource.PlayOneShot(Foot[j]);
            j--;
            if (j == -1)
            {
                j = 7;
            }
        }

        else
        {
            AudioSource.pitch = 2;
            AudioSource.PlayOneShot(Foot[i]);
            i++;
            if (i == 8)
            {
                i = 0;
            }
        }        
    }
}
