using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class AudioEffectsManager : MonoBehaviour
{
    public static AudioEffectsManager Instance1;
    public static AudioEffectsManager Instance2;
    private static int _maxInstanceCounter = 2; //max amount of instances of AudioEffectsManagers.
    private static int _instanceCount = 0; //amount of AudioEffectsManagers instances.
    private static int _oldInstanceNum = 1; //integer indicating which instance is the old instance manager to delete if a new instance is created.

    /// <summary>
    /// Initializes and assigns instances and static members when a new AudioEffectsManager is created.
    /// </summary>
    void Awake()
    {
        if (_instanceCount == 0)
        {
            Instance1 = this;
            DontDestroyOnLoad(gameObject);
            _instanceCount++;
        }
        else if (_instanceCount == 1)
        {
            Instance2 = this;
            DontDestroyOnLoad(gameObject);
            _instanceCount++;
        }
        else if (_instanceCount == _maxInstanceCounter)
        {
            if (_oldInstanceNum == 1)
            {
                Destroy(Instance1.gameObject);
                ChangeOldInstanceIndicator();
                
                Instance1 = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_oldInstanceNum == 2)
            {
                Destroy(Instance2.gameObject);
                ChangeOldInstanceIndicator();

                Instance2 = this;
                DontDestroyOnLoad(gameObject);
            }
            

        }
    }

    /// <summary>
    /// Changes the old instance count pointer to the number of the instance from the prior scene.
    /// </summary>
    private void ChangeOldInstanceIndicator()
    {
        if (_oldInstanceNum == 1) ++_oldInstanceNum;
        else --_oldInstanceNum;
    }

    /// <summary>
    /// Plays a given audio clip through this audio source.
    /// </summary>
    /// <param name="audioClip">  </param>
    public void PlaySoundClip(AudioClip audioClip)
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(audioClip);
    }
}
