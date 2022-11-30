using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

/// <summary>
/// Handles some network functionalities on the player side.
/// Author(s): Jun Earl Solomon, Jason Cheung
/// Date: Oct 29 2022
/// Source(s):
///     Online multiplayer with Photon Fusion - 2D Arcade Style Car Controller in Unity tutorial Part 12: https://youtu.be/yrXQSM1cleU
///     Online multiplayer FPS Unity & Photon Fusion EP1 (Fusion setup + movement): https://youtu.be/hqIZCoLHOig
///     Online multiplayer FPS Unity & Photon Fusion EP4.1 (player names and RPCs): https://youtu.be/-opvmn_QKw0
/// Change History: Nov 28 2022 - Jason Cheung
/// - added more utility methods for different cases of disabling player inputs
/// - moved DisableInputs and ColorSprite utility methods to this class
/// - added networked property nickname that's above the fighter prefab's head
/// </summary>
public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    // our local player
    public static NetworkPlayer Local { get; set; }

    // fighter components
    protected Attack _attack;
    protected Jump _jump;
    protected Move _move;
    protected Dodge _dodge;
    protected Rigidbody2D _body;

    // other scene objects to reference
    protected GameplayAudioManager _audioManager;

    // fighter prefab UI components
    [SerializeField] private TextMeshProUGUI _playerNickname;

    // networked property of the fighter's nickname; listens for OnChanged and notifies others
    private NetworkString<_16> nickName;
    [Networked(OnChanged = nameof(OnNickNameChanged)), UnityNonSerialized]
    public NetworkString<_16> NickName
    {
        get
        {
            return nickName;
        }
        set
        {
            nickName = value;
            // client update changes to host
            if (Object.HasInputAuthority)
            {
                RPC_SetNickName(value);
            }
        }
    }


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (!_attack) _attack = gameObject.GetComponentInParent<Attack>();
        if (!_jump) _jump = gameObject.GetComponentInParent<Jump>();
        if (!_move) _move = gameObject.GetComponentInParent<Move>();
        if (!_dodge) _dodge = gameObject.GetComponentInParent<Dodge>();
        if (!_body) _body = gameObject.GetComponentInParent<Rigidbody2D>();
    }

    /// <summary>
    /// Start is called after Awake, and before Update.
    /// Generally used to reference other scene objects, after they have all been initialized.
    /// </summary>
    private void Start()
    {
        // cache other scene objects
        if (!_audioManager) _audioManager = GameObject.Find("SceneAudioManager").GetComponent<GameplayAudioManager>();
    }


    /// <summary>
    /// gives input authority to the local player
    /// </summary>
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            Debug.Log("Spawned own character");
        }
        else
        {
            Debug.Log("Spawned other player's character");
        }
    }


    /// <summary>
    /// despawn the object when it has left the game
    /// </summary>
    /// <param name="player">The player</param>
    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }


    /// <summary>
    /// Networked OnChanged method for the Network Property Nickname
    /// </summary>
    /// <param name="changed"></param>
    static void OnNickNameChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.OnNickNameChanged();
    }

    /// <summary>
    /// OnChanged method to update the nickname displayed on top of their fighter
    /// </summary>
    private void OnNickNameChanged()
    {
        Debug.Log($"Nickname changed to {NickName} for player ref: ->");
        _playerNickname.text = NickName.ToString();
    }


    /// <summary>
    /// RPC method for client to notify host its changes for fighter nickname
    /// </summary>
    /// <param name="nickName"></param>
    /// <param name="info"></param>
    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetNickName(NetworkString<_16> nickName, RpcInfo info = default)
    {
        Debug.Log($"[RPC] SetNickName {NickName}");
        this.NickName = nickName;
    }

    /// <summary>
    /// Utility method to Disable Action Inputs for a Temporary duration.
    /// Action inputs that can be disabled are Attack and Dodge.
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="disableAttack"></param>
    /// <param name="disableDodge"></param>
    public void DisableActionInputsTemporarily(float seconds, bool disableAttack, bool disableDodge)
    {
        StartCoroutine(OnDisableActionInputsTemporarily(seconds, disableAttack, disableDodge));
    }

    /// <summary>
    /// Co-Routine to Disable Action Inputs Temporarily.
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="disableAttack"></param>
    /// <param name="disableDodge"></param>
    /// <returns></returns>
    IEnumerator OnDisableActionInputsTemporarily(float seconds, bool disableAttack, bool disableDodge)
    {
        if (disableAttack)
            _attack.enabled = false;

        if (disableDodge)
            _dodge.enabled = false;

        yield return new WaitForSeconds(seconds);

        if (disableAttack)
            _attack.enabled = true;

        if (disableDodge)
            _dodge.enabled = true;
    }

    /// <summary>
    /// Utility method to disable player input temporarily.
    /// This starts a co-routine to sleep all input-related components for the passed duration.
    /// </summary>
    /// <param name="seconds"></param>
    public void DisableInputsTemporarily(float seconds)
    {
        StartCoroutine(OnDisableInputsTemporarily(seconds));
    }


    /// <summary>
    /// Co-Routine to Disable All Character Inputs Temporarily.
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    IEnumerator OnDisableInputsTemporarily(float seconds)
    {
        _attack.enabled = false;
        _jump.enabled = false;
        _move.enabled = false;
        _dodge.enabled = false;
        yield return new WaitForSeconds(seconds);
        _attack.enabled = true;
        _jump.enabled = true;
        _move.enabled = true;
        _dodge.enabled = true;
    }

    /// <summary>
    /// Utility method to disable player input temporarily and slow down the player rigidbody.
    /// This starts a co-routine to sleep all input-related components for the passed duration.
    /// Then, dramatically slow down player movement.
    /// </summary>
    public void DisableInputsAndSlowPlayer()
    {
        // disable inputs
        _attack.enabled = false;
        _jump.enabled = false;
        _move.enabled = false;
        _dodge.enabled = false;

        // stop & slow functions of the player
        _audioManager.RPC_StopSFXAudio(); // stop player sfx audio
        _body.velocity = _body.velocity / 50; // slow down rigidbody velocity
        _body.gravityScale = _body.gravityScale / 50;
    }


    /// <summary>
    /// Utility method to color player sprite temporarily.
    /// This starts a co-routine to color the sprite for the passed duration, and then reset it back to white.
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="color"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_ColorSpriteTemporarily(float seconds, Color color)
    {
        StartCoroutine(OnColorSpriteTemporarily(seconds, color));
    }

    /// <summary>
    /// Co-Routine to color sprite renderer temporarily, as a visual effect
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    IEnumerator OnColorSpriteTemporarily(float seconds, Color color)
    {
        GetComponent<Renderer>().material.color = color;
        yield return new WaitForSeconds(seconds);
        GetComponent<Renderer>().material.color = Color.white;
    }

}