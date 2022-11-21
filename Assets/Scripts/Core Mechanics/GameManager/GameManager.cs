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
/// Change History: Nov 11 2022 - Jason Cheung
/// - Modified methods to be networked and use rpc calls
/// - 
/// - bugfix: shows on both host & client instances
/// </summary>
public class GameManager : NetworkBehaviour
{
    // Static instance of GameManager so other scripts can access it
    public static GameManager Manager = null;

    // other scene objects to reference
    protected GameTimerController _gameTimerController;
    protected CountdownController _countdownController;
    protected NetworkFighterObserver _networkFighterObserver;
    protected GameResultsController _gameResultsController;

    // the fighter they are controlling
    private NetworkObject _playerOne;
    private NetworkObject _playerTwo;

    // player references - fusion gives them a player id
    private int _playerOneRef = 0;
    private int _playerTwoRef = 0;

    // the winner and loser when the game ends
    private NetworkObject _winner;
    private NetworkObject _loser;

    // Current Game State
    public GameStates GameState { get; private set; } = GameStates.Waiting;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Manager = this;
        Debug.Log("GameManager instance awake: " + Manager);
    }

    // Start is called after Awake, and before Update
    public void Start()
    {
        CacheOtherObjects();
    }

    // Helper method to initialize OTHER game objects and their components
    private void CacheOtherObjects()
    {
        if (!_gameTimerController) _gameTimerController = GameTimerController.Instance;
        if (!_countdownController) _countdownController = CountdownController.Instance;
        if (!_networkFighterObserver) _networkFighterObserver = NetworkFighterObserver.Observer;
        if (!_gameResultsController) _gameResultsController = GameResultsController.Instance;
    }

    // Method to cache the selected and spawned fighters
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_CachePlayers(int playerOneRef, NetworkObject playerOne, int playerTwoRef, NetworkObject playerTwo)
    {
        if (!_playerOne) _playerOne = playerOne;
        if (!_playerTwo) _playerTwo = playerTwo;

        _playerOneRef = playerOneRef;
        _playerTwoRef = playerTwoRef;

        // cache players for the other scene objects that need it
        _networkFighterObserver.RPC_CachePlayers(playerOneRef, playerOne, playerTwoRef, playerTwo);
        _gameResultsController.RPC_CachePlayers(playerOne, playerTwo);
    }

    // Set Game State to waiting
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_SetGameStateWaiting()
    {
        if (GameState != GameStates.Waiting)
        {
            GameState = GameStates.Waiting;
            Debug.Log("GameManager state is: " + GameState.ToString());
        }
    }

    // Set Game State to countdown
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_SetGameStateStarting()
    {
        if (GameState != GameStates.Starting)
        {
            GameState = GameStates.Starting;
            Debug.Log("GameManager state is: " + GameState.ToString());

            RPC_OnGameStateStarting();
            // TODO: (not in this method) disable player input until this countdown is finished
        }
    }

    // Set Game State to running
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_SetGameStateRunning()
    {
        if (GameState != GameStates.Running)
        {
            GameState = GameStates.Running;
            Debug.Log("GameManager state is: " + GameState.ToString());

            RPC_OnGameStateRunning();
        }
    }

    // Set Game State to gameOver
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_SetGameStateGameOver()
    {
        if (GameState != GameStates.GameOver)
        {
            GameState = GameStates.GameOver;
            Debug.Log("GameManager state is: " + GameState.ToString());
        }

        if (GameState == GameStates.GameOver && Object.HasStateAuthority)
        {
            RPC_OnGameStateGameOver();
        }
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    protected void RPC_OnGameStateStarting()
    {
        // start the starting countdown
        CountdownController.Instance.RPC_StartStartingCountdown();
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    protected void RPC_OnGameStateRunning()
    {
        // start the game timer
        GameTimerController.Instance.RPC_StartTimer();

        StartCoroutine(GameRunningCheck());
    }

    IEnumerator GameRunningCheck()
    {
        while (GameState == GameStates.Running)
        {
            // TODO:
            // check if a player has left
            // if so, forcibly end the game

            // perform this co-routine check every second
            yield return new WaitForSeconds(1f);
        }

        // stop this check since gamestate has changed
        StopCoroutine(GameRunningCheck());
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    protected void RPC_OnGameStateGameOver()
    {
        // display the "TIME!" text and stop the game timer
        _countdownController.DisplayEndText();
        _gameTimerController.gameObject.SetActive(false);

        _gameResultsController.RPC_CacheGameResults();

        // TODO:
        // - disable player input once game is over (not in this method?)

        StartCoroutine(GameOverCheck());
    }


    // Helper method to hide the game scene like players, stage, game ui
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    protected void RPC_HideGameScene()
    {
        // hide players, stage, game ui
        _playerOne.gameObject.SetActive(false);
        _playerTwo.gameObject.SetActive(false);
        //transform.parent.Find("GameStage").gameObject.SetActive(false);
        _countdownController.gameObject.SetActive(false);
        _networkFighterObserver.gameObject.SetActive(false);
    }


    IEnumerator GameOverCheck()
    {
        bool cachedGameResults = false;
        bool showedGameResults = false;

        while (GameState == GameStates.GameOver)
        {
            if (Object.HasStateAuthority && !cachedGameResults)
            {
                cachedGameResults = true;
                // ask the game results controller to process the end-game results
                _gameResultsController.RPC_CacheGameResults();
            }

            yield return new WaitForSeconds(3.5f);

            if (Object.HasStateAuthority && !showedGameResults)
            {
                showedGameResults = true;

                // update the scene to hide game elements
                RPC_HideGameScene();

                // update the game results controller to display results
                _gameResultsController.RPC_ShowGameResults();
            }

        }

        // stop this check since gamestate has changed
        StopCoroutine(GameOverCheck());
    }

}
