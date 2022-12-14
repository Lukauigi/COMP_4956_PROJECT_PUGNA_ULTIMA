
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
/// 
/// Change History:
/// 2022-11-25 - Xiang Zhu
/// - Change the navigation screen to the most updated one
/// 2022-11-26 - Lukasz Bednarek
/// - Add audio manager method calls.
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
        AudioEffectsManager.Instance2.PlayLoopingSoundClip(MenuActions.Waiting);
        var IsRegistered = false;
        print("username: " + Username);
        print("email: " + Email);
        print("password: " + Password);
        PlayFabClientAPI.RegisterPlayFabUser(
            new RegisterPlayFabUserRequest()
            {
                Email = Email,
                Password = Password,
                Username = Username,
                DisplayName = Username,
                RequireBothUsernameAndEmail = true
            },
            response => { 
                Debug.Log($"User successfully registered | " +
                                    $"Username: {Username} | Email: {Email}"); 
                
                // Database call to set all initial data in database
                SetUserDataOnRegister();
                
                IsRegistered = true;
                AudioEffectsManager.Instance2.StopLoopingSoundClip();
                AudioEffectsManager.Instance2.PlaySoundClipOnce(MenuActions.Confirm);
                SceneManager.LoadScene("Scenes/Game Design/Screen Navigation/jr/Login Page");
            },
            error =>
            {
                Debug.Log($"User registration unsuccessful | Error: {error.Error}"); 
                IsRegistered = false;
                AudioEffectsManager.Instance2.StopLoopingSoundClip();
                AudioEffectsManager.Instance2.PlaySoundClipOnce(MenuActions.Error);
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
        AudioEffectsManager.Instance2.PlayLoopingSoundClip(MenuActions.Waiting);
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
                GetUserProfileData(response.PlayFabId);
                //SetUserData("Wins", "13");
                //SendLeaderboard("MostWins", 10);

                AudioEffectsManager.Instance2.StopLoopingSoundClip();
                AudioEffectsManager.Instance2.PlaySoundClipOnce(MenuActions.Login);
                SceneManager.LoadScene("Scenes/Game Design/Screen Navigation/jr/Main Menu");
            },
            error =>
            {
                // On failed login, log the error
                AudioEffectsManager.Instance2.StopLoopingSoundClip();
                AudioEffectsManager.Instance2.PlaySoundClipOnce(MenuActions.Error);
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
