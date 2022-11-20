using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameTimer Controller Class to update the game match timer.
/// Author(s): Jason Cheung
/// Date: Oct 27 2022
/// Source(s):
///     How to Make an In-Game Timer in Unity - Beginner Tutorial: https://youtu.be/qc7J0iei3BU
///     Game Architecture Tips - Unity: https://youtu.be/pRjTM3pzqDw
/// Remarks:
/// Change History: Nov 11 2022 - Jason Cheung
/// - Modified methods to be networked and use rpc calls
/// - bugfix: shows on both host & client instances
/// - bugfix: GameTimer now counts down the remaining game time
/// </summary>
public class GameTimerController : NetworkBehaviour
{
    // Static instance of GameTimerController so other scripts can access it
    public static GameTimerController Instance = null;

    // Unity UI Text to update the Match Timer
    [SerializeField] private TMPro.TextMeshProUGUI _gameTimerText;

    // Length of a Game Match, in minutes & seconds
    [SerializeField] private int _gameLengthMinutes = 8;
    [SerializeField] private int _gameLengthSeconds = 0;

    // Game timer value to display
    private float _timeValue;

    private bool _isTimerRunning = false;

    // Awake is called when the script instance is being loaded, even if the script is disabled
    private void Awake()
    {
        Instance = this;
        _timeValue = (_gameLengthMinutes * 60) + _gameLengthSeconds;
        _gameTimerText.text = "";
    }

    /// <summary>
    /// Begin the Game's Match Timer.
    /// </summary>
    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_StartTimer()
    {
        _isTimerRunning = true;
    }

    /// <summary>
    /// End the Game's Match Timer.
    /// </summary>
    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_EndTimer()
    {
        _isTimerRunning = false;

        // hide game timer text; ending countdown will show instead
        CountdownController.Instance.RPC_StartEndingCountdown();
    }

    private void Update()
    {
        if (!_isTimerRunning)
        {
            _gameTimerText.text = "";
            return;
        }

        if (_timeValue > CountdownController.Instance.EndingCountdown + 1)
        {
            _timeValue -= Time.deltaTime;
        }
        else
        {
            RPC_EndTimer();
        }

        // Display Timer in minutes & seconds
        int minutes = Mathf.FloorToInt(_timeValue / 60);
        int seconds = Mathf.FloorToInt(_timeValue % 60);
        _gameTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Display Timer in minutes, seconds & milliseconds
        //int minutes = Mathf.FloorToInt(_timeValue / 60);
        //int seconds = Mathf.FloorToInt(_timeValue % 60);
        //int hundredths = Mathf.FloorToInt((_timeValue * 100) % 60);
        //_gameTimerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);
    }

}
