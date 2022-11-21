using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Roswell Doria
/// Date: 2022-11-10
/// 
/// The purpose of this Network Behavior is to monitor the PlayerITem Object that is spawned by the Network Runner
/// on player joined. This object observes when both players select a character. Once both have selected a character
/// spawn the associated character prefabs and start GameManager countdown.
/// </summary>
public class PlayerItemObserver : NetworkBehaviour
{
    public static PlayerItemObserver Observer = null;

    // other scene objects to reference
    protected GameManager _gameManager;
    protected NetworkFighterObserver _networkPlayerObserver;

    [SerializeField] private NetworkObject[] CharacterPrefabs;

    private bool isPlayerOneReady = false;
    private bool isPlayerTwoReady = false;

    //default
    private int playerOneIndexSelect = 0;
    private int playerTwoIndexSelect = 0;

    //the fighters they will spawn
    private NetworkObject playerOneFighter;
    private NetworkObject playerTwoFighter;
    
    //default
    private int playerOneRef = 0;
    private int playerTwoRef = 0;

    //Player usernames
    private string _playerOneUsername;
    private string _playerTwoUsername;

    private bool isPlayersSpawned = false;
    

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        Observer = this;
        Debug.Log("PlayerItemObserver instance awake: " + Observer);
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
        if (!_networkPlayerObserver) _networkPlayerObserver = NetworkFighterObserver.Observer;
    }

    /// <summary>
    /// 
    /// Change History:
    /// 2022-11-21 Roswell Doria
    /// - Added paramaters to RPC_CachePlayers() for player one and player two usernames.
    ///
    /// </summary>
    public override void FixedUpdateNetwork()
    {

        // Both players are ready (selected their character)
        if (isPlayerOneReady && isPlayerTwoReady && Runner.IsServer && !isPlayersSpawned)
        {
            // Despawn Player one and player two character select objects
            RPC_DespawnPlayerItems(playerOneRef, playerTwoRef);
            // Spawn Player one and player two selected characters
            RPC_SpawnNetworkFighters(playerOneIndexSelect, playerTwoIndexSelect, playerOneRef, playerTwoRef);
            isPlayersSpawned = true;

            // Assign Player one and player two references to GameManager
            //_networkPlayerObserver.RPC_SetNetworkFighters(playerOneRef, playerOneFighter, playerTwoRef, playerTwoFighter);
            _gameManager.RPC_CachePlayers(playerOneRef, playerOneFighter, playerTwoRef, playerTwoFighter, _playerOneUsername, _playerTwoUsername);


            // Switch Game State to 'Starting' Game
            _gameManager.RPC_SetGameStateStarting();
        }
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-10
    /// 
    /// RPC respsonible for setting players to the ready state.
    /// 
    /// Change history:
    /// 2022-11-21 - Roswell Doria
    ///  - Added paramter username
    ///  - set _playerOneUsername and _playerTwoUsername
    /// 
    /// </summary>
    /// <param name="playerRefIndex">an interger representing the playerRef Index of player one</param>
    /// <param name="playerPrefabIndex">an interger representing the playerRef Index of player two</param>
    /// <param name="isHost">a bool if player is host</param>
    /// <param name="username"> a string of the player's username</param>
    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SetPlayerReady(int playerRefIndex, int playerPrefabIndex, bool isHost, string username)
    { 
        if(isHost)
        {
            playerOneRef = playerRefIndex;
            playerOneIndexSelect = playerPrefabIndex;
            isPlayerOneReady = true;
            _playerOneUsername = username;
        }
        else if (!isHost)
        {
            playerTwoRef = playerRefIndex;
            playerTwoIndexSelect = playerPrefabIndex;
            isPlayerTwoReady = true;
            _playerTwoUsername = username;
        }
        
    }


    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CheckBothPlayerReady()
    {
    
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-10
    /// 
    /// This RPC is responsible for dispawning the PlayerItems that are responsible for character select.
    /// </summary>
    /// <param name="playerOneRef">an interger representing the playerRef Index of player one</param>
    /// <param name="playerTwoRef">an interger representing the playerRef Index of player two</param>
    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_DespawnPlayerItems(int playerOneRef, int playerTwoRef)
    {
        //Obtain PlayerItem Network Object
        NetworkObject playerOnePlayerItem = Runner.GetPlayerObject(playerOneRef);
        NetworkObject playerTwoPlayerItem = Runner.GetPlayerObject(playerTwoRef);
        //Despawn PlayerItem from runner
        Runner.Despawn(playerOnePlayerItem);
        Runner.Despawn(playerTwoPlayerItem);
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-10
    /// 
    /// This Rpc method is to be used to spawn player one and player two objects and assign the associated player references.
    /// </summary>
    /// <param name="playerOneSelected"></param>
    /// <param name="playerTwoSelected"></param>
    /// <param name="playerOneRef"></param>
    /// <param name="playerTwoRef"></param>
    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SpawnNetworkFighters(int playerOneSelected, int playerTwoSelected, int playerOneRef, int playerTwoRef)
    {
        // Player Spawn points
        Vector3 playerOneSpawnLocation = new Vector3(1, 0, 0);
        Vector3 playerTwoSpawnLocation = new Vector3(-1, 0, 0);
        // Spawn players
        playerOneFighter = this.Runner.Spawn(CharacterPrefabs[playerOneSelected], playerOneSpawnLocation, Quaternion.identity, playerOneRef);
        playerTwoFighter = this.Runner.Spawn(CharacterPrefabs[playerTwoSelected], playerTwoSpawnLocation, Quaternion.identity, playerTwoRef);

    }

}
