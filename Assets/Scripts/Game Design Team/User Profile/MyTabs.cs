using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTabs : MonoBehaviour
{

    public GameObject tabbutton1;
    public GameObject tabbutton2;

    public GameObject tabcontent1;
    public GameObject tabcontent2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideAllTabs()
    {
        tabcontent1.SetActive(false);
        tabcontent2.SetActive(false);
    }

    public void ShowTab1()
    {
        HideAllTabs();
        tabcontent1.SetActive(true);
    }

    public void ShowTab2()
    {
        HideAllTabs();
        tabcontent2.SetActive(true);
    }
}
