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
public static class MatchData
{

    // Stores all the user's profile data for local use
    public static Dictionary<string, string> PlayerOneInfo = new Dictionary<string, string>();
    
    // Stores all the user's profile data for local use
    public static Dictionary<string, string> PlayerTwoInfo = new Dictionary<string, string>();

    public static string PlayerOneName;

    public static string PlayerTwoName;
    
    

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
    
    
    
    public static void GetGameProfileData(string myPlayFabId, int player)
    {

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result =>
        {
            if (player == 1)
            {
                Debug.Log("Get player 1 data");
                PlayerOneInfo.Add("Wins", result.Data["Wins"].Value);
                PlayerOneInfo.Add("Loses", result.Data["Loses"].Value);
                PlayerOneInfo.Add("Total Matches", result.Data["Total Matches"].Value);
                PlayerOneInfo.Add("Player Rating", result.Data["Player Rating"].Value);
                PlayerOneInfo.Add("Total Damage Done", result.Data["Total Damage Done"].Value);
                PlayerOneInfo.Add("Total Kills", result.Data["Total Kills"].Value);
            } else if (player == 2)
            {
                Debug.Log("Get player 2 data"); 
                PlayerTwoInfo.Add("Wins", result.Data["Wins"].Value);
                PlayerTwoInfo.Add("Loses", result.Data["Loses"].Value);
                PlayerTwoInfo.Add("Total Matches", result.Data["Total Matches"].Value);
                PlayerTwoInfo.Add("Player Rating", result.Data["Player Rating"].Value);
                PlayerTwoInfo.Add("Total Damage Done", result.Data["Total Damage Done"].Value);
                PlayerTwoInfo.Add("Total Kills", result.Data["Total Kills"].Value);
            }

        }, (error) =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
    
    public static void SetPostGameData(string name, string wins, string loses, string totalMatches)
    {
        if (PlayerPrefs.GetString("PlayerName") == name)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
                {
                    Data = new Dictionary<string, string>() {
                        { "Wins", wins },
                        {"Loses", loses},
                        {"Total Matches", totalMatches}
                    }
                },
                result => Debug.Log("Successfully updated user data"),
                error => {
                    Debug.Log(error.GenerateErrorReport());
                });
        }
    }



}

