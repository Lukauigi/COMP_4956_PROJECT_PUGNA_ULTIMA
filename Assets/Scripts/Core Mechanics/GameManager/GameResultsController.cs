using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    private int playerOneSelectedIndex;
    private int playerTwoSelectedIndex;
    private int winnerSelectedIndex;

    // stored values for results screen and/or database
    private int playerOneKills;
    private int playerOneDamageDone;

    private int playerTwoKills;
    private int playerTwoDamageDone;


    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Instance = this;

        // hide the game results screen initially
        gameObject.SetActive(false);
    }


    // Method to cache the selected and spawned fighters
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_CachePlayers(NetworkObject playerOne, NetworkObject playerTwo,
        int playerOneSelectedIndex, int playerTwoSelectedIndex)
    {
        if (!_playerOne) _playerOne = playerOne;
        if (!_playerTwo) _playerTwo = playerTwo;

        this.playerOneSelectedIndex = playerOneSelectedIndex;
        this.playerTwoSelectedIndex = playerTwoSelectedIndex;
    }


    // Method to reference the players and find the winner/loser
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_CacheGameResults()
    {
        GetStats();
        GetWinnerLoser();
        SetResultsScreen();
        SaveToDatabase();   // TODO
    }

    // Helper method to determine the winner/loser
    private void GetWinnerLoser()
    {
        // check who has more kills
        if (playerOneKills > playerTwoKills)
        {
            _winner = _playerOne;
            _loser = _playerTwo;
            winnerSelectedIndex = playerOneSelectedIndex;   // cache selected image index
        }
        else if (playerOneKills < playerTwoKills)
        {
            _winner = _playerTwo;
            _loser = _playerOne;
            winnerSelectedIndex = playerTwoSelectedIndex;
        } else  // same amount of kills/deaths
        {
            // check their final health; whoever has less health wins
            int playerOneCurrentHealth = _playerOne.gameObject.GetComponent<Health>().CurrentHealth;
            int playerTwoCurrentHealth = _playerTwo.gameObject.GetComponent<Health>().CurrentHealth;

            if (playerOneCurrentHealth <= playerTwoCurrentHealth)
            {
                _winner = _playerOne;
                _loser = _playerTwo;
                winnerSelectedIndex = playerOneSelectedIndex;
            }
            else
            {
                _winner = _playerTwo;
                _loser = _playerOne;
                winnerSelectedIndex = playerTwoSelectedIndex;
            }
        }

    }

    // Helper method to get player stats from this game
    private void GetStats()
    {
        // get damage done
        playerOneDamageDone = _playerOne.gameObject.GetComponent<Attack>().DamageDone;
        playerTwoDamageDone = _playerTwo.gameObject.GetComponent<Attack>().DamageDone;

        // get the other player's deaths to get your individual amount of kills
        playerOneKills = _playerTwo.gameObject.GetComponent<Stock>().Deaths;
        playerTwoKills = _playerOne.gameObject.GetComponent<Stock>().Deaths;

    }

    // Helper method to set the text values of the game results screen
    private void SetResultsScreen()
    {
        // set winner & players' image
        _winnerImage.sprite = _avatars[winnerSelectedIndex];
        _playerOneImage.sprite = _avatars[playerOneSelectedIndex];
        _playerTwoImage.sprite = _avatars[playerTwoSelectedIndex];

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

    // Helper method to save player stats to database
    private void SaveToDatabase()
    {
        // TODO save results to database
        // relevant member variables:
        // - NetworkObject _winner
        // - NetworkObject _loser
        // - NetworkObject _playerOne
        // - NetworkObject _playerTwo
        // - int playerOneKills
        // - int playerTwoKills
        // - int playerOneDamageDone
        // - int playerTwoDamageDone

        // use _winner and _loser to find user data by playfabid
        // do something to update user data 
    }

    // Method to unhide/show the game results scene object
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_ShowGameResults()
    {
        // show the game object
        gameObject.SetActive(true);
    }

    // OnClick event method for the 'Main Menu' button
    public void OnMainMenuBtnClick()
    {
        // load next scene: return to main menu
        Debug.Log("returning to Main Menu...");
        Runner.Shutdown();
        SceneManager.LoadScene("Main Menu");
    }
}
