using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

/// <summary>
/// Static class that displays the results screen at the end of a game.
/// Shows the winner, kills, and damage done.
/// Author(s): Jason Cheung
/// Date: Nov 20 2022
/// Change History:
/// Nov 24 2022 - Jason Cheung
/// - caches player username and avatar image
/// </summary>
public class GameResultsController : NetworkBehaviour
{
    // Static instance of GameManager so other scripts can access it
    public static GameResultsController Instance = null;

    // the fighter they are controlling
    private NetworkObject _playerOne;
    private NetworkObject _playerTwo;

    // the winner and loser when the game ends
    private NetworkObject _winner;
    private NetworkObject _loser;

    // TextMeshPro UI elements to update
    // winner
    [SerializeField] private Image _winnerBGMaskImageColor;
    [SerializeField] private Image _winnerImage;
    [SerializeField] private TextMeshProUGUI _winnerName;
    // left player results
    [SerializeField] private Image _playerTwoImage;
    [SerializeField] private TextMeshProUGUI _playerTwoName;
    [SerializeField] private TextMeshProUGUI _playerTwoKills;
    [SerializeField] private TextMeshProUGUI _playerTwoDamageDone;
    // right player results
    [SerializeField] private Image _playerOneImage;
    [SerializeField] private TextMeshProUGUI _playerOneName;
    [SerializeField] private TextMeshProUGUI _playerOneKills;
    [SerializeField] private TextMeshProUGUI _playerOneDamageDone;

    // list of character avatar images from PlayerItem
    [SerializeField] private Sprite[] _avatars;

    // their selected index from the list of avatars
    private int _playerOneSelectedIndex;
    private int _playerTwoSelectedIndex;
    private int _winnerSelectedIndex;

    // stored values for results screen and/or database
    private int playerOneKills;
    private int playerOneDamageDone;

    private int playerTwoKills;
    private int playerTwoDamageDone;

    // Database variables for post match POST requests
    private bool _savedToDB = false;
    private string _playerOneId;
    private string _playerTwoId;
    private string _DatabasePlayerOneName;
    private string _DatabasePlayerTwoName;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Instance = this;

