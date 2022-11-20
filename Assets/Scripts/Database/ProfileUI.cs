using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using static UserData;

/// <summary>
/// Populating Profile page with data from database
/// Author(s): Justin Payne
/// Date: - Nov 08 2022
/// Source(s): 
/// Remarks: (
/// Change History: 11/08/2022, Created file
/// </summary>
public class ProfileUI : MonoBehaviour
{

    [SerializeField] public TMP_Text TextWins;
    [SerializeField] public TMP_Text TextLoses;
    [SerializeField] public TMP_Text TextTotalMatchesPlayed;
    [SerializeField] public TMP_Text TextRating;
    [SerializeField] public TMP_Text TextFavoriteCharacter;

    void Start()
    {
        // Not sure how to make these calls async while returning data, so I moved the call to when the user logins for now. Hopefully I can figure out async methods for azure playfab
        TextWins.text = TextWins.text + " " + UserData.ProfileInfo["Wins"];
        TextLoses.text = TextLoses.text + " " + UserData.ProfileInfo["Loses"];
        TextTotalMatchesPlayed.text = TextTotalMatchesPlayed.text + " " + UserData.ProfileInfo["Total Matches"];
        TextRating.text = TextRating.text + " " + UserData.ProfileInfo["Player Rating"];
        TextFavoriteCharacter.text = TextFavoriteCharacter.text + " " + UserData.ProfileInfo["Favourite Character"];

    }

}
    