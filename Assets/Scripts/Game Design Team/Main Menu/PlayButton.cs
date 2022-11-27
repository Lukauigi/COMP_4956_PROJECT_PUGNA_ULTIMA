using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the PlayButton to play game screen.
/// Authors: Xiang Zhu
/// Date: Oct  28 2022
/// Source:
///     How to Make a Main Menu in Unity 2022 - https://www.youtube.com/watch?v=FfaG9TvCe5g&t=742s
/// </summary>
public class PlayButton : MonoBehaviour
{
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        
    }

    /// <summary>
    /// Onclick the button to load the new scene to Select&GamePlay.
    /// </summary>
    public void OnClickPlayButton()
    {
        SceneManager.LoadScene("Select&GamePlay");
    }
}
