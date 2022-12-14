using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//掃除機に関連する効果音・BGMを扱うクラス

public class CleanerSound : MonoBehaviour
{
    [SerializeField] private AudioSource Cleaner_Sound;
    [SerializeField] private AudioSource Cleaner_BGM;
    [SerializeField] private AudioClip Toridasu;
    [SerializeField] private AudioClip Simau;
    
    
    public void Toridasu_Sound()
    {
        Cleaner_Sound.PlayOneShot(Toridasu);
    }

    public void Simau_Sound()
    {
        Cleaner_Sound.PlayOneShot(Simau);
    }

    public void BGM_Play()
    {
        Cleaner_BGM.Play();
    }

    public void BGM_Stop()
    {
        Cleaner_BGM.Stop();
    }
    
   
}
