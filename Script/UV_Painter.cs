using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UV_Painter : MonoBehaviour
{
    public Brush _brush;

    private float CleanPercent = 94f;

    [SerializeField] private GameObject Kirakira;

    [SerializeField] private GameObject Kirakira2;

    [SerializeField] private List<GameObject> paintObj = new List<GameObject>();

    [SerializeField]
    private GameObject _paintObj1;

    [SerializeField]
    private GameObject _paintObj2;

    [SerializeField]
    private GameObject _paintObj3;

    [SerializeField]
    private GameObject _paintObj4;

    [SerializeField]
    private GameObject _paintObj5;

    [SerializeField]
    private GameObject _paintObj6;

    [SerializeField]
    private GameObject _paintObj7;

    [SerializeField]
    private GameObject _paintObj8,_paintObj9,_paintObj10;

    private Texture2D _tex1;
    private Texture2D _tex2;
    private Texture2D _tex3;
    private Texture2D _tex4;
    private Texture2D _tex5;
    private Texture2D _tex6;
    private Texture2D _tex7;
    private Texture2D _tex8;
    private Texture2D _tex9;
    private Texture2D _tex10;

   　public struct tex  
     {
        public Texture2D hokori;
     }
     List<tex> tex_list = new List<tex>();

    [SerializeField]
    private GameObject _brushObj;
    
    [SerializeField]
    private GameDirector gameDirector;

    private float distance = 1f;

    [SerializeField]
    private cleaner_Move cleaner_Move;

    [SerializeField] private List<GameObject> dirt = new List<GameObject>();

    [SerializeField]
    private GameObject dirt1;

    [SerializeField]
    private GameObject dirt2;

    [SerializeField]
    private GameObject dirt3;

    [SerializeField]
    private GameObject dirt4;

    [SerializeField]
    private GameObject dirt5;

    [SerializeField]
    private GameObject dirt6,dirt7,dirt8;

    private IEnumerator syori1;
    private IEnumerator syori2;
    private IEnumerator syori3;
    private IEnumerator syori4;
    private IEnumerator syori5;
    private IEnumerator syori6;
    private IEnumerator syori7;
    private IEnumerator syori8;
    private float i;
    private int j;
    void Start()
    {       
        /*tex tmp = tex_list[0];
        tmp.hokori = new Texture2D(_paintObj1.GetComponent<Renderer>().material.mainTexture.width, _paintObj1.GetComponent<Renderer>().material.mainTexture.height, TextureFormat.RGBA32, false);
        tex_list[0] = tmp;*/

        _tex1 = new Texture2D(_paintObj1.GetComponent<Renderer>().material.mainTexture.width, _paintObj1.GetComponent<Renderer>().material.mainTexture.height,TextureFormat.RGBA32,false);        
        Graphics.CopyTexture(_paintObj1.GetComponent<Renderer>().material.mainTexture, 0, 0, _tex1, 0, 0);

        _tex2 = new Texture2D(_paintObj2.GetComponent<Renderer>().material.mainTexture.width, _paintObj2.GetComponent<Renderer>().material.mainTexture.height);
        Graphics.CopyTexture(_paintObj2.GetComponent<Renderer>().material.mainTexture, 0, 0, _tex2, 0, 0);
        
        _tex3 = new Texture2D(_paintObj3.GetComponent<Renderer>().material.mainTexture.width, _paintObj3.GetComponent<Renderer>().material.mainTexture.height);
        Graphics.CopyTexture(_paintObj3.GetComponent<Renderer>().material.mainTexture, 0, 0, _tex3, 0, 0);
        
        _tex4 = new Texture2D(_paintObj4.GetComponent<Renderer>().material.mainTexture.width, _paintObj4.GetComponent<Renderer>().material.mainTexture.height);
        Graphics.CopyTexture(_paintObj4.GetComponent<Renderer>().material.mainTexture, 0, 0, _tex4, 0, 0);
        
        _tex5 = new Texture2D(_paintObj5.GetComponent<Renderer>().material.mainTexture.width, _paintObj5.GetComponent<Renderer>().material.mainTexture.height);
        Graphics.CopyTexture(_paintObj5.GetComponent<Renderer>().material.mainTexture, 0, 0, _tex5, 0, 0);
        
        _tex6 = new Texture2D(_paintObj6.GetComponent<Renderer>().material.mainTexture.width, _paintObj6.GetComponent<Renderer>().material.mainTexture.height);
        Graphics.CopyTexture(_paintObj6.GetComponent<Renderer>().material.mainTexture, 0, 0, _tex6, 0, 0);
        
        _tex7 = new Texture2D(_paintObj7.GetComponent<Renderer>().material.mainTexture.width, _paintObj7.GetComponent<Renderer>().material.mainTexture.height);
        Graphics.CopyTexture(_paintObj7.GetComponent<Renderer>().material.mainTexture, 0, 0, _tex7, 0, 0);

        _tex8 = new Texture2D(_paintObj8.GetComponent<Renderer>().material.mainTexture.width, _paintObj8.GetComponent<Renderer>().material.mainTexture.height);
        Graphics.CopyTexture(_paintObj8.GetComponent<Renderer>().material.mainTexture, 0, 0, _tex8, 0, 0);

        _tex9 = new Texture2D(_paintObj9.GetComponent<Renderer>().material.mainTexture.width, _paintObj9.GetComponent<Renderer>().material.mainTexture.height);
        Graphics.CopyTexture(_paintObj9.GetComponent<Renderer>().material.mainTexture, 0, 0, _tex9, 0, 0);

        _tex10 = new Texture2D(_paintObj10.GetComponent<Renderer>().material.mainTexture.width, _paintObj10.GetComponent<Renderer>().material.mainTexture.height);
        Graphics.CopyTexture(_paintObj10.GetComponent<Renderer>().material.mainTexture, 0, 0, _tex10, 0, 0);

        _brush.UpdateBrushColor();

        syori1 = syusoku1();
        syori2 = syusoku2();
        syori3 = syusoku3();
        syori4 = syusoku4();
        syori5 = syusoku5();
        syori6 = syusoku6();
        syori7 = syusoku7();
        syori8 = syusoku8();

    }

    void Update()
    {
        
        //if (!Input.GetMouseButton(0)) return;
        var ray = new Ray(_brushObj.transform.position, new Vector3(0, -1, 0));
        //Debug.DrawRay(_brushObj.transform.position, Vector3.forward, Color.red);
        //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (cleaner_Move.flag == true)
        {
            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.collider.tag == "dust")
                {
                    Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
                    MeshCollider meshCollider = hit.collider as MeshCollider;

                    if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || meshCollider == null)
                    {
                        Debug.Log("NULL");
                        return;
                    }

                    if (hit.collider.name == "Hokori1")
                    {
                        Vector2 pixelUV1 = hit.textureCoord;
                        pixelUV1.x *= _tex1.width;
                        pixelUV1.y *= _tex1.height;
                        // Debug.Log("pixelUV:::" + (int)pixelUV.x + " , " + (int)pixelUV.y);

                        _tex1.SetPixels((int)pixelUV1.x - _brush.brushWidth / 2, (int)pixelUV1.y - _brush.brushHeight / 2, _brush.brushWidth, _brush.brushHeight, _brush.colors);
                        _tex1.Apply();
                        renderer.material.mainTexture = _tex1;                       
                        Check(1);

                    }
                    
                    if (hit.collider.name == "Hokori2")
                    {
                        Vector2 pixelUV2 = hit.textureCoord;
                        pixelUV2.x *= _tex2.width;
                        pixelUV2.y *= _tex2.height;

                        _tex2.SetPixels((int)pixelUV2.x - _brush.brushWidth / 2, (int)pixelUV2.y - _brush.brushHeight / 2, _brush.brushWidth, _brush.brushHeight, _brush.colors);
                        _tex2.Apply();
                        renderer.material.mainTexture = _tex2;
                        Check(2);
                    }

                    if (hit.collider.name == "Hokori3")
                    {
                        Vector2 pixelUV3 = hit.textureCoord;
                        pixelUV3.x *= _tex3.width;
                        pixelUV3.y *= _tex3.height;

                        _tex3.SetPixels((int)pixelUV3.x - _brush.brushWidth / 2, (int)pixelUV3.y - _brush.brushHeight / 2, _brush.brushWidth, _brush.brushHeight, _brush.colors);
                        _tex3.Apply();
                        renderer.material.mainTexture = _tex3;
                        Check(3);
                    }

                    if (hit.collider.name == "Hokori4")
                    {
                        Vector2 pixelUV4 = hit.textureCoord;
                        pixelUV4.x *= _tex4.width;
                        pixelUV4.y *= _tex4.height;

                        _tex4.SetPixels((int)pixelUV4.x - _brush.brushWidth / 2, (int)pixelUV4.y - _brush.brushHeight / 2, _brush.brushWidth, _brush.brushHeight, _brush.colors);
                        _tex4.Apply();
                        renderer.material.mainTexture = _tex4;
                        Check(4);
                    }

                    if (hit.collider.name == "Hokori5")
                    {
                        Vector2 pixelUV5 = hit.textureCoord;
                        pixelUV5.x *= _tex5.width;
                        pixelUV5.y *= _tex5.height;

                        _tex5.SetPixels((int)pixelUV5.x - _brush.brushWidth / 2, (int)pixelUV5.y - _brush.brushHeight / 2, _brush.brushWidth, _brush.brushHeight, _brush.colors);
                        _tex5.Apply();
                        renderer.material.mainTexture = _tex5;
                        Check(5);
                    }

                    if (hit.collider.name == "Hokori6")
                    {
                        Vector2 pixelUV6 = hit.textureCoord;
                        pixelUV6.x *= _tex6.width;
                        pixelUV6.y *= _tex6.height;

                        _tex6.SetPixels((int)pixelUV6.x - _brush.brushWidth / 2, (int)pixelUV6.y - _brush.brushHeight / 2, _brush.brushWidth, _brush.brushHeight, _brush.colors);
                        _tex6.Apply();
                        renderer.material.mainTexture = _tex6;
                        Check(6);
                    }

                    if (hit.collider.name == "Hokori7")
                    {
                        Vector2 pixelUV7 = hit.textureCoord;
                        pixelUV7.x *= _tex7.width;
                        pixelUV7.y *= _tex7.height;

                        _tex7.SetPixels((int)pixelUV7.x - _brush.brushWidth / 2, (int)pixelUV7.y - _brush.brushHeight / 2, _brush.brushWidth, _brush.brushHeight, _brush.colors);
                        _tex7.Apply();
                        renderer.material.mainTexture = _tex7;
                        Check(7);
                    }

                    if (hit.collider.name == "Hokori8")
                    {
                        Vector2 pixelUV8 = hit.textureCoord;
                        pixelUV8.x *= _tex8.width;
                        pixelUV8.y *= _tex8.height;

                        _tex8.SetPixels((int)pixelUV8.x - _brush.brushWidth / 2, (int)pixelUV8.y - _brush.brushHeight / 2, _brush.brushWidth, _brush.brushHeight, _brush.colors);
                        _tex8.Apply();
                        renderer.material.mainTexture = _tex8;
                        Check(8);
                    }

                    if (hit.collider.name == "Hokori9")
                    {
                        Vector2 pixelUV9 = hit.textureCoord;
                        pixelUV9.x *= _tex9.width;
                        pixelUV9.y *= _tex9.height;

                        _tex9.SetPixels((int)pixelUV9.x - _brush.brushWidth / 2, (int)pixelUV9.y - _brush.brushHeight / 2, _brush.brushWidth, _brush.brushHeight, _brush.colors);
                        _tex9.Apply();
                        renderer.material.mainTexture = _tex9;
                        Check(9);
                    }

                    if (hit.collider.name == "Hokori10")
                    {
                        Vector2 pixelUV10 = hit.textureCoord;
                        pixelUV10.x *= _tex10.width;
                        pixelUV10.y *= _tex10.height;

                        _tex10.SetPixels((int)pixelUV10.x - _brush.brushWidth / 2, (int)pixelUV10.y - _brush.brushHeight / 2, _brush.brushWidth, _brush.brushHeight, _brush.colors);
                        _tex10.Apply();
                        renderer.material.mainTexture = _tex10;
                        Check(10);
                    }
                }

                if(hit.collider.tag == "dirt")
                {
                    if (hit.collider.name == "Dirt1")
                    {
                        StartCoroutine(syori1);
                    }

                    if(hit.collider.name == "Dirt2")
                    {
                        StartCoroutine(syori2);
                    }
                    
                    if (hit.collider.name == "Dirt3")
                    {
                        StartCoroutine(syori3);
                    }

                    if (hit.collider.name == "Dirt4")
                    {
                        StartCoroutine(syori4);
                    }

                    if (hit.collider.name == "Dirt5")
                    {
                        StartCoroutine(syori5);
                    }

                    if (hit.collider.name == "Dirt6")
                    {
                        StartCoroutine(syori6);
                    }

                    if (hit.collider.name == "Dirt7")
                    {
                        StartCoroutine(syori7);
                    }

                    if (hit.collider.name == "Dirt8")
                    {
                        StartCoroutine(syori8);
                    }
                }

                if(hit.collider.tag == "Ground")
                {
                    StopAllCoroutines();
                }
            }

        }

        /*if(cleaner_Move.flag == false)
        {
            StopAllCoroutines();
        }*/

        if (dirt1.activeSelf == true)
        {
            if (dirt1.transform.localScale.x <= 0.3)
            {
                gameDirector.Yogore();
                Instantiate(Kirakira2, dirt1.transform.position,Kirakira2.transform.rotation);
                dirt1.SetActive(false);
               
            }
        }


        if (dirt2.activeSelf == true)
        {
            if (dirt2.transform.localScale.x <= 0.3)
            {
                gameDirector.Yogore();
                Instantiate(Kirakira2, dirt2.transform.position, Kirakira2.transform.rotation);
                dirt2.SetActive(false);
                              
            }
        }

        if (dirt3.activeSelf == true)
        {
            if (dirt3.transform.localScale.x <= 0.3)
            {
                gameDirector.Yogore();
                Instantiate(Kirakira2, dirt3.transform.position, Kirakira2.transform.rotation);
                dirt3.SetActive(false);
                
                
            }
        }

        if (dirt4.activeSelf == true)
        {
            if (dirt4.transform.localScale.x <= 0.3)
            {
                gameDirector.Yogore();
                Instantiate(Kirakira2, dirt4.transform.position, Kirakira2.transform.rotation);
                dirt4.SetActive(false);
                
                
            }
        }

        if (dirt5.activeSelf == true)
        {
            if (dirt5.transform.localScale.x <= 0.3)
            {
                gameDirector.Yogore();
                Instantiate(Kirakira2, dirt5.transform.position, Kirakira2.transform.rotation);
                dirt5.SetActive(false);
                
                
            }
        }

        if (dirt6.activeSelf == true)
        {
            if (dirt6.transform.localScale.x <= 0.3)
            {
                gameDirector.Yogore();
                Instantiate(Kirakira2, dirt6.transform.position, Kirakira2.transform.rotation);
                dirt6.SetActive(false);
                
            }
        }

        if (dirt7.activeSelf == true)
        {
            if (dirt7.transform.localScale.x <= 0.3)
            {
                gameDirector.Yogore();
                Instantiate(Kirakira2, dirt7.transform.position, Kirakira2.transform.rotation);
                dirt7.SetActive(false);

            }
        }

        if (dirt8.activeSelf == true)
        {
            if (dirt8.transform.localScale.x <= 0.3)
            {
                gameDirector.Yogore();
                Instantiate(Kirakira2, dirt8.transform.position, Kirakira2.transform.rotation);
                dirt8.SetActive(false);

            }
        }
    }

    void Check(int no)
    {
        int count = 0;
        var magnification = 100;
        switch (no)
        {
            case 1:
                var pixels1 = _tex1.GetPixels(0, 0, _tex1.width, _tex1.height);
                foreach (var pixel1 in pixels1)
                {
                    if (pixel1.a == 0f)
                    {
                        count++;
                    }
                }               
                var pixelTexture1 = pixels1.Length / pixels1.Length;
                var pixelAlpha1 = (float)count / (float)pixels1.Length;
                var numPixelTexture1 = (pixelTexture1 * magnification);
                var numPixelAlpha1 = (pixelAlpha1 * magnification);
                if (numPixelAlpha1 >= CleanPercent)
                {
                    Instantiate(Kirakira, _paintObj1.transform.position,Kirakira.transform.rotation);
                    Destroy(_paintObj1);
                    gameDirector.Cleaner();
                    Debug.Log("掃除完了2");
                }
                break;

            case 2:
                var pixels2 = _tex2.GetPixels(0, 0, _tex2.width, _tex2.height);
                foreach (var pixel2 in pixels2)
                {
                    if (pixel2.a == 0f)
                    {
                        count++;
                    }
                }
                var pixelTexture2 = pixels2.Length / pixels2.Length;
                var pixelAlpha2 = (float)count / (float)pixels2.Length;
                var numPixelTexture2 = (pixelTexture2 * magnification);
                var numPixelAlpha2 = (pixelAlpha2 * magnification);
                if (numPixelAlpha2 >= CleanPercent)
                {
                    Instantiate(Kirakira, _paintObj2.transform.position, Kirakira.transform.rotation);
                    Destroy(_paintObj2);
                    gameDirector.Cleaner();
                    Debug.Log("掃除完了2");
                }
                break;

            case 3:
                var pixels3 = _tex3.GetPixels(0, 0, _tex3.width, _tex3.height);
                foreach (var pixel3 in pixels3)
                {
                    if (pixel3.a == 0f)
                    {
                        count++;
                    }
                }
                var pixelTexture3 = pixels3.Length / pixels3.Length;
                var pixelAlpha3 = (float)count / (float)pixels3.Length;
                var numPixelTexture3 = (pixelTexture3 * magnification);
                var numPixelAlpha3 = (pixelAlpha3 * magnification);
                if (numPixelAlpha3 >= CleanPercent)
                {
                    Instantiate(Kirakira, _paintObj3.transform.position, Kirakira.transform.rotation);
                    Destroy(_paintObj3);
                    gameDirector.Cleaner();
                    Debug.Log("掃除完了3");
                }
                break;

            case 4:
                var pixels4 = _tex4.GetPixels(0, 0, _tex4.width, _tex4.height);
                foreach (var pixel4 in pixels4)
                {
                    if (pixel4.a == 0f)
                    {
                        count++;
                    }
                }
                var pixelTexture4 = pixels4.Length / pixels4.Length;
                var pixelAlpha4 = (float)count / (float)pixels4.Length;
                var numPixelTexture4 = (pixelTexture4 * magnification);
                var numPixelAlpha4 = (pixelAlpha4 * magnification);
                if (numPixelAlpha4 >= CleanPercent)
                {
                    Instantiate(Kirakira, _paintObj4.transform.position, Kirakira.transform.rotation);
                    Destroy(_paintObj4);
                    gameDirector.Cleaner();
                    Debug.Log("掃除完了4");
                }
                break;

            case 5:
                var pixels5 = _tex5.GetPixels(0, 0, _tex5.width, _tex5.height);
                foreach (var pixel5 in pixels5)
                {
                    if (pixel5.a == 0f)
                    {
                        count++;
                    }
                }
                var pixelTexture5 = pixels5.Length / pixels5.Length;
                var pixelAlpha5 = (float)count / (float)pixels5.Length;
                var numPixelTexture5 = (pixelTexture5 * magnification);
                var numPixelAlpha5 = (pixelAlpha5 * magnification);
                if (numPixelAlpha5>= CleanPercent)
                {
                    Instantiate(Kirakira, _paintObj5.transform.position, Kirakira.transform.rotation);
                    Destroy(_paintObj5);
                    gameDirector.Cleaner();
                    Debug.Log("掃除完了5");
                }
                break;

            case 6:
                var pixels6 = _tex6.GetPixels(0, 0, _tex6.width, _tex6.height);
                foreach (var pixel6 in pixels6)
                {
                    if (pixel6.a == 0f)
                    {
                        count++;
                    }
                }
                var pixelTexture6 = pixels6.Length / pixels6.Length;
                var pixelAlpha6 = (float)count / (float)pixels6.Length;
                var numPixelTexture6 = (pixelTexture6 * magnification);
                var numPixelAlpha6 = (pixelAlpha6 * magnification);
                if (numPixelAlpha6 >= CleanPercent)
                {
                    Instantiate(Kirakira, _paintObj6.transform.position, Kirakira.transform.rotation);
                    Destroy(_paintObj6);
                    gameDirector.Cleaner();
                    Debug.Log("掃除完了6");
                }
                break;

            case 7:
                var pixels7 = _tex7.GetPixels(0, 0, _tex7.width, _tex7.height);
                foreach (var pixel7 in pixels7)
                {
                    if (pixel7.a == 0f)
                    {
                        count++;
                    }
                }
                var pixelTexture7 = pixels7.Length / pixels7.Length;
                var pixelAlpha7 = (float)count / (float)pixels7.Length;
                var numPixelTexture7 = (pixelTexture7 * magnification);
                var numPixelAlpha7 = (pixelAlpha7 * magnification);
                if (numPixelAlpha7 >= CleanPercent)
                {
                    Instantiate(Kirakira, _paintObj7.transform.position, Kirakira.transform.rotation);
                    Destroy(_paintObj7);
                    gameDirector.Cleaner();
                    Debug.Log("掃除完了7");
                }
                break;

            case 8:
                var pixels8 = _tex8.GetPixels(0, 0, _tex8.width, _tex8.height);
                foreach (var pixel8 in pixels8)
                {
                    if (pixel8.a == 0f)
                    {
                        count++;
                    }
                }
                var pixelTexture8 = pixels8.Length / pixels8.Length;
                var pixelAlpha8 = (float)count / (float)pixels8.Length;
                var numPixelTexture8 = (pixelTexture8 * magnification);
                var numPixelAlpha8 = (pixelAlpha8 * magnification);
                if (numPixelAlpha8 >= CleanPercent)
                {
                    Instantiate(Kirakira, _paintObj8.transform.position, Kirakira.transform.rotation);
                    Destroy(_paintObj8);
                    gameDirector.Cleaner();
                    Debug.Log("掃除完了8");
                }
                break;

            case 9:
                var pixels9 = _tex9.GetPixels(0, 0, _tex9.width, _tex9.height);
                foreach (var pixel9 in pixels9)
                {
                    if (pixel9.a == 0f)
                    {
                        count++;
                    }
                }
                var pixelTexture9 = pixels9.Length / pixels9.Length;
                var pixelAlpha9 = (float)count / (float)pixels9.Length;
                var numPixelTexture9 = (pixelTexture9 * magnification);
                var numPixelAlpha9 = (pixelAlpha9 * magnification);
                if (numPixelAlpha9 >= CleanPercent)
                {
                    Instantiate(Kirakira, _paintObj9.transform.position, Kirakira.transform.rotation);
                    Destroy(_paintObj9);
                    gameDirector.Cleaner();
                    Debug.Log("掃除完了9");
                }
                break;

            case 10:
                var pixels10 = _tex10.GetPixels(0, 0, _tex10.width, _tex10.height);
                foreach (var pixel10 in pixels10)
                {
                    if (pixel10.a == 0f)
                    {
                        count++;
                    }
                }
                var pixelTexture10 = pixels10.Length / pixels10.Length;
                var pixelAlpha10 = (float)count / (float)pixels10.Length;
                var numPixelTexture10 = (pixelTexture10 * magnification);
                var numPixelAlpha10 = (pixelAlpha10 * magnification);
                if (numPixelAlpha10 >= CleanPercent)
                {
                    Instantiate(Kirakira, _paintObj10.transform.position, Kirakira.transform.rotation);
                    Destroy(_paintObj10);
                    gameDirector.Cleaner();
                    Debug.Log("掃除完了10");
                }
                break;

        } 
     

    }

    private IEnumerator syusoku1()
    {
        for (i = 1f; i >= 0f; i -= 0.001f)
        {
            dirt1.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator syusoku2()
    {
        for (i = 1f; i >= 0f; i -= 0.001f)
        {
            dirt2.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator syusoku3()
    {
        for (i = 1f; i >= 0f; i -= 0.001f)
        {
            dirt3.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator syusoku4()
    {
        for (i = 1f; i >= 0f; i -= 0.001f)
        {
            dirt4.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator syusoku5()
    {
        for (i = 1f; i >= 0f; i -= 0.001f)
        {
            dirt5.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator syusoku6()
    {
        for (i = 1f; i >= 0f; i -= 0.001f)
        {
            dirt6.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator syusoku7()
    {
        for (i = 1f; i >= 0f; i -= 0.001f)
        {
            dirt7.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator syusoku8()
    {
        for (i = 1f; i >= 0f; i -= 0.001f)
        {
            dirt8.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
