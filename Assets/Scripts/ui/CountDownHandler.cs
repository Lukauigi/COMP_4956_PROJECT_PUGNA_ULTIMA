using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownHandler : MonoBehaviour
{
    // Static instance of GameManager so other scripts can access it
    public static CountDownHandler instance = null;

    public Text countDownText;

    public int countDownStartMatch = 3;
    public int countDownEndMatch = 5;

    void Awake()
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
        countDownText.text = "";
        StartCoroutine(CountDownStartMatch());
    }


    public void StartEndingCountdown()
    {
        countDownText.text = "";

        gameObject.SetActive(true);
        StartCoroutine(CountDownEndMatch());
    }

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

        yield return new WaitForSeconds(0.75f);

        gameObject.SetActive(false);

        StopCoroutine(CountDownStartMatch());
    }


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
