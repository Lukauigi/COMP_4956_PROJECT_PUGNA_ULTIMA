using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Auhtor: Roswell Doria
/// Date: 2022-12-03
/// 
/// This class is responsible for spawning character select objects when clients connect to
/// gameplay scene.
///
/// </summary>
public class CharacterSelectSpawner : MonoBehaviour, INetworkRunnerCallbacks //here
{
    //The network runner represents client or server simulations
    private NetworkRunner _runner;
    private bool _firstPlayerLoaded;
    private bool _secondPlayerLoaded;

    public int selectedCharacter = 0;

    [SerializeField]
    private NetworkPrefabRef[] _playerPrefabs;

    [SerializeField]
    private Transform _transform;


    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-12-03
    /// 
    /// This function is responible for displaying the gui.
    ///
    /// </summary>
    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Shared);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Shared);
            }
        }
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-12-03
    /// 
    /// This function is responsible for Starting the game.
    ///
    /// </summary>
    /// <param name="mode"></param>
    async void StartGame(GameMode mode)
    {
        // create the fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "testroom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-12-03
    /// 
    /// This function is responsible for updating players on player joined to
    /// the network
    ///
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Debug.Log(player);
            UpdatePlayers(runner, player);
        }
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-12-03
    ///
    /// This function is responsible for updating player object positions on connection
    ///
    /// </summary>
    /// <param name="runner">NetworkRunner The network simulation</param>
    /// <param name="_player">PlayerRef the player reference</param>
    public void UpdatePlayers(NetworkRunner runner, PlayerRef _player) //here
    {
        if (!_firstPlayerLoaded)
        {
            Debug.Log(_player);
            _firstPlayerLoaded = !_firstPlayerLoaded;

            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3(-3, 2, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefabs[0], spawnPosition, Quaternion.identity, _player);
            //Set the parent object of the spawned networked object
            networkPlayerObject.transform.SetParent(_transform);
            networkPlayerObject.transform.localScale = new Vector3(1, 1, 1);

            // Keep track of the player avatars so we can remove it when they disconnect
            _spawnedCharacters.Add(_player, networkPlayerObject);

            return;
        }
        else
        {
            _secondPlayerLoaded = !_secondPlayerLoaded;
            // Create a unique position for the player

            Vector3 spawnPosition = new Vector3(3, 2, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefabs[0], spawnPosition, Quaternion.identity, _player);
            networkPlayerObject.transform.SetParent(_transform);
            networkPlayerObject.transform.localScale = new Vector3(1, 1, 1);

            // Keep track of the player avatars so we can remove it when they disconnect
            _spawnedCharacters.Add(_player, networkPlayerObject);
        }
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-12-03
    /// 
    /// This function is responsible for handling On player left network.
    /// Despawn objects.
    ///
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // Find and remove the players avatar
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-12-03
    ///
    /// This function is responsible for setting valid input commands for connected players.
    ///
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="input"></param>
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();
        input.Set(data);
    }

    // The following functions below are required methods for iterface.
    // This function currently do nothing but may need to be implemented in the future.
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
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
