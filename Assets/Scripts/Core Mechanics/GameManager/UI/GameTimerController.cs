using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MatchTimer Handler Class to update the game match timer.
/// Author(s): Jason Cheung
/// Date: Oct 27 2022
/// Source(s):
///     How to Make an In-Game Timer in Unity - Beginner Tutorial: https://youtu.be/qc7J0iei3BU
///     Game Architecture Tips - Unity: https://youtu.be/pRjTM3pzqDw
/// Remarks:
/// Change History:
/// </summary>
public class GameTimerController : NetworkBehaviour
{
    // Static instance of MatchTimer so other scripts can access it
    public static GameTimerController Instance = null;

    // Unity UI Text to update the Match Timer
    public TMPro.TextMeshProUGUI GameTimerText;

    // Length of a Game Match
    // intended game length: 8 minutes
    [SerializeField] private int GameLengthMinutes = 8;

    [SerializeField] private int GameLengthSeconds = 0;

    //[SerializeField] public float TimeValue = 15f;

    private float TimeValue;

    private bool IsTimerRunning;

    // Awake is called when the script instance is being loaded, even if the script is disabled
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        TimeValue = (GameLengthMinutes * 60) + GameLengthSeconds;
        
        // initially hide this game object
        GameTimerText.text = "";

        IsTimerRunning = false;
    }

    /// <summary>
    /// Begin the Game's Match Timer.
    /// </summary>
    public void StartTimer()
    {
        IsTimerRunning = true;

        StartCoroutine(UpdateTimer());
    }

    /// <summary>
    /// End the Game's Match Timer.
    /// </summary>
    public void EndTimer()
    {
        IsTimerRunning = false;
    }

    /// <summary>
    /// Co-routine to update the Game's Match Timer.
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateTimer()
    {
        while (IsTimerRunning)
        {
            if (TimeValue > CountdownController.Instance.EndGameCountdown + 1)
            {
                TimeValue -= Time.deltaTime;
            }
            else
            {
                CountdownController.Instance.BeginEndGameCountdown();
                break;
            }

            float minutes = Mathf.FloorToInt(TimeValue / 60);

            // Update in minutes & seconds
            //float seconds = Mathf.FloorToInt(TimeValue % 60);
            //GameTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Update in minutes, seconds & milliseconds
            float seconds = (TimeValue % 60);
            GameTimerText.text = string.Format("{0:00}:{1:00.00}", minutes, seconds.ToString("F1"));

            yield return null;
        }

        // hide Match Timer at the end of the co-routine; EndGameCountdown will display instead
        GameTimerText.text = "";
    }

}
