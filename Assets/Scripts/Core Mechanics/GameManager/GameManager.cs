using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// all game states during a game match
public enum GameStates { waiting, countDown, running, gameOver };

/// <summary>
/// Static GameManager Class to handle Game States, Timers and Win/Lose Logic in a Game Match.
/// Author(s): Jason Cheung
/// Date: Oct 27 2022
/// Source(s):
///     Countdown - How to create a 2D Arcade Style Top Down Car Controller in Unity tutorial Part 13: https://youtu.be/-SR24s7AryI?t=1560
/// Remarks:
/// Change History:
/// </summary>
public class GameManager : MonoBehaviour
{
    // Static instance of GameManager so other scripts can access it
    public static GameManager instance = null;

    // Current Game State
    public GameStates GameState { get; private set; } = GameStates.waiting;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // set static object
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Set Game State to countDown
    public void SetGameStateWaiting()
    {
        GameState = GameStates.waiting;
        Debug.Log("GameManager - Game State: " + GameState.ToString());

        // TODO: (not in this method) disable player input until this countdown is finished
    }

    // Set Game State to countDown
    public void SetGameStateCountDown()
    {
        GameState = GameStates.countDown;
        Debug.Log("GameManager - Game State: " + GameState.ToString());

        // TODO: (not in this method) disable player input until this countdown is finished
    }

    // Set Game State to running
    public void SetGameStateRunning()
    {
        GameState = GameStates.running;
        Debug.Log("GameManager - Game State: " + GameState.ToString());

        // Start Match Timer
        GameTimerController.instance.StartTimer();
    }

    // Set Game State to gameOver
    public void SetGameStateGameOver()
    {
        GameState = GameStates.gameOver;
        Debug.Log("GameManager - Game State: " + GameState.ToString());

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
