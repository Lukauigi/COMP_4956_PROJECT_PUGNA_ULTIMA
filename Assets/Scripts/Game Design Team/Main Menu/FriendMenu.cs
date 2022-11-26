using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the Friend Button in main menu screen.
/// Authors: Xiang Zhu
/// Date: Nov  25 2022
/// </summary>
public class FriendMenu : MonoBehaviour
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
    /// Onclick action to load the scene to friend list screen.
    /// </summary>
    public void OnClickFriendScreen()
    {
        SceneManager.LoadScene("Scenes/Game Design/Screen Navigation/FriendList");
    }
}
