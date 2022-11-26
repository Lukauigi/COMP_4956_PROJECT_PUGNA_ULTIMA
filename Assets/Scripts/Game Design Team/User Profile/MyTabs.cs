using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the tab screen in user profile.
/// Authors: Xiang Zhu
/// Date: Nov  15 2022
/// Source:
///     How to create navigation tabs and buttons UI in Unity and how to change the buttons color by script - https://www.youtube.com/watch?v=WCRxA3kB584&t=595s
/// </summary>
public class MyTabs : MonoBehaviour
{

    public GameObject tabButton1;
    public GameObject tabButton2;

    public GameObject tabContent1;
    public GameObject tabContent2;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        
    }

    /// <summary>
    /// Hide all the tabs.
    /// </summary>
    public void HideAllTabs()
    {
        tabContent1.SetActive(false);
        tabContent2.SetActive(false);
    }

    /// <summary>
    /// Reveal tab1 screen.
    /// </summary>
    public void ShowTab1()
    {
        HideAllTabs();
        tabContent1.SetActive(true);
    }

    /// <summary>
    /// Reveal tab2 screen.
    /// </summary>
    public void ShowTab2()
    {
        HideAllTabs();
        tabContent2.SetActive(true);
    }
}
