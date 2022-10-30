using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    [SerializeField] public Button SignInButton = null;

    [SerializeField] public Button SignUpButton = null;
    // Start is called before the first frame update
    void Start()
    {
        SignInButton.onClick.AddListener(NavigateToSignInScene);
        SignUpButton.onClick.AddListener(NavigateToSignUpScene);
    }

    void NavigateToSignInScene()
    {
        SceneManager.LoadScene("Scenes/Authentication Skeleton Scenes/SignIn");
    }

    void NavigateToSignUpScene()
    {
        SceneManager.LoadScene("Scenes/Authentication Skeleton Scenes/SignUp");
    }
}
