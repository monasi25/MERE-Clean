using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//コインを回転させるクラス
public class coinmove : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 0, 1);
    }
}
