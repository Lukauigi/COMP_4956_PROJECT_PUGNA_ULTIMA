using Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Static Class to observe the two spawned fighter's character status.
/// Waits for changes to their name / health / stocks and updates the game ui.
/// Author(s): Jason Cheung
/// Date: Nov 19 2022
/// Remarks:
/// - CurrentHealth and Stocks are network properties that will notify this observer to update itself.
/// </summary>
public class NetworkFighterObserver : NetworkBehaviour
{
    // Static instance of NetworkFighterObserver so other scripts can access it
    public static NetworkFighterObserver Observer = null;

    // other scene objects to reference
    protected GameManager _gameManager;

    // Game UI Components to update
    [SerializeField] private RawImage _playerTwoAvatar;
    [SerializeField] private TextMeshProUGUI _playerTwoName;
    [SerializeField] private TextMeshProUGUI _playerTwoStocks;
    [SerializeField] private TextMeshProUGUI _playerTwoCurrentHealth;
    [SerializeField] private TextMeshProUGUI _playerTwoMaxHealth;

    [SerializeField] private RawImage _playerOneAvatar;
    [SerializeField] private TextMeshProUGUI _playerOneName;
    [SerializeField] private TextMeshProUGUI _playerOneStocks;
    [SerializeField] private TextMeshProUGUI _playerOneCurrentHealth;
    [SerializeField] private TextMeshProUGUI _playerOneMaxHealth;


    // the fighter they are controlling
    private NetworkObject playerOne;    
    private NetworkObject playerTwo;

    // player references - fusion gives them a player id
    private int playerOneRef = 0;
    private int playerTwoRef = 0;


    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        if (!Observer) Observer = this;
        Debug.Log("NetworkPlayerObserver instance awake: " + Observer);

        // hide this game object
        this.gameObject.SetActive(false);
    }


    // Start is called after Awake, and before Update
    public void Start()
    {
        CacheOtherObjects();
    }

    // Helper method to initialize OTHER game objects and their components
    private void CacheOtherObjects()
    {
        if (!_gameManager) _gameManager = GameManager.Manager;
    }

    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SetNetworkFighters(int playerOneRef, NetworkObject playerOne, int playerTwoRef, NetworkObject playerTwo)
    {
        // assign player ref
        this.playerOneRef = playerOneRef;
        this.playerTwoRef = playerTwoRef;

        // TODO - assign selected images

        // assign network fighter
        this.playerOne = playerOne;
        this.playerTwo = playerTwo;

        // assign player nickname TODO - change to actual username
        this.playerOne.gameObject.GetComponent<NetworkPlayer>().NickName = "Player " + playerOneRef.ToString();
        this.playerTwo.gameObject.GetComponent<NetworkPlayer>().NickName = "Player " + playerTwoRef.ToString();

        RPC_CacheFighterStatusUI();
    }

    // Method to initialize the fighter status ui based on the newly assigned network fighters
    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CacheFighterStatusUI()
    {
        // TODO - set selected images

        // set intial values
        if (playerOne) // check for null
        {
            _playerOneName.text = "Player " + playerOneRef.ToString();
            Debug.Log("updated player name text: " + _playerOneName.text);
            _playerOneMaxHealth.text = "/ " + playerOne.gameObject.GetComponent<Health>().CurrentHealth.ToString();
            _playerOneCurrentHealth.text = playerOne.gameObject.GetComponent<Health>().CurrentHealth.ToString();
            _playerOneStocks.text = playerOne.gameObject.GetComponent<Stock>().Stocks.ToString();
        } else
        {
            Debug.Log("CacheFighterStatusUI Error - Player One null.");
        }

        if (playerTwo)
        {
            _playerTwoName.text = "Player " + playerTwoRef.ToString();
            Debug.Log("updated player name text: " + _playerTwoName.text);
            _playerTwoMaxHealth.text = "/ " + playerTwo.gameObject.GetComponent<Health>().CurrentHealth.ToString();
            _playerTwoCurrentHealth.text = playerTwo.gameObject.GetComponent<Health>().CurrentHealth.ToString();
            _playerTwoStocks.text = playerTwo.gameObject.GetComponent<Stock>().Stocks.ToString();
        } else
        {
            Debug.Log("CacheFighterStatusUI Error - Player Two null.");
        }

        // unhide this game object
        this.gameObject.SetActive(true);

    }


    // Method to update the fighter status ui; health and stock changes
    public void UpdateFighterStatusUI()
    {
        if (playerOne) // update if not null
        {
            _playerOneCurrentHealth.text = playerOne.gameObject.GetComponent<Health>().CurrentHealth.ToString();
            _playerOneStocks.text = playerOne.gameObject.GetComponent<Stock>().Stocks.ToString();
        }

        if (playerTwo)
        {
            _playerTwoCurrentHealth.text = playerTwo.gameObject.GetComponent<Health>().CurrentHealth.ToString();
            _playerTwoStocks.text = playerTwo.gameObject.GetComponent<Stock>().Stocks.ToString();
        }

    }


}
