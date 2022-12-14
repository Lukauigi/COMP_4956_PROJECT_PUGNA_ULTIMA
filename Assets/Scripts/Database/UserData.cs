using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;


/// <summary>
/// Database portion of the script
/// Author(s): Justin Payne, Eric Kwon
/// Date: - Oct 29 2022
/// Source(s): 
/// 	  https://www.youtube.com/watch?v=DQWYMfZyMNU&list=PL1aAeF6bPTB4oP-Tejys3n8P8iXlj7uj-&ab_channel=CocoCode
/// 	  
/// Remarks: (
/// Change History: 11/21/2022, Justin, Removed unnessecary functions + Added comments
/// </summary>
public static class UserData
{

    // Stores all the user's profile data for local use
    public static Dictionary<string, string> ProfileInfo = new Dictionary<string, string>();

    /// <summary>
    /// Author: Justin Payne
    /// Date: Nov 21 2022
    /// 
    /// SetUserData sends data from the game client to the Azure Playfab database. Takes a key for the database key and value for the value of the key
    /// </summary>
    /// <param name="key">String: Key for the data field</param>
    /// <param name="value">String: Value for the data field</param>
    public static void SetUserData(string key, string value)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
            { key, value }
            }
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    /// <summary>
    /// Author: Justin Payne
    /// Date: Nov 21 2022
    /// 
    /// Gets specific user data by sending a string for a key to match a key in the database
    /// </summary>
    /// <param name="myPlayFabId">String: Users PlayFabId</param>
    /// <param name="key">String: Key to match database field</param>
    public static void GetUserData(string myPlayFabId, string key)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result =>
        {
            Debug.Log("Got user data:");
            //if (result.Data == null || !result.Data.ContainsKey("Info")) Debug.Log("No Info");
            //else Debug.Log("Info: " + result.Data["Info"].Value);
            Debug.Log("Database data: " + result.Data[key].Value);
        }, (error) =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    /// <summary>
    /// Author: Justin Payne
    /// Date: Nov 21 2022
    /// 
    /// Grabs the profile data from the database
    /// Currently this function is just called and stores the data locally to populate the game scenes
    /// Ideally this function could be turned into an async function that could be called whenever the data is needed and returns the values specified. 
    /// Not sure how to make that happen right now. 
    /// Maybe as a side fix, could just constantly call this function during navigating through pages so just player data is always up to date.
    /// </summary>
    /// <param name="myPlayFabId">String: Users PlayFabId</param>
    public static void GetUserProfileData(string myPlayFabId)
    {

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result =>
        {
            Debug.Log("Got user data:");
            ProfileInfo.Add("Wins", result.Data["Wins"].Value);
            ProfileInfo.Add("Loses", result.Data["Loses"].Value);
            ProfileInfo.Add("Total Matches", result.Data["Total Matches"].Value);
            ProfileInfo.Add("Player Rating", result.Data["Player Rating"].Value);

        }, (error) =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
    
    /// <summary>
    /// Author: Justin Payne
    /// Date: Nov 23 2022
    ///
    ///  This Function is to be called when a new account is registered. It sets initial data for all the
    ///  the data being saved to the database.
    /// </summary>
    public static void SetUserDataOnRegister()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>() {
                    {"Wins", "0"},
                    {"Loses", "0"},
                    {"Total Matches", "0"},
                    {"Player Rating", "1000"},
                    {"Total Damage Done", "0"},
                    {"Total Kills", "0"}
                },
                Permission = UserDataPermission.Public
            },
            result => Debug.Log("Successfully Set Inital user data"),
            error => {
                Debug.Log(error.GenerateErrorReport());
            });
    }

    /// <summary>
    /// Author: Justin Payne, Eric Kwon
    /// Date: Nov 23 2022
    ///
    ///  This Function sends the updated user's wins stat to the leaderboard.
    ///  It is called after the game ends and updates data for both players
    /// </summary>
    /// <param name="name"></param>
    /// <param name="score"></param>
    public static void SendLeaderboard(string name, int score)
    {
        if (PlayerPrefs.GetString("PlayerName") == name)
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate
                    {
                        StatisticName = "MostWins",
                        Value = score,
                    }
                }
            };
            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
        }
    }

    /// <summary>
    /// Author: Eric Kwon
    /// Date: Oct 29 2022
    /// 
    /// OnError function for PlayFab UpdatePlayerStatisticsRequest 
    /// </summary>
    /// <param name="obj">PlayFabError: PlayFabError object from UpdatePlayerStatisticsRequest function </param>
    private static void OnError(PlayFabError obj)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Author: Eric Kwon
    /// Date: Oct 29 2022
    /// 
    /// OnLeaderboardUpdate function for PlayFab UpdatePlayerStatisticsRequest.
    /// Defines what to do after result is returned
    /// </summary>
    /// <param name="result">PlayFabError: UpdatePlayerStatisticsResult object from UpdatePlayerStatisticsRequest function </param>
    private static void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Leaderboard Successfully Updated");
    }

}


