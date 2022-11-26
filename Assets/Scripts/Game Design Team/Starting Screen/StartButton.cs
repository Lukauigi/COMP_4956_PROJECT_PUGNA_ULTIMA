using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the starting screen
/// Authors: Xiang Zhu
/// Date: Oct  28 2022
/// Source:
///     How to Make a Main Menu in Unity 2022 - https://www.youtube.com/watch?v=FfaG9TvCe5g&t=742s
/// </summary>
public class StartButton : MonoBehaviour
{
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
      
    }

    /// <summary>
    /// Navigate from starting screen to login screen
    /// </summary>
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("Login Page");
    }
}
