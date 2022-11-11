using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

/// <summary>
/// Handles the player spawning on the network side.
/// 
/// Note: 
///     Player input will probably be handled here as well, make sure to have it separated in the future.
/// /// Author(s): Jun Earl Solomon
/// Date: Oct 29 2022
/// Source(s):
///     The ULTIMATE 2D Character CONTROLLER in UNITY (2021): https://youtu.be/lcw6nuc2uaU
/// </summary>
public class FighterSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkPlayer playerPrefab;

    FighterControlHandler localCharacterControlHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// selects a random spawn point location based on the game objects inside the scene.
    /// </summary>
    /// <returns>a Vector3</returns>
    Vector3 GetRandomSpawnPoint()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        if (spawnPoints.Length == 0)
            return Vector3.zero;
        else return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform.position;
    }



    public void OnConnectedToServer(NetworkRunner runner)
    {
        if (runner.Topology == SimulationConfig.Topologies.Shared)
        {
            Debug.Log("OnConnectedToServer, starting player prefab as local player");

            runner.Spawn(playerPrefab, GetRandomSpawnPoint(), Quaternion.identity, runner.LocalPlayer);
        }
    }

    /// <summary>
    /// 
    /// Changes: Ross 2022-11-09
    /// Commented this section out. PlayerItemRunnerCallbacks now controls when the object is spawned.
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        //if (runner.IsServer)
        //{
        //    Debug.Log("OnPlayerJoined we are server. Spawning player");
        //    runner.Spawn(playerPrefab, GetRandomSpawnPoint(), Quaternion.identity, player);

        //    #region (Jason) Start Game by starting the game countdown
        //    // later, this call is when the character select is done,
        //    // both players have spawned, and the match should be ready to start.
        //    CountdownController.instance.BeginStartGameCountdown();
        //    #endregion
        //}
        //else Debug.Log("OnPlayerJoined");
    }

    // Ideally should be in a separate script, InputProvider. https://doc.photonengine.com/en-us/fusion/current/manual/network-input#buttons
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

        //Debug.Log("OnInput");
        if (localCharacterControlHandler == null && NetworkPlayer.Local != null)
        {
            localCharacterControlHandler = NetworkPlayer.Local.GetComponent<FighterControlHandler>();
            //Debug.Log("localCharacterControlHandler gotten");
        }

        if (localCharacterControlHandler != null)
        {
            input.Set(localCharacterControlHandler.GetNetworkInput());
            //Debug.Log("localCharacterControlHandler inputs sent");
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("NotImplementedException - OnPlayerLeft");
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log("NotImplementedException - OnInputMissing");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("NotImplementedException - OnShutdown");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("NotImplementedException - OnDisconnectedFromServer");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log("NotImplementedException - OnConnectRequest");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("NotImplementedException - OnConnectFailed");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log("NotImplementedException - OnUserSimulationMessage");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("NotImplementedException - OnSessionListUpdated");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log("NotImplementedException - OnCustomAuthenticationResponse");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("NotImplementedException - OnHostMigration");
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        Debug.Log("NotImplementedException - OnReliableDataReceived");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("NotImplementedException - OnSceneLoadDone");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("NotImplementedException - OnSceneLoadStart");
    }
}
