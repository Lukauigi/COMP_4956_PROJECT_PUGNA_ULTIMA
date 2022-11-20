using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _effectsSource;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void PlaySound(AudioClip audioClip)
    {
        print("before" + this.gameObject.activeInHierarchy);
        Instance.gameObject.SetActive(true);
        print("after" + this.gameObject.activeInHierarchy);
        _effectsSource.PlayOneShot(audioClip);
    }
}
