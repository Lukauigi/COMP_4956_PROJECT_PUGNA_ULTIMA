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
    [SerializeField] public TMP_Text WelcomeUserLabel;

    private void Start()
    {
        var PlayfabId = PlayerPrefsManager.GetPlayfabId();
        WelcomeUserLabel.text = $"Welcome {PlayfabId}, you are logged in!";
    }
    
}
