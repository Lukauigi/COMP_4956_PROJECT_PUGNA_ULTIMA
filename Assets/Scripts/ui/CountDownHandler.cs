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
public class CountDownHandler : MonoBehaviour
{
    // Static instance of GameManager so other scripts can access it
    public static CountDownHandler instance = null;

    // Unity UI Text to update the CountDown Timer
    public Text countDownText;

    // seconds to countdown before starting the match
    public int countDownStartMatch = 3;

    // seconds to countdown before ending the match
    public int countDownEndMatch = 5;

    // Awake is called when the script instance is being loaded
    void Awake()
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
        countDownText.text = "";
        StartCoroutine(CountDownStartMatch());
    }

    /// <summary>
    /// StartEndingCountdown is called when the game match is about to end and its ending countdown should begin.
    /// </summary>
    public void StartEndingCountdown()
    {
        countDownText.text = "";

        // show/re-enable this game object
        gameObject.SetActive(true);

        StartCoroutine(CountDownEndMatch());
    }

    /// <summary>
    /// Co-routine to countdown the start of a match.
    /// </summary>
    /// <returns></returns>
    IEnumerator CountDownStartMatch()
    {
        yield return new WaitForSeconds(0.3f);

        int counter = countDownStartMatch;

        while (true)
        {
            if (counter != 0)
                countDownText.text = counter.ToString();
            else
            {
                countDownText.text = "GO!";
                GameManager.instance.SetGameStateRunning();
                break;
            }

            counter--;
            yield return new WaitForSeconds(1.0f);
        }

        // how long to keep the 'GO!' text for
        yield return new WaitForSeconds(0.75f);

        // hide this game object
        gameObject.SetActive(false);
        StopCoroutine(CountDownStartMatch());
    }

    /// <summary>
    /// Co-routine to countdown the end of a match.
    /// </summary>
    /// <returns></returns>
    IEnumerator CountDownEndMatch()
    {
        int counter = countDownEndMatch;

        while (true)
        {
            if (counter != 0)
                countDownText.text = counter.ToString();
            else
            {
                countDownText.text = "TIME!";
                GameManager.instance.SetGameStateGameOver();
                break;
            }

            counter--;
            yield return new WaitForSeconds(1.0f);
        }
    }

}
