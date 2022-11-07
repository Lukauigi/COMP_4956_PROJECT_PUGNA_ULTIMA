using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectSpawnerTest : NetworkBehaviour
{
    public NetworkRunner runner;

    private bool _firstPlayerLoaded;
    private bool _secondPlayerLoaded;

    [SerializeField] private NetworkPrefabRef[] _playerPrefabs;
    [SerializeField] private Transform _transform;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

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
}
