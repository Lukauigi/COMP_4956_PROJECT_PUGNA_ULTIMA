using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Linq;

/// <summary>
/// !!! DEPRECATED AFTER MERGING CORE MECHANICS SCENE WITH NETWORKING !!!
/// Handles and tells Photon Fusion's Network Runner what to do.
/// This is where you initialize what the network runner is going to be.
/// Author(s): Jun Earl Solomon
/// Date: Oct 29 2022
/// Source(s):
///     Online multiplayer with Photon Fusion - 2D Arcade Style Car Controller in Unity tutorial Part 12: https://youtu.be/yrXQSM1cleU
///     Online multiplayer ?? FPS Unity & Photon Fusion EP1 (Fusion setup + movement): https://youtu.be/hqIZCoLHOig
/// </summary>
public class NetworkRunnerHandler : MonoBehaviour
{

    NetworkRunner networkRunner;

    private void Awake()
    {
        networkRunner = GetComponent<NetworkRunner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.name = "Network Runner";

        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.AutoHostOrClient, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);

        Debug.Log($"Server NetworkRunner started.");
    }

    /// <summary>
    /// task to initialize the network runner
    /// </summary>
    /// <param name="runner">The network runner</param>
    /// <param name="gameMode">The game's mode</param>
    /// <param name="address">The game's address</param>
    /// <param name="scene">The game's scene</param>
    /// <param name="initialized">The action</param>
    /// <returns></returns>
    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        runner.ProvideInput = true;

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = "TestRoom",
            Initialized = initialized,
            SceneManager = sceneManager
        });
    }
}
