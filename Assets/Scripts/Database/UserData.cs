using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

/// <summary>
/// Database portion of the script
/// Author(s): Eric Kwon
/// Date: - Oct 29 2022
/// Source(s): 
/// 	  https://www.youtube.com/watch?v=DQWYMfZyMNU&list=PL1aAeF6bPTB4oP-Tejys3n8P8iXlj7uj-&ab_channel=CocoCode
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
            //if (result.Data == null || !result.Data.ContainsKey("Info")) Debug.Log("No Info");
            //else Debug.Log("Info: " + result.Data["Info"].Value);
            Debug.Log("Database data: " + result.Data[key].Value);
        }, (error) => {
            Debug.Log(error.GenerateErrorReport());
        });
    }
    public static void SendBattleStats(int ELO, int matchesPlayed, int winRate)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName ="battleStat",
                    Value = winRate,
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

    public static void GetBattleStats(int damageTaken, int damageDealt, int winRate)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "battleStat",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, onleaderboardGet, OnError);
    }
    private static void onleaderboardGet(GetLeaderboardResult result)
    {
        Debug.Log("sent BattleStats");
    }
}


