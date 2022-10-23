using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Brink : MonoBehaviour
{
    private float speed = 0.5f;
    private Image Image;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        Image = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Image.color = GetAlphaColor(Image.color);
    }

    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 5.0f * speed;
        color.a = Mathf.Sin(time);
        return color;
    }
}
