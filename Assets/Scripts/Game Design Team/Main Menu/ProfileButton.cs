using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the Profile Button to user profile screen.
/// Authors: Xiang Zhu
/// Date: Oct  28 2022
/// </summary>
public class ProfileButton : MonoBehaviour
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
    /// Button Onclick activity to load the scene to Profile Screen.
    /// </summary>
    public void OnClickProfileButton()
    {
        SceneManager.LoadScene("Scenes/Game Design/Screen Navigation/jr/Profile Screen");
    }
}
