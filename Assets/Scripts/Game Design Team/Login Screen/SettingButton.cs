using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the login screen
/// Authors: Xiang Zhu
/// Date: Oct  28 2022
/// Source:
///     How to Make a Main Menu in Unity 2022 - https://www.youtube.com/watch?v=FfaG9TvCe5g&t=742s
/// </summary>
public class SettingButton : MonoBehaviour
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
    /// Navigate to the setting screen
    /// </summary>
    public void OnClickSettingButton()
    {
        SceneManager.LoadScene("Setting Screen");
    }
}
