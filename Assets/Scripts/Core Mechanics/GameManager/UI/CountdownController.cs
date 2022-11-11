using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// CountDown Handler Class to count down the start and end of a game match.
/// Author(s): Jason Cheung
/// Date: Oct 27 2022
/// Source(s):
///     Countdown - How to create a 2D Arcade Style Top Down Car Controller in Unity tutorial Part 13: https://youtu.be/-SR24s7AryI?t=1560
///     How to Make an In-Game Timer in Unity - Beginner Tutorial: https://youtu.be/qc7J0iei3BU
/// Remarks:
/// Change History:
/// </summary>
public class CountdownController : NetworkBehaviour
{
    // Static instance of GameManager so other scripts can access it
    public static CountdownController Instance = null;

    // Unity UI Text to update the CountDown Timer
    //public Text countdownText;
    public TMPro.TextMeshProUGUI CountdownText;

    // seconds to countdown before starting the game
    [SerializeField] public int GameStartCountdown = 3;

    // seconds to countdown before ending the game
    [SerializeField] public int EndGameCountdown = 5;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        CountdownText.text = "";
    }

    public void BeginStartGameCountdown()
    {
        CountdownText.text = "";
        StartCoroutine(CountdownStartGame());
    }

    /// <summary>
    /// StartEndingCountdown is called when the game match is about to end and its ending countdown should begin.
    /// </summary>
    public void BeginEndGameCountdown()
    {
        CountdownText.text = "";

        // show/re-enable this game object
        gameObject.SetActive(true);

        StartCoroutine(CountdownEndGame());
    }

    /// <summary>
    /// Co-routine to countdown the start of a match.
    /// </summary>
    /// <returns></returns>
    IEnumerator CountdownStartGame()
    {
        yield return new WaitForSeconds(0.3f);

        int counter = GameStartCountdown;

        GameManager.Manager.RPC_SetGameStateCountDown();

        while (true)
        {
            if (counter != 0)
                CountdownText.text = counter.ToString();
            else
            {
                CountdownText.text = "GO!";
                GameManager.Manager.RPC_SetGameStateRunning();
                break;
            }

            counter--;
            yield return new WaitForSeconds(1.0f);
        }

        // how long to keep the 'GO!' text for
        yield return new WaitForSeconds(0.75f);

        // hide this game object
        CountdownText.text = "";
        StopCoroutine(CountdownStartGame());
    }

    /// <summary>
    /// Co-routine to countdown the end of a match.
    /// </summary>
    /// <returns></returns>
    IEnumerator CountdownEndGame()
    {
        int counter = EndGameCountdown;

        while (true)
        {
            if (counter != 0)
                CountdownText.text = counter.ToString();
            else
            {
                CountdownText.text = "TIME!";
                GameManager.Manager.RPC_SetGameStateGameOver();
                break;
            }

            counter--;
            yield return new WaitForSeconds(1.0f);
        }
    }

}
