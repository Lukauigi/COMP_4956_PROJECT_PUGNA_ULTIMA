using System;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;


/// <summary>
/// Script for getting leaderboard + generating rows dynamically
/// Author(s): Justin Payne,
/// Date: - Nov 21 2022
/// Source(s): 
/// 	  https://www.youtube.com/watch?v=DQWYMfZyMNU&list=PL1aAeF6bPTB4oP-Tejys3n8P8iXlj7uj-&ab_channel=CocoCode
/// 	  https://www.youtube.com/watch?v=jlZYr9Hbmys
/// Remarks: (
/// Change History: 11/21/2022, Justin, Created Script
/// </summary>
public class GetLeaderboard : MonoBehaviour
{
    public GameObject rowPrefab;
    public Transform rowsParent;

    /// <summary>
    /// Author: Justin Payne
    /// Date: Nov 21 2022
    /// 
    /// Onclick is called when you change the profile tab from user stats to leaderboard.
    /// </summary>
    public void OnClick()
    {
        GetLeaderboards("MostWins");
    }

    /// <summary>
    /// Author: Justin Payne
    /// Date: Nov 21 2022
    /// 
    /// GetLeaderboard is the API call to the PlayFab database to get the requested leaderboard data, takes a string for the name of the leaderboard
    /// </summary>
    /// <param name="leaderboardName"> a string </param>
    private void GetLeaderboards(string leaderboardName)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = leaderboardName,
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    /// <summary>
    /// Author: Justin Payne
    /// Date: Nov 21 2022
    /// 
    /// onleaderboardGet function is passed to the API call request with information on what to do when the object returns from Azure PlayFab.
    /// </summary>
    /// <param name="result"> GetLeaderboardResult result from api call</param>
    private void OnLeaderboardGet(GetLeaderboardResult result)
    {

        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            TMP_Text[] texts = newGo.GetComponentsInChildren<TMP_Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            //texts[1].text = item.PlayFabId;
            texts[2].text = item.StatValue.ToString();
        }
    }

    /// <summary>
    /// Author: Justin Payne
    /// Date: Nov 21 2022
    /// 
    /// OnError function is passed to the API call request with information on what to do when an error is found.
    /// </summary>
    /// <param name="obj"> PlayFabError object </param>
    private void OnError(PlayFabError obj)
    {
        throw new NotImplementedException();
    }

}
