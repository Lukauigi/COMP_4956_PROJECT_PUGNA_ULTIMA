using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for the Sign In UI and button behaviours of the Sign In Screen.
/// Author: Jashanpreet Singh
/// Date: 2022-10-15
/// </summary>
public class SignInUI : MonoBehaviour
{
    [SerializeField] 
    public TMP_InputField Username;
    
    [SerializeField] 
    public TMP_InputField Password;
    
    /// <summary>
    /// Author: Jashanpreet Singh
    /// Date: 2022-11-24
    /// Sign in an existing user
    /// </summary>
    public void SignIn()
    {
        AccountManager.Instance.SignIn(Username.text, Password.text);
    }

    /// <summary>
    /// Author: Xiang Zhu
    /// Date: 2022-11-25
    /// Navigate to the signup page
    /// </summary>
    public void NavigateToSignUp()
    {
        SceneManager.LoadScene("Scenes/Game Design/Screen Navigation/jr/Signup Page");
    }
}
