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
/// Change History:
/// Nov 24 2022 - Jason Cheung
/// - caches player username and avatar image
/// </summary>
public class NetworkFighterObserver : NetworkBehaviour
{
    // Static instance of NetworkFighterObserver so other scripts can access it
    public static NetworkFighterObserver Observer = null;

    // Game UI Components to update
    [SerializeField] private Image _playerTwoImage;
    [SerializeField] private TextMeshProUGUI _playerTwoName;
    [SerializeField] private TextMeshProUGUI _playerTwoStocks;
    [SerializeField] private TextMeshProUGUI _playerTwoCurrentHealth;
    //[SerializeField] private TextMeshProUGUI _playerTwoMaxHealth;

    [SerializeField] private Image _playerOneImage;
    [SerializeField] private TextMeshProUGUI _playerOneName;
    [SerializeField] private TextMeshProUGUI _playerOneStocks;
    [SerializeField] private TextMeshProUGUI _playerOneCurrentHealth;
    //[SerializeField] private TextMeshProUGUI _playerOneMaxHealth;

    // list of character avatar images from PlayerItem
    [SerializeField] private Sprite[] _avatars;

    // the fighter they are controlling
    private NetworkObject _playerOne;    
    private NetworkObject _playerTwo;

    private string _playerOneUsername;
    private string _playerTwoUsername;


    // previous character status values; used to compare and gather fighter stats
    private int _prevPlayerOneCurrentHealth;
    private int _prevPlayerOneStocks;
    private int _prevPlayerTwoCurrentHealth;
    private int _prevPlayerTwoStocks;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (!Observer) Observer = this;
        Debug.Log("NetworkPlayerObserver instance awake: " + Observer);

