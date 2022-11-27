using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// An enumeration of available player actions of a character.
/// Author(s): Lukasz Bednarek
/// Date: November 23, 2022
/// Remarks: N/A
/// Change History: November 23, 2022 - Lukasz Bednarek
/// - Add enumeration
/// - Add documentation
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
/// An enumeration of user menu iteraction types.
/// Author(s): Lukasz Bednarek
/// Date: November 23, 2022
/// Change History: November 26, 2022 - Lukasz Bednarek
/// - Add Waiting enum item.
/// </summary>
public enum MenuActions
{
    Navigate,
    Confirm,
    Revert,
    Error,
    Login,
    Waiting
}

/// <summary>
/// An audio manager GameObject for the gameplay battle scene.
/// Author(s): Lukasz Bednarek
/// Date: November 23, 2022
/// Remarks: Functionality relating to move is present but not functioning properly.
/// Change History: November 25, 2022 - Lukasz Bednarek
/// - Add class
/// - Add documentation
/// - Add random usage of sound effects in sound pool.
/// - Edit method headers and add method documentation.
/// - Edit Move audio logic.
/// </summary>
public class GameplayAudioManager : NetworkBehaviour
{
    public static GameplayAudioManager Instance = null;

    // sound pools for actions with multiple audio clips
    [SerializeField] private AudioClip[] _attackSoundPool;
    [SerializeField] private AudioClip[] _receiveDamageSoundPool;
    [SerializeField] private AudioClip[] _moveSoundPool;

    // universally-shared character audio clips
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _jumpLandSound;
    [SerializeField] private AudioClip _dodgeSound;

    // menu interaction audio clips
    [SerializeField] private AudioClip _navigateSound;
    [SerializeField] private AudioClip _confirmSound;
    [SerializeField] private AudioClip _revertSound;
    [SerializeField] private AudioClip _errorSound;

    // categorical data structures for certain audio effects
    private Dictionary<string, AudioClip[]> _hostPlayerAudio;
    private Dictionary<string, AudioClip[]> _clientPlayerAudio;
    private Dictionary<string, AudioClip> _universalPlayerAudio;
    private Dictionary<string, AudioClip> _menuAudioPlayer;

    // audio sources, can be thought of as audio channels
    [SerializeField] private AudioSource _sfxAudioSource;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _player1MoveLoopAudioSource;
    [SerializeField] private AudioSource _player2MoveLoopAudioSource;

    private NetworkId[] _playerIds = new NetworkId[2];

    /// <summary>
    /// Initializes a game object's components. Ideal section to initialize instance data of game object.
    /// </summary>
    private void Awake()
    {
        Instance = this;

        _hostPlayerAudio = new Dictionary<string, AudioClip[]> {
            { PlayerActions.Attack.ToString(), _attackSoundPool },
            { PlayerActions.ReceiveDamage.ToString(), _receiveDamageSoundPool },
            { PlayerActions.Move.ToString(), _moveSoundPool }
        };

        _clientPlayerAudio = new Dictionary<string, AudioClip[]> {
            { PlayerActions.Attack.ToString(), _attackSoundPool },
            { PlayerActions.ReceiveDamage.ToString(), _receiveDamageSoundPool },
            { PlayerActions.Move.ToString(), _moveSoundPool }
        };

        _universalPlayerAudio = new Dictionary<string, AudioClip>
        {
            { PlayerActions.Death.ToString(), _deathSound },
            { PlayerActions.Dodge.ToString(), _dodgeSound },
            { PlayerActions.Jump.ToString(), _jumpSound },
            { PlayerActions.JumpLand.ToString(), _jumpLandSound }
        };

        _menuAudioPlayer = new Dictionary<string, AudioClip>
        {
            { MenuActions.Navigate.ToString(), _navigateSound },
            { MenuActions.Confirm.ToString(), _confirmSound },
            { MenuActions.Revert.ToString(), _revertSound },
            { MenuActions.Error.ToString(), _errorSound }
        };
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_SetPlayerIds(NetworkId playerOne, NetworkId playerTwo)
    {
        print("player ids: " + playerOne + ", " + playerTwo);
        _playerIds[0] = playerOne;
        _playerIds[1] = playerTwo;
        
    }

    /// <summary>
    /// Plays a specific character sound effect.
    /// </summary>
    /// <param name="playerRefId">Reference ID of a player</param>
    /// <param name="playerAction">The Player Action string of the enumeration</param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_PlaySpecificCharatcerSFXAudio(int playerRefId, string playerAction)
    {
        print("Audio Call " + playerAction);

        // Get random audio clip from sound pool
        int soundPoolLength = _hostPlayerAudio[playerAction].Length;
        int clipIndex = Random.Range(0, soundPoolLength);

        _sfxAudioSource.PlayOneShot(_hostPlayerAudio[playerAction][clipIndex]);
    }

    /// <summary>
    /// Plays a universal character sound effect.
    /// </summary>
    /// <param name="playerAction">The Player Action string of the enumeration</param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_PlayUniversalCharatcerSFXAudio(string playerAction)
    {
        print("Audio Call " + playerAction);
        _sfxAudioSource.PlayOneShot(_universalPlayerAudio[playerAction]);
    }

    /// <summary>
    /// Plays character ground movement audio.
    /// 
    /// This audio will loop until RPC_StopMoveAudio is called.
    /// </summary>
    /// <param name="playerAction">The Player Action string of the enumeration</param>
    /// <param name="playerId">A player's unique identifier on the network</param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_PlayMoveAudio(string playerAction, NetworkId playerId)
    {
        print("Audio Call " + playerAction + " id: " + playerId);
        if (playerId == _playerIds[0])
        {
            _player1MoveLoopAudioSource.clip = _hostPlayerAudio[playerAction][0];
            _player1MoveLoopAudioSource.Play();
        }
        else if (playerId == _playerIds[1])
        {
            _player2MoveLoopAudioSource.clip = _hostPlayerAudio[playerAction][0];
            _player2MoveLoopAudioSource.Play();
        }
    }

    /// <summary>
    /// Stops character ground movement audio.
    /// <param name="playerId">A player's unique identifier on the network</param>
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_StopMoveAudio(NetworkId playerId)
    {
        print("Stop Move Audio Call, id: " + playerId);
        if (playerId == _playerIds[0])
        {
            _player1MoveLoopAudioSource.Stop();
            _player1MoveLoopAudioSource.clip = null;
        }
        else if (playerId == _playerIds[1])
        {
            _player2MoveLoopAudioSource.Stop();
            _player2MoveLoopAudioSource.clip = null;
        }
    }

    /// <summary>
    /// Stops the audio of all sound effects
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_StopSFXAudio()
    {
        _player1MoveLoopAudioSource.Stop();
    }

    /// <summary>
    /// Plays a menu sound effect.
    /// <param name="menuAction">The Menu Action string of the enumeration</param>
    /// </summary>
    public void PlayMenuSFXAudio(string menuAction)
    {
        print("Audio Call " + menuAction);
        _sfxAudioSource.PlayOneShot(_menuAudioPlayer[menuAction]);
    }
}
