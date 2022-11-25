using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

/// <summary>
/// Database portion of the script
/// Author(s): Justin Payne, Eric Kwon
/// Date: - Nov 24 2022
/// Source(s): 
/// 	  https://www.youtube.com/watch?v=DQWYMfZyMNU&list=PL1aAeF6bPTB4oP-Tejys3n8P8iXlj7uj-&ab_channel=CocoCode
/// Remarks:
/// Change History: 10/29/2022, Eric, added the leaderboard function
/// </summary>
public static class MatchData
{

    // Stores all the user's profile data for local use
    public static Dictionary<string, string> PlayerOneInfo = new Dictionary<string, string>()
    {
        {"Wins", ""}, {"Loses", ""}, {"Total Matches", ""}, {"Player Rating", ""},
        {"Total Damage Done", ""}, {"Total Kills", ""}
    };
    
    // Stores all the user's profile data for local use
    public static Dictionary<string, string> PlayerTwoInfo = new Dictionary<string, string>(){
        {"Wins", ""}, {"Loses", ""}, {"Total Matches", ""}, {"Player Rating", ""},
        {"Total Damage Done", ""}, {"Total Kills", ""}
    };



    /// <summary>
    /// Author: Justin Payne
    /// Date: Nov 23 2022
    /// 
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
    /// Author: Justin Payne
    /// Date: Nov 23 2022
    ///
    /// Gets the user profile data for both users when the game starts. This function runs twice
    /// </summary>
    /// <param name="myPlayFabId"> string representing the Azure PlayFab Id</param>
    /// <param name="player">int representing if the fusion player is either player 1 or 2</param>
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
                PlayerOneInfo["Wins"] = result.Data["Wins"].Value;
                PlayerOneInfo["Loses"] = result.Data["Loses"].Value;
                PlayerOneInfo["Total Matches"] = result.Data["Total Matches"].Value;
                PlayerOneInfo["Player Rating"] = result.Data["Player Rating"].Value;
                PlayerOneInfo["Total Damage Done"] = result.Data["Total Damage Done"].Value;
                PlayerOneInfo["Total Kills"] = result.Data["Total Kills"].Value;
            } else if (player == 2)
            {
                Debug.Log("Get player 2 data"); 
                PlayerTwoInfo["Wins"] = result.Data["Wins"].Value;
                PlayerTwoInfo["Loses"] = result.Data["Loses"].Value;
                PlayerTwoInfo["Total Matches"] = result.Data["Total Matches"].Value;
                PlayerTwoInfo["Player Rating"] = result.Data["Player Rating"].Value;
                PlayerTwoInfo["Total Damage Done"] = result.Data["Total Damage Done"].Value;
                PlayerTwoInfo["Total Kills"] = result.Data["Total Kills"].Value;
            }

        }, (error) =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
    
    /// <summary>
    /// Author: Justin Payne
    /// Date: Nov 23 2022
    ///
    /// Sets the user profile data for both users when the game ends. This function runs twice.
    /// This function must pass an if check to match the username.
    /// </summary> 
    /// <param name="name">String representing the player Name</param>
    /// <param name="wins">String representing the player Wins</param>
    /// <param name="loses">String representing the player Loses</param>
    /// <param name="totalMatches">String representing the player Total Matches</param>
    /// <param name="playerRating">String representing the player Player Rating</param>
    /// <param name="totalKills">String representing the player Total Kills</param>
    /// <param name="totalDamage">String representing the player Total Damage Done</param>
    public static void SetPostGameData(string name, string wins, string loses, string totalMatches, string playerRating, string totalKills, string totalDamage)
    {
        if (PlayerPrefs.GetString("PlayerName") == name)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
                {
                    Data = new Dictionary<string, string>() {
                        { "Wins", wins },
                        {"Loses", loses},
                        {"Total Matches", totalMatches},
                        {"Player Rating", playerRating},
                        {"Total Kills", totalKills},
                        {"Total Damage Done", totalDamage}
                    },
                    Permission = UserDataPermission.Public
                },
                result => Debug.Log("Successfully updated user data"),
                error => {
                    Debug.Log(error.GenerateErrorReport());
                });
        }
    }



}

