using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// all game states during a game match
public enum GameStates { Waiting, Starting, Running, GameOver };

/// <summary>
/// Static GameManager Class to handle Game States, Timers and Win/Lose Logic in a Game Match.
/// Author(s): Jason Cheung
/// Date: Oct 27 2022
/// Source(s):
///     Countdown - How to create a 2D Arcade Style Top Down Car Controller in Unity tutorial Part 13: https://youtu.be/-SR24s7AryI?t=1560
/// Remarks:
/// Change History:
/// </summary>
public class GameManager : NetworkBehaviour
{
    // Static instance of GameManager so other scripts can access it
    public static GameManager Manager = null;

    // Current Game State
    public GameStates GameState { get; private set; } = GameStates.Waiting;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Debug.Log("GameManager Null: " + Manager);
        Manager = this;
        Debug.Log("GameManager not Null: " + Manager);
    }

    // Set Game State to waiting
    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SetGameStateWaiting()
    {
        GameState = GameStates.Waiting;
        Debug.Log("GameManager - Game State is  " + GameState.ToString());

        // TODO: (not in this method) disable player input until this countdown is finished
    }

    // Set Game State to countdown
    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SetGameStateCountDown()
    {
        GameState = GameStates.Starting;
        Debug.Log("GameManager - Game State is " + GameState.ToString());

        // TODO: (not in this method) disable player input until this countdown is finished
    }

    // Set Game State to running
    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SetGameStateRunning()
    {
        GameState = GameStates.Running;
        Debug.Log("GameManager - Game State is " + GameState.ToString());

        // Start Match Timer
        GameTimerController.Instance.StartTimer();
    }

    // Set Game State to gameOver
    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SetGameStateGameOver()
    {
        GameState = GameStates.GameOver;
        Debug.Log("GameManager - Game State is " + GameState.ToString());

        // TODO:
        // - [optional?] (not in this method) disable player input once game is over
        // - trigger endGame state to end the game, gather win/lose results, and move to the next screen.
    }

    //// This function is called when the object becomes enabled and active
    //private void OnEnable()
    //{
    //    // TODO: ensure GameManager is enabled only after BOTH network players are fully loaded in.

    //    // initialize event
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    ///// <summary>
    ///// OnSceneLoaded Event. Start GameManager by starting the countdown.
    ///// </summary>
    ///// <param name="scene"></param>
    ///// <param name="mode"></param>
    //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    SetGameStateRunning();
    //}
}
