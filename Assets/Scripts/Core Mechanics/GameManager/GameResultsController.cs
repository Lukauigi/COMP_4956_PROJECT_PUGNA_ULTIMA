using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameResultsController : NetworkBehaviour
{
    // Static instance of GameManager so other scripts can access it
    public static GameResultsController Instance = null;

    protected GameManager _gameManager;

    private NetworkObject _playerOne;
    private NetworkObject _playerTwo;

    private NetworkObject _winner;
    private NetworkObject _loser;

    // winner - shown on top
    [SerializeField] private Image _winnerBGMaskImageColor;
    [SerializeField] private RawImage _winnerAvatar;
    [SerializeField] private TextMeshProUGUI _winnerName;
    // left player
    [SerializeField] private RawImage _playerTwoAvatar;
    [SerializeField] private TextMeshProUGUI _playerTwoName;
    [SerializeField] private TextMeshProUGUI _playerTwoKills;
    [SerializeField] private TextMeshProUGUI _playerTwoDamageDone;
    // right player
    [SerializeField] private RawImage _playerOneAvatar;
    [SerializeField] private TextMeshProUGUI _playerOneName;
    [SerializeField] private TextMeshProUGUI _playerOneKills;
    [SerializeField] private TextMeshProUGUI _playerOneDamageDone;

    // for results screen and/or database
    private int playerOneKills;
    private int playerOneDamageDone;

    private int playerTwoKills;
    private int playerTwoDamageDone;


    private void Awake()
    {
        Instance = this;

        // hide this initially
        gameObject.SetActive(false);
    }


    // Method to cache the selected and spawned fighters
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_CachePlayers(NetworkObject playerOne, NetworkObject playerTwo)
    {
        if (!_playerOne) _playerOne = playerOne;
        if (!_playerTwo) _playerTwo = playerTwo;
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
        }
        else if (playerOneKills < playerTwoKills)
        {
            _winner = _playerTwo;
            _loser = _playerOne;
        } else  // same amount of kills/deaths
        {
            // check their final health
            int playerOneCurrentHealth = _playerOne.gameObject.GetComponent<Health>().CurrentHealth;
            int playerTwoCurrentHealth = _playerTwo.gameObject.GetComponent<Health>().CurrentHealth;

            if (playerOneCurrentHealth >= playerTwoCurrentHealth) // TODO: reverse this condition if knockback & damage done% is implemented
            {
                _winner = _playerOne;
                _loser = _playerTwo;
            }
            else
            {
                _winner = _playerTwo;
                _loser = _playerOne;
            }
        }

    }

    private void GetStats()
    {
        // get damage done
        playerOneDamageDone = _playerOne.gameObject.GetComponent<Attack>().DamageDone;
        playerTwoDamageDone = _playerTwo.gameObject.GetComponent<Attack>().DamageDone;

        // get the other player's deaths to get your individual amount of kills
        playerOneKills = _playerTwo.gameObject.GetComponent<Stock>().Deaths;
        playerTwoKills = _playerOne.gameObject.GetComponent<Stock>().Deaths;

    }

    private void SetResultsScreen()
    {
        // TODO set winner & players' raw image
        // TODO set winner's mask color -> _winnerBGMaskImageColor

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

    // TODO save results to database
    private void SaveToDatabase()
    {
        // relevant member variables:
        // - NetworkObject _winner
        // - NetworkObject _loser
        // - NetworkObject _playerOne
        // - NetworkObject _playerTwo
        // - int playerOneKills
        // - int playerTwoKills
        // - int playerOneDamageDone
        // - int playerTwoDamageDone
    }

    // Method to reference the players and find the winner/loser
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_ShowGameResults()
    {
        //Debug.Log(_winner.gameObject.GetComponent<NetworkPlayer>().NickName + " won!");
        //Debug.Log(_loser.gameObject.GetComponent<NetworkPlayer>().NickName + " lost...");

        // show the game object
        gameObject.SetActive(true);
    }

    public void OnMainMenuBtnClick()
    {
        // load next scene: return to main menu
        Debug.Log("returning to Main Menu...");
        Runner.Shutdown();
        SceneManager.LoadScene("Main Menu");
    }
}
