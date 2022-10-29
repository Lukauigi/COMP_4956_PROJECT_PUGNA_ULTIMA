using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingButton : MonoBehaviour
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
    public void OnClickSettingButton()
    {
        SceneManager.LoadScene("Setting Screen");
    }
}
