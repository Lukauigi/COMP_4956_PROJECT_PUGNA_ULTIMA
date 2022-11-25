using TMPro;
using UnityEngine;

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
    // Text Field Variables related to displaying data on profile page
    [SerializeField] public TMP_Text TextWins;
    [SerializeField] public TMP_Text TextLoses;
    [SerializeField] public TMP_Text TextTotalMatchesPlayed;
    [SerializeField] public TMP_Text TextRating;

    /// <summary>
    /// Author: Justin Payne
    /// Date: Nov 2022
    /// 
    /// Start Function, sets all the profile page data when page is loaded.
    /// </summary>
    void Start()
    {
        TextWins.text = TextWins.text + " " + UserData.ProfileInfo["Wins"];
        TextLoses.text = TextLoses.text + " " + UserData.ProfileInfo["Loses"];
        TextTotalMatchesPlayed.text = TextTotalMatchesPlayed.text + " " + UserData.ProfileInfo["Total Matches"];
        TextRating.text = TextRating.text + " " + UserData.ProfileInfo["Player Rating"];
    }

}
    