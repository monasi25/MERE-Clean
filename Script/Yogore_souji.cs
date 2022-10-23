using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yogore_souji : MonoBehaviour
{
    private GameObject Yogore;

    [SerializeField] private Souji_Manager Souji_Manager;
    [SerializeField] private GameDirector director;
    [SerializeField] private GameObject YogoreEfect;

    private int myNumber;
    private float i;
    private bool once = false;
    // Start is called before the first frame update
    void Start()
    {
        Yogore = this.gameObject;

        for (int i = 0; i < Souji_Manager.Yogore_Parent.transform.childCount; i++)
        {
            if (Yogore == Souji_Manager.Yogore_Child[i])
            {
                myNumber = i;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((Souji_Manager.Yogore_Detection[myNumber] == true) && (once == false))
        {
            StartCoroutine(syusoku());
            once = true;
        }
       
        if (Yogore.transform.localScale.x <= 0.3)
        {
            director.Yogore();
            Instantiate(YogoreEfect, Yogore.transform.position, YogoreEfect.transform.rotation);
            Yogore.SetActive(false);
        }
        
    }

    private IEnumerator syusoku()
    {
        for (i = 1f; i >= 0f; i -= 0.01f)
        {
            Yogore.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
