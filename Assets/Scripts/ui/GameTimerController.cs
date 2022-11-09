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
public class GameTimerController : MonoBehaviour
{
    // Static instance of MatchTimer so other scripts can access it
    public static GameTimerController instance = null;

    // Unity UI Text to update the Match Timer
    //public Text gameTimerText;
    public TMPro.TextMeshProUGUI gameTimerText;

    // Length of a Game Match
    //private TimeSpan durationLeft = new TimeSpan(0, 8, 0);

    // Length before the game ending Countdown should begin
    // testing: 1 minute game length
    private TimeSpan startCountdownToFinishGame = new TimeSpan(0, 1, 0);

    private TimeSpan timePlaying;

    private bool timerRunning;

    private float elapsedTime;

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
        // Unity Warning: DontDestroyOnLoad only works for root GameObjects or components on root GameObjects
        // commented out for now
        //DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        // initially hide this game object
        gameTimerText.text = "";

        timerRunning = false;
    }

    /// <summary>
    /// Begin the Game's Match Timer.
    /// </summary>
    public void StartTimer()
    {
        timerRunning = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    /// <summary>
    /// End the Game's Match Timer.
    /// </summary>
    public void EndTimer()
    {
        timerRunning = false;
    }

    /// <summary>
    /// Co-routine to update the Game's Match Timer.
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateTimer()
    {
        while (timerRunning)
        {
            // TODO: BUG - make match timer countdown instead of counting up
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string gameTimerString = timePlaying.ToString("mm':'ss'.'ff");
            gameTimerText.text = gameTimerString;

            // display ending countdown when the Game Match is about to end
            if (timePlaying > startCountdownToFinishGame)
            {
                CountdownController.instance.BeginEndGameCountdown();
                break;
            }

            yield return null;
        }

        // hide Match Timer at the end of the co-routine; Game ending countdown is displayed instead
        gameTimerText.text = "";
    }

}
