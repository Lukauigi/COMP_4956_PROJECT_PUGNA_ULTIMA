using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the BackButton in User Profile Screen.
/// Authors: Xiang Zhu
/// Date: Nov 20 2022
/// </summary>
public class BackButton : MonoBehaviour
{
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
    /// Back Button Onclick Action to navigate back to the main menu.
    /// </summary>
    public void OnClickBackButton()
    {
        SceneManager.LoadScene("Scenes/Game Design/Screen Navigation/jr/Main Menu");
    }
}
