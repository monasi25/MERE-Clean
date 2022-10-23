using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private AudioSource aud;
    [SerializeField] AudioClip clip1;

    private void Start()
    {
        aud = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            aud.PlayOneShot(clip1);
            FadeManager.Instance.LoadScene("Apartment", 3.0f);
        }
    }
}
