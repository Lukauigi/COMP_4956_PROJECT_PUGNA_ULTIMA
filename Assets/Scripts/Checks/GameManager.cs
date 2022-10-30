using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameStates { countDown, running, gameOver }; 


public class GameManager : MonoBehaviour
{
    // Static instance of GameManager so other scripts can access it
    public static GameManager instance = null;

    GameStates gameState = GameStates.countDown;

    private void Awake()
    {
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

    public GameStates GetGameState()
    {
        return gameState;
    }

    void SetGameStateCountDown()
    {
        gameState = GameStates.countDown;
        Debug.Log("GameManager -- Game starting...: " + GetGameState().ToString());
    }

    public void SetGameStateRunning()
    {
        gameState = GameStates.running;
        Debug.Log("GameManager -- Game started: " + GetGameState().ToString());

        // start Match Timer
        MatchTimerHandler.instance.BeginTimer();
    }

    public void SetGameStateGameOver()
    {
        gameState = GameStates.gameOver;
        Debug.Log("GameManager -- Game ended: " + GetGameState().ToString());
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetGameStateCountDown();
    }
}
