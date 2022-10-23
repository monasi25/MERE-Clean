using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerSound : MonoBehaviour
{
    [SerializeField] private AudioSource Cleaner_Sound;
    [SerializeField] private AudioSource Cleaner_BGM;
    [SerializeField] private AudioClip Toridasu;
    [SerializeField] private AudioClip Simau;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
    // Update is called once per frame
    void Update()
    {
        
    }
}
