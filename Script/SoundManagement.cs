using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagement : MonoBehaviour
{
    [SerializeField] private AudioSource koukaon1, koukaon2;
    [SerializeField] private AudioSource bgm1, bgm2;

    public void SoundSliderOnValueChange1(float newSliderValue1)
    {
        koukaon1.volume = newSliderValue1;
        koukaon2.volume = newSliderValue1;
    }

    public void SoundSliderOnValueChange2(float newSliderValue2)
    {
        bgm1.volume = newSliderValue2;
        bgm2.volume = newSliderValue2;
    }
}
