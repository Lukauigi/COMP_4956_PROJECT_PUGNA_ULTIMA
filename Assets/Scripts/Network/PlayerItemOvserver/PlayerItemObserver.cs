using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemObserver : NetworkBehaviour
{
    public static PlayerItemObserver Observer = null;

    [SerializeField] private NetworkObject[] CharacterPrefabs;

    private bool PlayerOneReady = false;
    private bool PlayerTwoReady = false;

    //default
    private int PlayerOneIndexSelect = 0;
    private int PlayerTwoIndexSelect = 0;

    //default
    private int PlayerOneRef = 0;
    private int PlayerTwoRef = 0;

    private bool Spawned = false;
    
    public void Awake()
    {
        Debug.Log("Observer Null: " + Observer);
        Observer = this;
        Debug.Log("Obserer not Null: " + Observer);
    }
    public override void FixedUpdateNetwork()
    {
        // show console logs that player(s) is not ready
        if (!PlayerOneReady || !PlayerTwoReady)
        {
            //Debug.Log("PlayerOneReady: " + PlayerOneReady);
            //Debug.Log("PlayerTwoReady: " + PlayerTwoReady);
        }

        //Both players are ready, Host connection runs this code
        if (PlayerOneReady && PlayerTwoReady && Runner.IsServer && !Spawned)
        {
            
            Debug.Log("PlayerItemObserver.cs : Both players are ready, Host connection runs this code");
            // Despawn Player one and player two character select objects
            RPC_DespawnPlayerItems(PlayerOneRef, PlayerTwoRef);
            // Spawn Player one and player two selected characters
            RPC_SpawnBothPlayers(PlayerOneIndexSelect, PlayerTwoIndexSelect, PlayerOneRef, PlayerTwoRef);
            Spawned = true;

            // Switch Game State to 'Starting' Game
            //CountdownController.Instance.RPC_StartStartingCountdown();
            GameManager.Manager.RPC_SetGameStateStarting();
        }
    }

    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SetPlayerReady(int playerRefIndex, int playerPrefabIndex, bool isHost)
    { 
        Debug.Log("PlayerItemObserver.cs : RPC_SetPlayerReady() cycle");
        Debug.Log("PlayerRefIndex: " + playerRefIndex);
        if(isHost)
        {
            PlayerOneRef = playerRefIndex;
            PlayerOneIndexSelect = playerPrefabIndex;
            PlayerOneReady = true;
        }
        else
        {
            PlayerTwoRef = playerRefIndex;
            PlayerTwoIndexSelect = playerPrefabIndex;
            PlayerTwoReady = true;

        }
        
    }

    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CheckBothPlayerReady()
    {
    
    }

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

    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SpawnBothPlayers(int playerOneSelected, int playerTwoSelected, int playerOneRef, int playerTwoRef)
    {
        //Player Spawn points
        Vector3 playerOneSpawnLocation = new Vector3(1, 0, 0);
        Vector3 playerTwoSpawnLocation = new Vector3(-1, 0, 0);
        // Spawn player one
        this.Runner.Spawn(CharacterPrefabs[playerOneSelected], playerOneSpawnLocation, Quaternion.identity, playerOneRef);
        this.Runner.Spawn(CharacterPrefabs[playerTwoSelected], playerTwoSpawnLocation, Quaternion.identity, playerTwoRef);
    }

    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_StartCountDownGameState() { }
}
