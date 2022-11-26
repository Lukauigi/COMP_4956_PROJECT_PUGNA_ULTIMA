using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The names of music tracks.
/// Author(s): Lukasz Bednarek
/// Date: November 26, 2022
/// </summary>
public enum MusicTrack { UniversalMenu, Battle }

/// <summary>
/// The manager of music in the game.
/// </summary>
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    
    // volume presets.
    public const float FullVolume = 1.0f;
    public const float MuffledVolume = 0.6f;

    private AudioSource _musicSource;

    // music audio clips
    [SerializeField] private AudioClip _universalMenuTheme;
    [SerializeField] private AudioClip _battleTheme;

    private Dictionary<MusicTrack, AudioClip> _musicCatalogue;

    /// <summary>
    /// Initializes a game object's components. Ideal section to initialize instance data of game object.
    /// </summary>
    void Awake()
    {
        if (Instance == null) // singleton.
        {
            Instance = this;
            this._musicSource = gameObject.GetComponent<AudioSource>();
            this._musicCatalogue = InitializeMusicCatalogue();
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    /// <summary>
    /// Constructs the music catalogue of the game.
    /// </summary>
    /// <returns>A dictionary of music tracks with corresponding audio clips.</returns>
    private Dictionary<MusicTrack, AudioClip> InitializeMusicCatalogue()
    {
        return new Dictionary<MusicTrack, AudioClip>
        {
            { MusicTrack.UniversalMenu, _universalMenuTheme },
            { MusicTrack.Battle, _battleTheme }
        };
    }

    /// <summary>
    /// Plays the specified audio track.
    /// </summary>
    /// <param name="theme"></param>
    public void SwitchMusicTrack(MusicTrack theme)
    {
        _musicSource.Stop();
        _musicSource.clip = _musicCatalogue[theme];
        _musicSource.Play();
    }

    /// <summary>
    /// Sets the music volume to a pre-determined volume.
    /// </summary>
    public void MuffleVolume()
    {
        _musicSource.volume = MuffledVolume;
    }

    /// <summary>
    /// Sets the volume of music to the maximum.
    /// </summary>
    public void SetFullVolume()
    {
        _musicSource.volume = FullVolume;
    }
    
}
