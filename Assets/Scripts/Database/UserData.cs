using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using static ProfileUI;

/// <summary>
/// Database portion of the script
/// Author(s): Justin Payne, Eric Kwon
/// Date: - Oct 29 2022
/// Source(s): 
/// 	  https://www.youtube.com/watch?v=DQWYMfZyMNU&list=PL1aAeF6bPTB4oP-Tejys3n8P8iXlj7uj-&ab_channel=CocoCode
/// Remarks: (
/// Change History: 10/29/2022, Eric, added the leaderboard function
/// </summary>
public static class UserData
{

    // Stores all the user's profile data for local use
    public static Dictionary<string, string> ProfileInfo = new Dictionary<string, string>();

    /// <summary>
    /// Sends user data to the database
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
            ProfileInfo.Add("Favourite Character", result.Data["Favourite Character"].Value);

        }, (error) =>
        {
            Debug.Log(error.GenerateErrorReport());
        });

    }


    public static void SendLeaderboard(string leaderboardName, int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = leaderboardName,
                    Value = score,
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, onleaderboardUpdate, OnError);
    }

    private static void OnError(PlayFabError obj)
    {
        throw new NotImplementedException();
    }

    private static void onleaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("sent BattleStats");
    }

    public static void GetLeaderboard(string leaderboardName)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = leaderboardName,
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, onleaderboardGet, OnError);
    }
    private static void onleaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.DisplayName+ " " + item.StatValue);
        }
    }
}


