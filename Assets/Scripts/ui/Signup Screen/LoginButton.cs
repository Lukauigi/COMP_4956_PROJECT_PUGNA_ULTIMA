using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Navigate back to the main screen
    public void OnClickLoginButton()
    {
        SceneManager.LoadScene("Main Screen");
    }
}
