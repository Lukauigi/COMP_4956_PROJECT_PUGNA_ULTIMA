using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Navigate back to the main screen and save the settings
    public void OnClickSaveButton()
    {
        SceneManager.LoadScene("Main Screen");
    }
}