        // hide this game object
        this.gameObject.SetActive(false);
    }


    /// <summary>
    /// Method to cache the selected and spawned fighters
    /// 
    /// Change History:
    /// 2022-11-21 - Roswell Doria
    ///  - Added paramters for player one and player two usernames.
    ///  - Modifed this.playerOne and this.playerTwo Nicknames to take player usernames.
    ///
    /// </summary>
    /// <param name="playerOne"></param>
    /// <param name="playerTwo"></param>
    /// <param name="playerOneUsername">a string of player one's username</param>
    /// <param name="playerTwoUsername">a string of player two's username</param>
    /// <param name="playerOneSelectedIndex">the selected index for the avatar image</param>
    /// <param name="playerTwoSelectedIndex">the selected index for the avatar image</param>
    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CachePlayers(
        NetworkObject playerOne, NetworkObject playerTwo,
        string playerOneUsername, string playerTwoUsername,
        int playerOneSelectedIndex, int playerTwoSelectedIndex)
    {
        // assign network fighter
        this._playerOne = playerOne;
        this._playerTwo = playerTwo;

        // assign player usernames
        this._playerOneUsername = playerOneUsername;
        this._playerTwoUsername = playerTwoUsername;

        // set avatar images to ui
        _playerOneImage.sprite = _avatars[playerOneSelectedIndex];
        _playerTwoImage.sprite = _avatars[playerTwoSelectedIndex];

        // assign nicknames to their NetworkPlayer
        this._playerOne.gameObject.GetComponent<NetworkPlayer>().NickName = playerOneUsername;
        this._playerTwo.gameObject.GetComponent<NetworkPlayer>().NickName = playerTwoUsername;

        RPC_CacheFighterStatusUI(playerOneSelectedIndex, playerTwoSelectedIndex);
    }


    /// <summary>
    /// Method to initialize the fighter status ui based on the newly assigned network fighters
    /// 
    /// Change history:
    /// 2022-11-21 - Roswell Doria
    ///  - Modfied _playerOneName.text and _playerTwoName.text to grab the stored loggin username inside NetworkPlayer NickName value.
    ///
    /// </summary>
    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CacheFighterStatusUI(int playerOneSelectedIndex, int playerTwoSelectedIndex)
    {
        // set intial values for player one
        if (_playerOne) // check for null
        {
            // cache name, max health and image; doesn't change after initial cache
            _playerOneName.text = _playerOneUsername;
            //_playerOneMaxHealth.text = "/ " + playerOne.gameObject.GetComponent<Health>().CurrentHealth.ToString();
            _playerOneImage.sprite = _avatars[playerOneSelectedIndex];

            // store the initial values to the "old" ones
            _prevPlayerOneCurrentHealth = _playerOne.gameObject.GetComponent<Health>().CurrentHealth;
            _prevPlayerOneStocks = _playerOne.gameObject.GetComponent<Stock>().CurrentStocks;

            // continue caching the ui
            _playerOneCurrentHealth.text = _prevPlayerOneCurrentHealth.ToString();
            _playerOneStocks.text = _prevPlayerOneStocks.ToString();
        } else
        {
            Debug.Log("CacheFighterStatusUI Error - Player One null.");
        }

        // set initial values for player two
        if (_playerTwo)
        {
            // cache name, max health and image; doesn't change after initial cache
            _playerTwoName.text = _playerTwoUsername;
            //_playerTwoMaxHealth.text = "/ " + playerTwo.gameObject.GetComponent<Health>().CurrentHealth.ToString();
            _playerTwoImage.sprite = _avatars[playerTwoSelectedIndex];

            // store the initial values to the "old" ones
            _prevPlayerTwoCurrentHealth = _playerTwo.gameObject.GetComponent<Health>().CurrentHealth;
            _prevPlayerTwoStocks = _playerTwo.gameObject.GetComponent<Stock>().CurrentStocks;

            // continue caching the ui
            _playerTwoCurrentHealth.text = _prevPlayerTwoCurrentHealth.ToString();
            _playerTwoStocks.text = _prevPlayerTwoStocks.ToString();
        } else
        {
            Debug.Log("CacheFighterStatusUI Error - Player Two null.");
        }

        // unhide this game object
        this.gameObject.SetActive(true);

    }


    /// <summary>
    /// Method to update the fighter status ui; health and stock changes.
    /// </summary>
    public void UpdateFighterStatus()
    {
        // fighter status values to check
        int playerOneCurrentHealth = _playerOne.gameObject.GetComponent<Health>().CurrentHealth;
        int playerOneStocks = _playerOne.gameObject.GetComponent<Stock>().CurrentStocks;

        int playerTwoCurrentHealth = _playerTwo.gameObject.GetComponent<Health>().CurrentHealth;
        int playerTwoStocks = _playerTwo.gameObject.GetComponent<Stock>().CurrentStocks;

        // compare values with old stored ones
        // check player 1 health
        if (_prevPlayerOneCurrentHealth != playerOneCurrentHealth)
        {
            UpdateFighterStatusUI(_playerOneCurrentHealth, playerOneCurrentHealth);
            UpdateHealthUIColor(_playerOneCurrentHealth, playerOneCurrentHealth);
            _prevPlayerOneCurrentHealth = playerOneCurrentHealth;
        }
        // check player 1 stocks
        if (_prevPlayerOneStocks != playerOneStocks)
        {
            UpdateFighterStatusUI(_playerOneStocks, playerOneStocks);
            UpdateStocksUIColor(_playerOneStocks, playerOneStocks);
            _prevPlayerOneStocks = playerOneStocks;
        }

        // check player 2 health
        if (_prevPlayerTwoCurrentHealth != playerTwoCurrentHealth)
        {
            UpdateFighterStatusUI(_playerTwoCurrentHealth, playerTwoCurrentHealth);
            UpdateHealthUIColor(_playerTwoCurrentHealth, playerTwoCurrentHealth);
            _prevPlayerTwoCurrentHealth = playerTwoCurrentHealth;
        }
        // check player 2 stocks
        if (_prevPlayerTwoStocks != playerTwoStocks)
        {
            UpdateFighterStatusUI(_playerTwoStocks, playerTwoStocks);
            UpdateStocksUIColor(_playerTwoStocks, playerTwoStocks);
            _prevPlayerTwoStocks = playerTwoStocks;
        }

        // store the values again
        _prevPlayerOneCurrentHealth = playerOneCurrentHealth;
        _prevPlayerOneStocks = playerOneStocks;
        _prevPlayerTwoCurrentHealth = playerTwoCurrentHealth;
        _prevPlayerTwoStocks = playerTwoStocks;


    }


    /// <summary>
    /// Helper method to update the Fighter Status UI element to the updated value.
    /// </summary>
    /// <param name="textObj"></param>
    /// <param name="newValue"></param>
    private void UpdateFighterStatusUI(TextMeshProUGUI textObj, int newValue)
    {
        textObj.text = newValue.ToString();
    }

    /// <summary>
    /// Helper method to update the Health UI element to the correct color
    /// </summary>
    /// <param name="textObj"></param>
    /// <param name="newValue"></param>
    private void UpdateHealthUIColor(TextMeshProUGUI textObj, int health)
    {
        Color color;

        // set colors based on current health
        if (health <= 150) color = Color.white;
        else if (health <= 300) color = Color.yellow;
        else color = Color.red;

        textObj.color = color;
    }

    private void UpdateStocksUIColor(TextMeshProUGUI textObj, int stocks)
    {
        Color color;

        // set to red if on last stock
        if (stocks == 1) color = Color.red;
        else color = Color.white;

        textObj.color = color;
    }


}
