using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text Username;
    // Start is called before the first frame update
    void Start()
    {
        Username.text = "Welcome " + PlayerPrefs.GetString("PlayerName") + "!";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
