using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This is the Main Menu
/// Authors: Xiang Zhu
/// Date: Nov 23 2022
/// Source:
///     How to Make a Main Menu in Unity 2022 - https://www.youtube.com/watch?v=FfaG9TvCe5g&t=742s
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text Username;
    
    /// <summary>
    /// Start is called before the first frame update and update the user name to the welcome message.
    /// </summary>
    void Start()
    {
        Username.text = "Welcome " + PlayerPrefs.GetString("PlayerName") + "!";

    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        
    }
}
