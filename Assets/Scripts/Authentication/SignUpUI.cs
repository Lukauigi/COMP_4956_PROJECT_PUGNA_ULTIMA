using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEditor.PackageManager;
using UnityEngine.SceneManagement;

public class SignUpUI : MonoBehaviour
{
    [SerializeField] public TMP_InputField Username;

    [SerializeField] public TMP_InputField Email;

    [SerializeField] public TMP_InputField Password;

    /*[SerializeField] public Button Submit;*/

    /// <summary>
    /// Sign up a new user
    /// </summary>
    public void CreateAccount()
    { 
        AccountManager.Instance.CreateAccount(Username.text, Email.text, Password.text);
    }
    
    /// <summary>
    /// Navigate back to the main menu
    /// </summary>
    public void NavigateBackToHome()
    {
        SceneManager.LoadScene("Scenes/Home");
    }


}
