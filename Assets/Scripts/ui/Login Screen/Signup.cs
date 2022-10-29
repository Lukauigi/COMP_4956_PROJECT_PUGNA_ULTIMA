using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Signup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Navigate to the setting screen
    public void OnClickSignUpButton()
    {
        SceneManager.LoadScene("Signup Screen");
    }
}
