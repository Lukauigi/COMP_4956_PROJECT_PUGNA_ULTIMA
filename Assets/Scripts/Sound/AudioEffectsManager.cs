using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An audio sound effects manager which manages all sound effects in a scene.
/// Author(s): Lukasz Bednarek
/// Date: November 11, 2022
/// Source(s): N/A
/// Remarks:
/// Change History: November 11 - Lukasz Bednarek
/// Added new methods for looping sound clips.
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
        /*  
        This process is done to have an audio effect play through a scene transition and have the new audio effects manager for the next scene.
         */

        //If no instances, create the first instance of an audio effects manager.
        if (_instanceCount == 0)
        {
            Instance1 = this;
            DontDestroyOnLoad(gameObject);
            _instanceCount++;
        }

        //If 1 instance, create another instance of an audio effects manager.
        else if (_instanceCount == 1)
        {
            Instance2 = this;
            DontDestroyOnLoad(gameObject);
            _instanceCount++;
        }

        //If 2 instances, destroy the old audio effects manager and make a new one in its place and point to the other audio effects manager as the old manager.
        else if (_instanceCount == _maxInstanceCounter)
        {
            //replace instance 1 with new instance.
            if (_oldInstanceNum == 1)
            { 
                Destroy(Instance1.gameObject); 
                ChangeOldInstanceIndicator(); //change count to 2 to indicate instance 2 is the older manager.
                
                Instance1 = this;
                DontDestroyOnLoad(gameObject);
            }

            //replace instance 2 with new instance.
            else if (_oldInstanceNum == 2)
            {
                Destroy(Instance2.gameObject);
                ChangeOldInstanceIndicator(); //change count to 1 to indicate instance 1 is the older manager.

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
    /// Plays a given audio clip without looping through this audio source.
    /// </summary>
    /// <param name="audioClip"> a clip of audio. </param>
    public void PlaySoundClipOnce(AudioClip audioClip)
    {
        this.gameObject.GetComponent<AudioSource>().loop = false;
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(audioClip);
    }

    /// <summary>
    /// Plays a given audio clip which is meant to loop its audio through this audio source.
    /// </summary>
    /// <param name="audioClip"> a clip of audio. </param>
    public void PlayLoopingSoundClip(AudioClip audioClip)
    {
        this.gameObject.GetComponent<AudioSource>().loop = true;
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(audioClip);
    }

    /// <summary>
    /// Stops the current sound effect of a looping audio clip.
    /// </summary>
    public void StopLoopingSoundClip()
    {
        this.gameObject.GetComponent<AudioSource>().Stop();
    }
}
