using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Countdown Controller Class to count down the start and end of a game match.
/// Author(s): Jason Cheung
/// Date: Oct 27 2022
/// Source(s):
///     Countdown - How to create a 2D Arcade Style Top Down Car Controller in Unity tutorial Part 13: https://youtu.be/-SR24s7AryI?t=1560
///     How to Make an In-Game Timer in Unity - Beginner Tutorial: https://youtu.be/qc7J0iei3BU
/// Remarks:
/// Change History: Nov 11 2022 - Jason Cheung
/// - Modified methods to be networked and use rpc calls
/// - bugfix: shows on both host & client instances
/// </summary>
public class CountdownController : NetworkBehaviour
{
    // Static instance of GameManager so other scripts can access it
    public static CountdownController Instance = null;

    // Unity UI Text to update the CountDown Timer
    [SerializeField] private TextMeshProUGUI _countdownText;

    // seconds to countdown before starting the game
    [SerializeField] private int _startingCountdown = 3;

    // seconds to countdown before ending the game
    [SerializeField] private int _endingCountdown = 5;

    // getters
    public int StartingCountdown => _startingCountdown;
    public int EndingCountdown => _endingCountdown;

    private float startingDelay = 0.3f;
    public float StartingDelay => startingDelay;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        Instance = this;
        _countdownText.text = "";
    }

    /// <summary>
    /// Start StartingCoundown when the game is ready to start.
    /// Only the host observes the ready status of both PlayerItems, so only the host can call this rpc.
    /// </summary>
    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_StartStartingCountdown()
    {
        StartCoroutine(UpdateStartingCountdown());
    }

    /// <summary>
    /// Start EndingCoundown when the game is about to end.
    /// Either the host/client can call this rpc, depending on who's game timer is earlier (if they are no longer sync'd)
    /// </summary>
    [Rpc(sources: RpcSources.All, RpcTargets.All)]
    public void RPC_StartEndingCountdown()
    {
        StartCoroutine(UpdateEndingCountdown());
    }

    /// <summary>
    /// Co-routine to countdown the start of a match.
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateStartingCountdown()
    {
        yield return new WaitForSeconds(startingDelay);

        int counter = _startingCountdown;

        while (true)
        {
            if (counter != 0)
                _countdownText.text = counter.ToString();
            else
            {
                _countdownText.text = "GO!";
                GameManager.Manager.RPC_SetGameStateRunning();
                break;
            }

            counter--;
            yield return new WaitForSeconds(1.0f);
        }

        // how long to keep the 'GO!' text for
        yield return new WaitForSeconds(0.75f);

        // hide the ui and stop updating the countdown
        _countdownText.text = "";
        StopCoroutine(UpdateStartingCountdown());
    }

    /// <summary>
    /// Co-routine to countdown the end of a match.
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateEndingCountdown()
    {
        int counter = _endingCountdown;

        while (true)
        {
            if (counter != 0)
                _countdownText.text = counter.ToString();
            else
            {
                //DisplayEndText();
                GameManager.Manager.RPC_SetGameStateGameOver();
                break;
            }

            counter--;
            yield return new WaitForSeconds(1.0f);
        }

        // how long to keep the 'TIME!' text for
        yield return new WaitForSeconds(2.5f);

        // hide the ui and stop updating the countdown
        _countdownText.text = "";
        StopCoroutine(UpdateEndingCountdown());
    }

    public void DisplayEndText()
    {
        _countdownText.text = "TIME!";
    }

}