        // hide the game results screen initially
        gameObject.SetActive(false);
    }


    /// <summary>
    /// Method to Cache the selected and spawned fighters.
    /// </summary>
    /// <param name="playerOne"></param>
    /// <param name="playerTwo"></param>
    /// <param name="playerOneSelectedIndex"></param>
    /// <param name="playerTwoSelectedIndex"></param>
    /// <param name="playerOneId"></param>
    /// <param name="playerTwoId"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_CachePlayers(NetworkObject playerOne, NetworkObject playerTwo,
        int playerOneSelectedIndex, int playerTwoSelectedIndex, string playerOneId, string playerTwoId)
    {
        if (!_playerOne) _playerOne = playerOne;
        if (!_playerTwo) _playerTwo = playerTwo;
        
        // Database calls for player data
        _playerOneId = playerOneId;
        _playerTwoId = playerTwoId;
        MatchData.GetGameProfileData(_playerOneId, 1);
        MatchData.GetGameProfileData(_playerTwoId, 2);
        
        this._playerOneSelectedIndex = playerOneSelectedIndex;
        this._playerTwoSelectedIndex = playerTwoSelectedIndex;
    }


    /// <summary>
    /// Method to run the behaviour to gather end game results.
    /// </summary>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_CacheGameResults()
    {
        GetStats();
        GetWinnerLoser();
        SetResultsScreen();
        SaveToDatabase();
    }

    /// <summary>
    /// Helper method to determine the winner/loser.
    /// </summary>
    private void GetWinnerLoser()
    {
        // check who has more kills
        if (playerOneKills > playerTwoKills)
        {
            _winner = _playerOne;
            _loser = _playerTwo;
            _winnerSelectedIndex = _playerOneSelectedIndex;   // cache selected image index
        }
        else if (playerOneKills < playerTwoKills)
        {
            _winner = _playerTwo;
            _loser = _playerOne;
            _winnerSelectedIndex = _playerTwoSelectedIndex;
        } else  // same amount of kills/deaths
        {
            // check their final health; whoever has less health wins
            int playerOneCurrentHealth = _playerOne.gameObject.GetComponent<Health>().CurrentHealth;
            int playerTwoCurrentHealth = _playerTwo.gameObject.GetComponent<Health>().CurrentHealth;

            if (playerOneCurrentHealth <= playerTwoCurrentHealth)
            {
                _winner = _playerOne;
                _loser = _playerTwo;
                _winnerSelectedIndex = _playerOneSelectedIndex;
            }
            else
            {
                _winner = _playerTwo;
                _loser = _playerOne;
                _winnerSelectedIndex = _playerTwoSelectedIndex;
            }
        }

    }

    /// <summary>
    /// Helper method to get player stats from the game.
    /// </summary>
    private void GetStats()
    {
        // get damage done
        playerOneDamageDone = _playerOne.gameObject.GetComponent<Attack>().DamageDone;
        playerTwoDamageDone = _playerTwo.gameObject.GetComponent<Attack>().DamageDone;

        // get the other player's deaths to get your individual amount of kills
        playerOneKills = _playerTwo.gameObject.GetComponent<Stock>().Deaths;
        playerTwoKills = _playerOne.gameObject.GetComponent<Stock>().Deaths;

    }

    /// <summary>
    /// Helper method to set the text values of the game results screen.
    /// </summary>
    private void SetResultsScreen()
    {
        // set winner & players' image
        _winnerImage.sprite = _avatars[_winnerSelectedIndex];
        _playerOneImage.sprite = _avatars[_playerOneSelectedIndex];
        _playerTwoImage.sprite = _avatars[_playerTwoSelectedIndex];

        // TODO ? set winner's mask color -> _winnerBGMaskImageColor

        // set winner & players' names
        _winnerName.text = _winner.gameObject.GetComponent<NetworkPlayer>().NickName.ToString();
        _playerOneName.text = _playerOne.gameObject.GetComponent<NetworkPlayer>().NickName.ToString();
        _playerTwoName.text = _playerTwo.gameObject.GetComponent<NetworkPlayer>().NickName.ToString();

        // set player kills
        _playerOneKills.text = playerOneKills.ToString();
        _playerTwoKills.text = playerTwoKills.ToString();

        // set player damage done
        _playerOneDamageDone.text = playerOneDamageDone.ToString();
        _playerTwoDamageDone.text = playerTwoDamageDone.ToString();

    }

    /// <summary>
    /// Function called at end of game to handle sending post game data to database.
    /// Also handles updating leaderboard results
    /// 
    /// Author: Justin Payne
    /// Date: Nov 23 2022
    /// </summary>
    private void SaveToDatabase()
    {

        if (!_savedToDB)
        {
            // Sets savedToDb boolean to true so SaveToDatabase() command runs once.
            _savedToDB = true;
            
            // Saves player names to variables for single player game api call checks.
            _DatabasePlayerOneName = _playerOne.gameObject.GetComponent<NetworkPlayer>().NickName.ToString();
            _DatabasePlayerTwoName = _playerTwo.gameObject.GetComponent<NetworkPlayer>().NickName.ToString();
            
            // All player data that needs to be updated for each player
            // Player 1
            int DatabasePlayerOneWins = Int32.Parse(MatchData.PlayerOneInfo["Wins"]);
            int DatabasePlayerOneLoses = Int32.Parse(MatchData.PlayerOneInfo["Loses"]);
            int DatabasePlayerOneTotalMatches = Int32.Parse(MatchData.PlayerOneInfo["Total Matches"]);
            int DatabasePlayerOnePlayerRating = Int32.Parse(MatchData.PlayerOneInfo["Player Rating"]);
            int DatabasePlayerOneTotalDamageDone = Int32.Parse(MatchData.PlayerOneInfo["Total Damage Done"]);
            int DatabasePlayerOneTotalKills = Int32.Parse(MatchData.PlayerOneInfo["Total Kills"]);
            
            
            //Player 2
            int DatabasePlayerTwoWins = Int32.Parse(MatchData.PlayerTwoInfo["Wins"]);
            int DatabasePlayerTwoLoses = Int32.Parse(MatchData.PlayerTwoInfo["Loses"]);
            int DatabasePlayerTwoTotalMatches = Int32.Parse(MatchData.PlayerTwoInfo["Total Matches"]);
            int DatabasePlayerTwoPlayerRating = Int32.Parse(MatchData.PlayerTwoInfo["Player Rating"]);
            int DatabasePlayerTwoTotalDamageDone = Int32.Parse(MatchData.PlayerTwoInfo["Total Damage Done"]);
            int DatabasePlayerTwoTotalKills = Int32.Parse(MatchData.PlayerTwoInfo["Total Kills"]);
            
            // Set data for which player wins
            if (_winner == _playerOne)
            {
                DatabasePlayerOneWins = DatabasePlayerOneWins + 1;
                DatabasePlayerTwoLoses = DatabasePlayerTwoLoses + 1;
                
                // Use some kind of algorithm to determine player rating gains/losses after match
                DatabasePlayerOnePlayerRating = DatabasePlayerOnePlayerRating + 20;
                DatabasePlayerTwoPlayerRating = DatabasePlayerTwoPlayerRating - 20;
            }
            else
            {
                DatabasePlayerOneLoses = DatabasePlayerOneLoses + 1;
                DatabasePlayerTwoWins = DatabasePlayerTwoWins + 1;
                DatabasePlayerOnePlayerRating = DatabasePlayerOnePlayerRating - 20;
                DatabasePlayerTwoPlayerRating = DatabasePlayerTwoPlayerRating + 20;
            }
            // Set all other data not dependant on who won
            DatabasePlayerOneTotalMatches = DatabasePlayerOneTotalMatches + 1;
            DatabasePlayerTwoTotalMatches = DatabasePlayerTwoTotalMatches + 1;
            DatabasePlayerOneTotalDamageDone = DatabasePlayerOneTotalDamageDone + playerOneDamageDone;
            DatabasePlayerTwoTotalDamageDone = DatabasePlayerTwoTotalDamageDone + playerTwoDamageDone;
            DatabasePlayerOneTotalKills = DatabasePlayerOneTotalKills + playerOneKills;
            DatabasePlayerTwoTotalKills = DatabasePlayerTwoTotalKills + playerTwoKills;
            
            
            // POST requests to update data post game for each player
            Debug.Log("Update Data for players");
            MatchData.SetPostGameData( _DatabasePlayerOneName, DatabasePlayerOneWins.ToString(), DatabasePlayerOneLoses.ToString(), DatabasePlayerOneTotalMatches.ToString(),
                DatabasePlayerOnePlayerRating.ToString(), DatabasePlayerOneTotalKills.ToString(), DatabasePlayerOneTotalDamageDone.ToString());
            MatchData.SetPostGameData(_DatabasePlayerTwoName, DatabasePlayerTwoWins.ToString(), DatabasePlayerTwoLoses.ToString(), DatabasePlayerTwoTotalMatches.ToString(),
                DatabasePlayerTwoPlayerRating.ToString(), DatabasePlayerTwoTotalKills.ToString(), DatabasePlayerTwoTotalDamageDone.ToString());
            
            // Leaderboard code
            UserData.SendLeaderboard(_DatabasePlayerOneName, DatabasePlayerOneWins);
            UserData.SendLeaderboard(_DatabasePlayerTwoName, DatabasePlayerTwoWins);
        }

        // TODO save results to database
        // Add leaderboard calls here
        // use _winner and _loser to find user data by playfabid
        // do something to update user data 
    }


    /// <summary>
    /// Method to unhide/show the game results scene object.
    /// </summary>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_ShowGameResults()
    {
        // show the game object
        gameObject.SetActive(true);
    }


    /// <summary>
    /// OnClick event method for the 'Main Menu' button.
    /// This also shuts down the Fusion Runner for the local instance.
    /// </summary>
    public void OnMainMenuBtnClick()
    {
        // load next scene: return to main menu
        Debug.Log("returning to Main Menu...");
        Runner.Shutdown();
        SceneManager.LoadScene("Scenes/Game Design/Screen Navigation/jr/Main Menu");
    }
}
