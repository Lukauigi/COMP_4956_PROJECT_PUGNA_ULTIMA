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
    private NetworkObject playerOne;    
    private NetworkObject playerTwo;

    private string playerOneUsername;
    private string playerTwoUsername;


    // previous character status values; used to compare and gather fighter stats
    private int prevPlayerOneCurrentHealth;
    private int prevPlayerOneStocks;
    private int prevPlayerTwoCurrentHealth;
    private int prevPlayerTwoStocks;


    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        if (!Observer) Observer = this;
        Debug.Log("NetworkPlayerObserver instance awake: " + Observer);

        // hide this game object
        this.gameObject.SetActive(false);
    }


    // Method to cache the selected and spawned fighters
    /// <summary>
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
        this.playerOne = playerOne;
        this.playerTwo = playerTwo;

        // assign player usernames
        this.playerOneUsername = playerOneUsername;
        this.playerTwoUsername = playerTwoUsername;

        // set avatar images to ui
        _playerOneImage.sprite = _avatars[playerOneSelectedIndex];
        _playerTwoImage.sprite = _avatars[playerTwoSelectedIndex];

        // assign nicknames to their NetworkPlayer
        this.playerOne.gameObject.GetComponent<NetworkPlayer>().NickName = playerOneUsername;
        this.playerTwo.gameObject.GetComponent<NetworkPlayer>().NickName = playerTwoUsername;

        RPC_CacheFighterStatusUI(playerOneSelectedIndex, playerTwoSelectedIndex);
    }

    // Method to initialize the fighter status ui based on the newly assigned network fighters
    /// <summary>
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
        if (playerOne) // check for null
        {
            // cache name, max health and image; doesn't change after initial cache
            _playerOneName.text = playerOneUsername;
            //_playerOneMaxHealth.text = "/ " + playerOne.gameObject.GetComponent<Health>().CurrentHealth.ToString();
            _playerOneImage.sprite = _avatars[playerOneSelectedIndex];

            // store the initial values to the "old" ones
            prevPlayerOneCurrentHealth = playerOne.gameObject.GetComponent<Health>().CurrentHealth;
            prevPlayerOneStocks = playerOne.gameObject.GetComponent<Stock>().Stocks;

            // continue caching the ui
            _playerOneCurrentHealth.text = prevPlayerOneCurrentHealth.ToString();
            _playerOneStocks.text = prevPlayerOneStocks.ToString();
        } else
        {
            Debug.Log("CacheFighterStatusUI Error - Player One null.");
        }

        // set initial values for player two
        if (playerTwo)
        {
            // cache name, max health and image; doesn't change after initial cache
            _playerTwoName.text = playerTwoUsername;
            //_playerTwoMaxHealth.text = "/ " + playerTwo.gameObject.GetComponent<Health>().CurrentHealth.ToString();
            _playerTwoImage.sprite = _avatars[playerTwoSelectedIndex];

            // store the initial values to the "old" ones
            prevPlayerTwoCurrentHealth = playerTwo.gameObject.GetComponent<Health>().CurrentHealth;
            prevPlayerTwoStocks = playerTwo.gameObject.GetComponent<Stock>().Stocks;

            // continue caching the ui
            _playerTwoCurrentHealth.text = prevPlayerTwoCurrentHealth.ToString();
            _playerTwoStocks.text = prevPlayerTwoStocks.ToString();
        } else
        {
            Debug.Log("CacheFighterStatusUI Error - Player Two null.");
        }

        // unhide this game object
        this.gameObject.SetActive(true);

    }


    // Method to update the fighter status ui; health and stock changes
    public void UpdateFighterStatus()
    {
        // fighter status values to check
        int playerOneCurrentHealth = playerOne.gameObject.GetComponent<Health>().CurrentHealth;
        int playerOneStocks = playerOne.gameObject.GetComponent<Stock>().Stocks;

        int playerTwoCurrentHealth = playerTwo.gameObject.GetComponent<Health>().CurrentHealth;
        int playerTwoStocks = playerTwo.gameObject.GetComponent<Stock>().Stocks;

        // compare values with old stored ones
        // check player 1 health
        if (prevPlayerOneCurrentHealth != playerOneCurrentHealth)
        {
            UpdateFighterStatusUI(_playerOneCurrentHealth, playerOneCurrentHealth);
            prevPlayerOneCurrentHealth = playerOneCurrentHealth;
        }
        // check player 1 stocks
        if (prevPlayerOneStocks != playerOneStocks)
        {
            UpdateFighterStatusUI(_playerOneStocks, playerOneStocks);
            prevPlayerOneStocks = playerOneStocks;
        }

        // check player 2 health
        if (prevPlayerTwoCurrentHealth != playerTwoCurrentHealth)
        {
            UpdateFighterStatusUI(_playerTwoCurrentHealth, playerTwoCurrentHealth);
            prevPlayerTwoCurrentHealth = playerTwoCurrentHealth;
        }
        // check player 2 stocks
        if (prevPlayerTwoStocks != playerTwoStocks)
        {
            UpdateFighterStatusUI(_playerTwoStocks, playerTwoStocks);
            prevPlayerTwoStocks = playerTwoStocks;
        }

        // store the values again
        prevPlayerOneCurrentHealth = playerOneCurrentHealth;
        prevPlayerOneStocks = playerOneStocks;
        prevPlayerTwoCurrentHealth = playerTwoCurrentHealth;
        prevPlayerTwoStocks = playerTwoStocks;


    }

    // Helper method to update the fighter status ui for the passed element.
    private void UpdateFighterStatusUI(TextMeshProUGUI textObj, int newValue)
    {
        textObj.text = newValue.ToString();
    }


}
