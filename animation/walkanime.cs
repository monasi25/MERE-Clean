using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkanime : MonoBehaviour
{
    private Animator animator;

    private const string key_iswalk = "iswalk";
    private const string key_isjump = "isjump";
    Rigidbody pla;
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        pla = GameObject.Find("Player").GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = pla.velocity.magnitude;
        Debug.Log(speed);
        if ((Input.GetKey(KeyCode.W))||(Input.GetKey(KeyCode.A))||(Input.GetKey(KeyCode.S))||(Input.GetKey(KeyCode.D)))
        {
            animator.SetBool(key_iswalk, true);
        }
        else
        {
            animator.SetBool(key_iswalk, false);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool(key_isjump, true);
        }
        else
        {
            animator.SetBool(key_isjump, false);
        }
    }
}
