using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance;
    public void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// This method creates/registers the user in the Playfab DB.
    /// </summary>
    /// <param name="Username"> a string </param>
    /// <param name="Email">a string </param>
    /// <param name="Password">a string </param>
    public void CreateAccount(string Username, string Email, string Password)
    {
        PlayFabClientAPI.RegisterPlayFabUser(
            new RegisterPlayFabUserRequest()
            {
                Email = Email,
                Password = Password,
                Username = Username,
                RequireBothUsernameAndEmail = true
            },
            response =>
            {
                Debug.Log($"User successfully registered | Username: {Username} | Email: {Email}");
            },
            error =>
            {
                Debug.Log($"User registration unsuccessful | Error: {error.Error}");
            }
        );
    }
    
    /// <summary>
    /// This method logs the user in.
    /// </summary>
    /// <param name="Username"> a string </param>
    /// <param name="Password"> a string </param>
    public void SignIn(string Username, string Password)
    {
        PlayFabClientAPI.LoginWithPlayFab(
            new LoginWithPlayFabRequest()
            {
                Username = Username,
                Password = Password
            },
            response =>
            {
                Debug.Log($"The id id  {response.PlayFabId}");
                Debug.Log($"User successfully logged in | Username: {Username}");
                Debug.Log($"The session ticket is: {response.SessionTicket}");
            },
            error =>
            {
                Debug.Log($"User login unsuccessful | Error: {error.Error}");
            }
        );
    }
}
