using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class AudioEffectsManager : MonoBehaviour
{
    public static AudioEffectsManager Instance;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="audioClip">  </param>
    public void PlaySoundClip(AudioClip audioClip)
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(audioClip);
    }
}
