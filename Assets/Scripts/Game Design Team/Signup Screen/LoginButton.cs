using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the signup screen
/// Authors: Xiang Zhu
/// Date: Oct  28 2022
/// </summary>
public class LoginButton : MonoBehaviour
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
    /// Navigate back to the login screen
    /// </summary>
    public void OnClickLoginButton()
    {
        SceneManager.LoadScene("Login Screen");
    }
}
