using System.Collections;
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
/// 	  https://learn.microsoft.com/en-us/gaming/playfab/features/data/playerdata/quickstart
/// Remarks: (
/// Change History: 10/29/2022, Eric, added the leaderboard function
/// </summary>
public static class UserData
{

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

    public static void GetUserData(string myPlayFabId, string key)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result => {
            Debug.Log("Got user data:");
            Debug.Log("Database data: " + result.Data[key].Value);
        }, (error) => {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public static void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate
                    {
                        StatisticName ="MostWins",
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
        Debug.Log("Successfull leaderboard sent");
    }

    public static void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "MostWins",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, onLeaderboardGet, OnError);
    }
    private static void onLeaderboardGet(GetLeaderboardResult result)
    {
        Debug.Log("Recieved leaderboard");
        foreach (var item in result.Leaderboard) {
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }

}


