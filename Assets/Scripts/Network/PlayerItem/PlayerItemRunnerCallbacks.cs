using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Roswell Doria
/// Date: 2022-12-03
/// 
/// This file is responsible for callback functions to the network runner
/// when attached to a PlayerItem Prefab
/// </summary>
public class PlayerItemRunnerCallbacks : NetworkBehaviour, INetworkRunnerCallbacks
{
    private bool _player1Joined = false;
    private bool _player2Joined = false;

    //private FighterControlHandler localCharacterControlHandler;

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-03-12
    /// 
    /// This function adds all callbacks of this object to the network runner
    /// on enabled.
    ///
    /// </summary>
    public void OnEnable()
    {
        Debug.Log("Entered Enabled!");
        if(Runner != null)
        {
            Debug.Log("Access to Runner!");
            Runner.AddCallbacks( this );
        }
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-12-03
    /// 
    /// This function removes all callbacks of this object to the network runner
    /// on disable.
    ///
    /// </summary>
    public void OnDisable()
    {
        if( Runner != null )
        {
            Runner.RemoveCallbacks( this );
        }
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-12-03
    /// 
    /// This function is called whenever this object connects to the network runner.
    ///
    /// </summary>
    /// <param name="runner"></param>
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Player item connected to server!");
    }

    //Bellow are required interface methods that currently do not do anything.
    //They can be implemented later on in the future for more functionality.
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){ }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){ }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){ }

    public void OnDisconnectedFromServer(NetworkRunner runner){ }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){ }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){ }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-12-03
    /// 
    /// This function is responsible for callbacks when the onject/player joines
    /// the network. This fucntion will set the PlayerPrefs of the ID of the player that joins
    /// for access to Database functions.
    ///
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
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

    }
    
    //Below are required interface functions that currently dont do anything.
    //Future implementations can be made to increase functionality.
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player){ }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data){ }

    public void OnSceneLoadDone(NetworkRunner runner){ }

    public void OnSceneLoadStart(NetworkRunner runner){ }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){ }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){ }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){ }

}
