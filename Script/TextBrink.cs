using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//タイトル画面のテキストを点滅させるためのクラス
public class TextBrink : MonoBehaviour
{
    private Text Text;
    private float speed = 0.5f;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        Text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Text.color = GetAlpha(Text.color);
    }

    Color GetAlpha(Color color)
    {
        time += Time.deltaTime * 5.0f * speed;
        color.a = Mathf.Sin(time);
        return color;
    }
}
