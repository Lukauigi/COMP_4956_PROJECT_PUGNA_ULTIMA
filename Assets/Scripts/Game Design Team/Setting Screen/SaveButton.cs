using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the setting screen
/// Authors: Xiang Zhu
/// Date: Oct  28 2022
/// </summary>
public class SaveButton : MonoBehaviour
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
    /// Navigate back to the login screen and save the settings - (needs to be improved to nagivate back the previous screen since the setting screen can be open through multiple screens.)
    /// </summary>
    public void OnClickSaveButton()
    {
        SceneManager.LoadScene("Login Screen");
    }
}
