
using System;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using static UserData;

/// <summary>
/// Author: Jashanpreet Singh
/// Date: 2020-02-20
///
/// This class is manages the login/registration process for the player.
/// This is a singleton class.
/// It makes sure only one instance of the class is created.
/// </summary>
public class AccountManager : MonoBehaviour
{
    /*
    public static string PlayfabId { get; set; }
    */
    
    public static AccountManager Instance;

    /// <summary>
    /// Author: Jashanpreet Singh
    /// Date: 2020-02-20
    /// 
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {

        Instance = this;
    }

    /// <summary>
    /// Author: Jashanpreet Singh
    /// Date: 2020-10-20
    /// 
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
    /// Author: Jashanpreet Singh
    /// Date: 2020-10-20
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
                PlayerPrefsManager.SetPlayerName(Username);
                Debug.Log($"The id is  {response.PlayFabId}");
                Debug.Log($"User successfully logged in | Username: {Username}");
                Debug.Log($"The session ticket is: {response.SessionTicket}");
                IsSignedIn = true;


                // Database functions calls on login
                //UserData.SetUserData("Wins", "13");
                //UserData.GetUserData(response.PlayFabId, "Favourite Character");
                //UserData.GetUserProfileData(response.PlayFabId);
                //UserData.SendLeaderboard("MostWins", 0);
                //UserData.GetLeaderboard("MostWins");

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
/// Author: Jashanpreet Singh
/// Date: 2020-10-20
/// 
/// This class saves the PlayfabId to the PlayerPrefs.
/// </summary>
public static class PlayerPrefsManager
{
    /// <summary>
    /// Author: Jashanpreet Singh
    /// Date: 2020-10-20
    /// 
    /// Sets the PlayfabId to the PlayerPrefs.
    /// </summary>
    /// <param name="playfabId"> a string </param>
    public static void SetPlayfabId(string playfabId)
    {
        PlayerPrefs.SetString("PlayfabId", playfabId);
    }

    /// <summary>
    /// Author: Jashanpreet Singh
    /// Date: 2020-10-20
    /// 
    /// Gets the PlayfabId from the PlayerPrefs.
    /// </summary>
    /// <returns>PlayfabID -> a string </returns>
    public static string GetPlayfabId()
    {
        return PlayerPrefs.GetString("PlayfabId");
    }
    
    /// <summary>
    /// Set the player name to the PlayerPrefs.
    /// </summary>
    /// <param name="playerName"></param>
    /// <returns></returns>
    public static string SetPlayerName(string playerName)
    {
        PlayerPrefs.SetString("PlayerName", playerName);
        return playerName;
    }
    
    /// <summary>
    /// Get the player name from the PlayerPrefs.
    /// </summary>
    /// <returns></returns>
    public static string GetPlayerName()
    {
        return PlayerPrefs.GetString("PlayerName");
    }

}
