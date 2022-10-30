using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchTimerHandler : MonoBehaviour
{
    // Static instance of MatchTimer so other scripts can access it
    public static MatchTimerHandler instance = null;

    public Text matchTimerText;

    //private TimeSpan durationLeft = new TimeSpan(0, 8, 0);
    private TimeSpan startCountDownFinishMatch = new TimeSpan(0, 0, 10);

    private TimeSpan timePlaying;
    private bool timerGoing;

    private float elapsedTime;

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
    private void Start()
    {
        gameObject.SetActive(false);
        matchTimerText.text = "00:00.00";
        timerGoing = false;
    }

    public void BeginTimer()
    {
        gameObject.SetActive(true);
        timerGoing = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerGoing = false;
    }

    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            // todo: make match timer countdown instead of counting up
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string matchTimerStr = timePlaying.ToString("mm':'ss'.'ff");
            matchTimerText.text = matchTimerStr;

            if (timePlaying > startCountDownFinishMatch)
            {
                CountDownHandler.instance.StartEndingCountdown();
                break;
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }

}
