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
        FindWinnerLoser();
    }

    // Helper method to determine the winner/loser
    private void FindWinnerLoser()
    {
        // check for null (despawned) players
        if (!_playerOne)
        {
            Debug.Log("Player 1 (client) is null. player 1 auto-loses");
            _winner = _playerTwo;
            _loser = _playerOne;
        }
        if (!_playerTwo)
        {
            Debug.Log("Player 2 (host) is null. player 2 auto-loses");
            _winner = _playerOne;
            _loser = _playerTwo;
        }

        // find the winner and loser
        int playerOneStocks = _playerOne.gameObject.GetComponent<Stock>().Stocks;
        int playerTwoStocks = _playerTwo.gameObject.GetComponent<Stock>().Stocks;

        // higher stocks wins
        if (playerOneStocks > playerTwoStocks)
        {
            _winner = _playerOne;
            _loser = _playerTwo;
        }
        else if (playerOneStocks < playerTwoStocks)
        {
            _winner = _playerTwo;
            _loser = _playerOne;
        }
        else  //(playerOneStocks == playerTwoStocks)
        {
            // stocks are equal, check current health to find winner
            int playerOneHealth = _playerOne.gameObject.GetComponent<Health>().CurrentHealth;
            int playerTwoHealth = _playerTwo.gameObject.GetComponent<Health>().CurrentHealth;

            // higher health wins
            // TODO: reverse this if we want lower health wins (working knockback implemented & no max health)
            if (playerOneHealth >= playerTwoHealth)
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

    // Method to reference the players and find the winner/loser
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_ShowGameResults()
    {
        Debug.Log(_winner.gameObject.GetComponent<NetworkPlayer>().NickName + " won!");
        Debug.Log(_loser.gameObject.GetComponent<NetworkPlayer>().NickName + " lost...");

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
