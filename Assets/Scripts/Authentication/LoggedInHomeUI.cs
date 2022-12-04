using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;


public class LoggedInHomeUI : MonoBehaviour
{
    [SerializeField] 
    public TMP_Text WelcomeUserLabel;

    /// <summary>
    /// Author: Jashanpreet Singh
    /// Date: 2022-11-24
    /// Start is called before the first frame update. This method is used to get the user's
    /// display name and display it on the home screen
    /// </summary>
    private void Start()
    {
        var PlayfabId = PlayerPrefsManager.GetPlayfabId();
        var Username = PlayerPrefsManager.GetPlayerName();
        WelcomeUserLabel.text = $"Welcome {Username}, Your PlayfabId is {PlayfabId}, you are logged in!";
    }
    
}
