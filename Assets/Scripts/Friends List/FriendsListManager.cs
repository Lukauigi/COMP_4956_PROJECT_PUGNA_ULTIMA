using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.AdminModels;
using PlayFab.ClientModels;
using PlayFab.ServerModels;
using UnityEngine.SocialPlatforms.Impl;
using FriendInfo = PlayFab.ClientModels.FriendInfo;
using GetLeaderboardRequest = PlayFab.ClientModels.GetLeaderboardRequest;
using PlayerLeaderboardEntry = PlayFab.ClientModels.PlayerLeaderboardEntry;
using PlayerLocation = PlayFab.ServerModels.PlayerLocation;


namespace Friends_List
{
    public class FriendsListManager: MonoBehaviour
    {
        public static FriendsListManager Instance;
        
        public List<PlayerLeaderboardEntry> friendsList = new List<PlayerLeaderboardEntry>();
        public void Awake()
        {
            Instance = this;
        }

        public void GetAllPlayers()
        {
            PlayFabClientAPI.GetLeaderboard(
                new GetLeaderboardRequest()
                {
                    
                    StartPosition = 0,
                    StatisticName = "MostWins",
                    MaxResultsCount = 100
                },
                response =>
                {
                    friendsList = response.Leaderboard;
                    foreach (var playerLeaderboardEntry in friendsList)
                    { 
                        Debug.Log("Entered the loop");
                        Debug.Log(playerLeaderboardEntry.Profile.PlayerId);
                    }
                    Debug.Log("Got Leaderboard");
                },
                error =>
                {
                    Debug.Log("Error Getting Leaderboard");
                });
        }
    }
}