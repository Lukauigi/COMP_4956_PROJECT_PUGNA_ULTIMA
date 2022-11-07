
using System;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using static UserData;

public class AccountManager : MonoBehaviour
{
    /*
    public static string PlayfabId { get; set; }
    */
    
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
    public Boolean CreateAccount(string Username, string Email, string Password)
    {
        var IsRegistered = false;
        PlayFabClientAPI.RegisterPlayFabUser(
            new RegisterPlayFabUserRequest()
            {
                Email = Email,
                Password = Password,
                Username = Username,
                RequireBothUsernameAndEmail = true
            },
            response => { 
                Debug.Log($"User successfully registered | " +
                                    $"Username: {Username} | Email: {Email}"); 
                IsRegistered = true;
                SceneManager.LoadScene("Scenes/Game Design/Screen Navigation/Login Screen");
            },
            error =>
            {
                Debug.Log($"User registration unsuccessful | Error: {error.Error}"); 
                IsRegistered = false;
            }
        );
        return IsRegistered;
    }

    /// <summary>
    /// This method logs the user in.
    /// </summary>
    /// <param name="Username"> a string </param>
    /// <param name="Password"> a string </param>
    public Boolean SignIn(string Username, string Password)
    {
        var IsSignedIn = false;
        // Login with PlayFab
        PlayFabClientAPI.LoginWithPlayFab(
            new LoginWithPlayFabRequest()
            {
                Username = Username,
                Password = Password
            },
            response =>
            {
                // On successful login, set the PlayFabId
                PlayerPrefsManager.SetPlayfabId(response.PlayFabId);
                Debug.Log($"The id is  {response.PlayFabId}");
                Debug.Log($"User successfully logged in | Username: {Username}");
                Debug.Log($"The session ticket is: {response.SessionTicket}");
                IsSignedIn = true;

                //Testing database functions
                UserData.GetUserData(response.PlayFabId, "Favourite Character");
                SceneManager.LoadScene("Scenes/Game Design/Screen Navigation/Main Menu");
            },
            error =>
            {
                // On failed login, log the error
                Debug.Log($"User login unsuccessful | Error: {error.Error}"); 
                IsSignedIn = false;
            }
        );
        return IsSignedIn;
    }

    /*public void SetPlayfabId(string playfabId)
    {
        AccountManager.Instance.PlayfabId = playfabId;
    }*/
}

/// <summary>
/// This class saves the PlayfabId to the PlayerPrefs.
/// </summary>
public static class PlayerPrefsManager
{
    /// <summary>
    /// Sets the PlayfabId to the PlayerPrefs.
    /// </summary>
    /// <param name="playfabId"> a string </param>
    public static void SetPlayfabId(string playfabId)
    {
        PlayerPrefs.SetString("PlayfabId", playfabId);
    }

    /// <summary>
    /// Gets the PlayfabId from the PlayerPrefs.
    /// </summary>
    /// <returns>PlayfabID -> a string </returns>
    public static string GetPlayfabId()
    {
        return PlayerPrefs.GetString("PlayfabId");
    }
}
