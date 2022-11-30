using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

/// <summary>
/// Handles the player control on the network side.
/// Player input info is gathered here and sent back to the local player's FighterControlHandler.
/// Note: 
///     Player input will probably be handled here as well, make sure to have it separated in the future.
/// /// Author(s): Jun Earl Solomon
/// Date: Oct 29 2022
/// Source(s):
///     The ULTIMATE 2D Character CONTROLLER in UNITY (2021): https://youtu.be/lcw6nuc2uaU
/// </summary>
public class NetworkPlayerController : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkPlayer playerPrefab;

    FighterControlHandler localCharacterControlHandler;


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
        //}
        //else Debug.Log("OnPlayerJoined");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnDisconnectedFromServer(NetworkRunner runner) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }
}
