using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// An enumeration of available player actions of a character.
/// Author(s): Lukasz Bednarek
/// Date: November 22, 2022
/// Remarks: N/A
/// Change History: November 22, 2022 - Lukasz Bednarek
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
    protected GameManager _gameManager;

    [SerializeField] private AudioClip[] attackSoundPool;
    [SerializeField] private AudioClip[] receiveDamageSoundPool;
    [SerializeField] private AudioClip[] moveSoundPool;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip jumpLandSound;
    [SerializeField] private AudioClip dodgeSound;
    private Dictionary<string, AudioClip[]> hostPlayerAudio;
    private Dictionary<string, AudioClip[]> clientPlayerAudio;
    private Dictionary<string, AudioClip> universalPlayerAudio;
    [SerializeField] public AudioSource sfxAudioSource;
    [SerializeField] public AudioSource musicAudioSource;

    /// <summary>
    /// Initializes a game object's properties.
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

    /// <summary>
    /// Stops the audio of all sound effects
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_StopSFXAudio()
    {
        sfxAudioSource.Stop();
    }

    /// <summary>
    /// Plays a menu sound effect.
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_PlayMenuSFXAudio()
    {

    }

}
