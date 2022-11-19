using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;
    private bool _firstPlayerLoaded;
    private bool _secondPlayerLoaded;

    public int selectedCharacter = 0;

    [SerializeField] private NetworkPrefabRef[] _playerPrefabs;
    [SerializeField] private Transform _transform;

    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

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
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Debug.Log(player);
            updatePlayers(runner, player);
            // Create a unique position for the player
            //Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
            //NetworkObject networkPlayerObject = runner.Spawn(_playerPrefabs[selectedCharacter], spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars so we can remove it when they disconnect
            //_spawnedCharacters.Add(player, networkPlayerObject);
        }
    }

    public void updatePlayers(NetworkRunner runner, PlayerRef _player)
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

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // Find and remove the players avatar
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        //if (Input.GetKey(KeyCode.W))
        //    data.direction += Vector3.forward;

        //if (Input.GetKey(KeyCode.S))
        //    data.direction += Vector3.back;

        //if (Input.GetKey(KeyCode.A))
        //    data.direction += Vector3.left;

        //if (Input.GetKey(KeyCode.D))
        //    data.direction += Vector3.right;

        input.Set(data);
    }
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
