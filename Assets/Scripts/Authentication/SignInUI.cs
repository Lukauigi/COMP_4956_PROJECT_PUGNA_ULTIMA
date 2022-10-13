using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignInUI : MonoBehaviour
{
    [SerializeField] public TMP_InputField Username;
    [SerializeField] public TMP_InputField Password;
    
    /// <summary>
    /// Sign in an existing user
    /// </summary>
    public void SignIn()
    {
        AccountManager.Instance.SignIn(Username.text, Password.text);
        SceneManager.LoadScene("Scenes/LoggedInHome");
    }
}
