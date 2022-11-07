
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
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
            response => { Debug.Log($"User successfully registered | " +
                                    $"Username: {Username} | Email: {Email}"); },
            error => { Debug.Log($"User registration unsuccessful | Error: {error.Error}"); }
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
                PlayerPrefsManager.SetPlayfabId(response.PlayFabId);
                Debug.Log($"The id is  {response.PlayFabId}");
                Debug.Log($"User successfully logged in | Username: {Username}");
                Debug.Log($"The session ticket is: {response.SessionTicket}");

                //Testing database functions
                UserData.GetUserData(response.PlayFabId, "Favourite Character");
            },
            error => { Debug.Log($"User login unsuccessful | Error: {error.Error}"); }
        );
    }

    /*public void SetPlayfabId(string playfabId)
    {
        AccountManager.Instance.PlayfabId = playfabId;
    }*/
}

public static class PlayerPrefsManager
{
    public static void SetPlayfabId(string playfabId)
    {
        PlayerPrefs.SetString("PlayfabId", playfabId);
    }

    public static string GetPlayfabId()
    {
        return PlayerPrefs.GetString("PlayfabId");
    }
}
