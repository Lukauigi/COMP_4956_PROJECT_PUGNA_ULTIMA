using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemRunnerCallbacks : NetworkBehaviour, INetworkRunnerCallbacks
{
    private Boolean _player1Joined = false;
    private Boolean _player2Joined = false;
    

    private FighterControlHandler localCharacterControlHandler;

    public void OnEnable()
    {
        Debug.Log("Entered Enabled!");
        if(Runner != null)
        {
            Debug.Log("Access to Runner!");
            Runner.AddCallbacks( this );
        }
    }

    public void OnDisable()
    {
        if( Runner != null )
        {
            Runner.RemoveCallbacks( this );
        }
    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Player item connected to server!");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){ }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){ }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){ }

    public void OnDisconnectedFromServer(NetworkRunner runner){ }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){ }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){ }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        
        // Debug.Log("PlayerID Joined:" + player.PlayerId);
        if (!_player1Joined)
        {
            Debug.Log("Player 1 Joined - Server");
            Debug.Log("PlayerID Joined:" + player.PlayerId);
            PlayerPrefs.SetInt("HostID", player.PlayerId);
                _player1Joined = true;
        }
        else if (!_player2Joined)
        {
            Debug.Log("Player 2 Joined - Server");
            Debug.Log("PlayerID Joined:" + player.PlayerId);
            PlayerPrefs.SetInt("ClientID", player.PlayerId);
            _player2Joined = true;
        }
        // if (Runner.IsClient)
        // {
        //     Debug.Log("Player 2 Joined - Client");
        //     Debug.Log("PlayerID Joined:" + player.PlayerId);
        //     _player2Joined = true;
        //     PlayerPrefs.SetInt("ClientID", player.PlayerId);
        //
        // }
        
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player){ }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data){ }

    public void OnSceneLoadDone(NetworkRunner runner){ }

    public void OnSceneLoadStart(NetworkRunner runner){ }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){ }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){ }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){ }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
