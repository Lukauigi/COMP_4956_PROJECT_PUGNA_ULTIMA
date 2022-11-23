using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// An enumeration of available player actions of a character.
/// Author(s): Lukasz Bednarek
/// Date: November 23, 2022
/// Remarks: Functionality relating to move is present but not functioning properly.
/// Change History: November 23, 2022 - Lukasz Bednarek
/// - Add enumeratioon
/// - Add docuementation
/// </summary>
public enum PlayerActions
{
    Attack,
    ReceiveDamage,
    Move,
    Death,
    Jump,
    JumpLand,
    Dodge
}

public enum MenuActions
{
    Navigate,
    Confirm,
    Revert,
    Error
}

/// <summary>
/// An audio manager GameObject for the gameplay battle scene.
/// Author(s): Lukasz Bednarek
/// Date: November 22, 2022
/// Remarks: N/A
/// Change History: November 22, 2022 = Lukasz Bednarek
/// -Add class
/// -Add documentation
/// </summary>
public class GameplayAudioManager : NetworkBehaviour
{
    public static GameplayAudioManager Instance = null;

    // sound pools for actions with multiple audio clips
    [SerializeField] private AudioClip[] attackSoundPool;
    [SerializeField] private AudioClip[] receiveDamageSoundPool;
    [SerializeField] private AudioClip[] moveSoundPool;

    // universally-shared character audio clips
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip jumpLandSound;
    [SerializeField] private AudioClip dodgeSound;

    // menu interaction audio clips
    [SerializeField] private AudioClip navigateSound;
    [SerializeField] private AudioClip confirmSound;
    [SerializeField] private AudioClip revertSound;
    [SerializeField] private AudioClip errorSound;

    // categorical data structures for certain audio effects
    private Dictionary<string, AudioClip[]> hostPlayerAudio;
    private Dictionary<string, AudioClip[]> clientPlayerAudio;
    private Dictionary<string, AudioClip> universalPlayerAudio;
    private Dictionary<string, AudioClip> menuAudioPlayer;

    // audio sources, can be thought of as audio channels
    [SerializeField] public AudioSource sfxAudioSource;
    [SerializeField] public AudioSource musicAudioSource;
    [SerializeField] public AudioSource moveLoopAudioSource;

    /// <summary>
    /// Initializes a game object's components. Ideal section to initialize instance data of game object.
    /// </summary>
    private void Awake()
    {
        Instance = this;

        hostPlayerAudio = new Dictionary<string, AudioClip[]> {
            { PlayerActions.Attack.ToString(), attackSoundPool },
            { PlayerActions.ReceiveDamage.ToString(), receiveDamageSoundPool },
            { PlayerActions.Move.ToString(), moveSoundPool }
        };

        clientPlayerAudio = new Dictionary<string, AudioClip[]> {
            { PlayerActions.Attack.ToString(), attackSoundPool },
            { PlayerActions.ReceiveDamage.ToString(), receiveDamageSoundPool },
            { PlayerActions.Move.ToString(), moveSoundPool }
        };

        universalPlayerAudio = new Dictionary<string, AudioClip>
        {
            { PlayerActions.Death.ToString(), deathSound },
            { PlayerActions.Dodge.ToString(), dodgeSound },
            { PlayerActions.Jump.ToString(), jumpSound },
            { PlayerActions.JumpLand.ToString(), jumpLandSound }
        };

        menuAudioPlayer = new Dictionary<string, AudioClip>
        {
            { MenuActions.Navigate.ToString(), navigateSound },
            { MenuActions.Confirm.ToString(), confirmSound },
            { MenuActions.Revert.ToString(), revertSound },
            { MenuActions.Error.ToString(), errorSound }
        };
    }

    /// <summary>
    /// Plays a specific character sound effect.
    /// </summary>
    /// <param name="playerRefId">Reference ID of a player</param>
    /// <param name="playerAction">The Player Action string of the enumeration</param>
    /// <param name="isLoopingAudio"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_PlaySpecificCharatcerSFXAudio(int playerRefId, string playerAction, bool isLoopingAudio)
    {
        sfxAudioSource.loop = isLoopingAudio;
        print("Audio Call " + playerAction);
        sfxAudioSource.PlayOneShot(hostPlayerAudio[playerAction][0]);
    }

    /// <summary>
    /// Plays a universal character sound effect.
    /// </summary>
    /// <param name="playerAction">The Player Action string of the enumeration</param>
    /// <param name="isLoopingAudio"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_PlayUniversalCharatcerSFXAudio(string playerAction, bool isLoopingAudio)
    {
        sfxAudioSource.loop = isLoopingAudio;
        print("Audio Call " + playerAction);
        sfxAudioSource.PlayOneShot(universalPlayerAudio[playerAction]);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_PlayMoveAudio(string playerAction)
    {
        print("Audio Call " + playerAction);
        moveLoopAudioSource.clip = hostPlayerAudio[playerAction][0];
        moveLoopAudioSource.Play(); 
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_StopMoveAudio()
    {
        print("Stop Move Audio Call");
        moveLoopAudioSource.Stop();
    }

    /// <summary>
    /// Stops the audio of all sound effects
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_StopSFXAudio()
    {
        moveLoopAudioSource.Stop();
    }

    /// <summary>
    /// Plays a menu sound effect.
    /// <param name="menuAction">The Menu Action string of the enumeration</param>
    /// </summary>
    public void PlayMenuSFXAudio(string menuAction)
    {
        sfxAudioSource.loop = false;
        print("Audio Call " + menuAction);
        sfxAudioSource.PlayOneShot(menuAudioPlayer[menuAction]);
    }
}
