using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// Class that handles the dodge of a fighter/player.
/// Author(s): Richard Mac
/// Date: Nov 18 2022
/// Change History: Nov 22 2022 - Lukasz Bednarek
/// - integrated Jaspers' animations using Animator controller and set triggers
/// - Add logic for RPC call for sound effect method.
/// Change History: November 24 2022 - Richard Mac
/// - implemented on dodge colour change
/// - implemented a cooldown for the dodge mechanic 
/// </summary>
public class Dodge : NetworkBehaviour
{
    // Fighter prefab components
    protected Rigidbody2D _body;
    protected Collider2D _playerHitbox;
    protected Animator _animator;
    protected NetworkPlayer _networkPlayer;

    // Other scene objects to reference
    protected GameplayAudioManager _audioManager;

    // If the dodge input key was pressed
    private bool isDodgePressed;

    // Time before next dodge
    private float _dodgeRate = 1f;
    private float _nextDodgeTime = 0f;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        CacheComponents();
    }

    /// <summary>
    /// Helper method to initialize components attached to self, from its own script or prefab.
    /// </summary>
    private void CacheComponents()
    {
        if (!_body) _body = GetComponent<Rigidbody2D>();
        if (!_playerHitbox) _playerHitbox = GetComponent<Collider2D>();
        if (!_animator) _animator = GetComponent<Animator>();
        if (!_networkPlayer) _networkPlayer = GetComponent<NetworkPlayer>();
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
    /// FixedUpdateNetwork is called once per frame. This is Fusion's Update() method.
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        // checking for input presses
        if (GetInput(out NetworkInputData data))
        {
            isDodgePressed |= data.dodge;
           
        }

        if (isDodgePressed)
        {
            isDodgePressed = false;
            if (Time.time > _nextDodgeTime)
            {
                StartCoroutine(DodgeAction());
                _nextDodgeTime = Time.time + _dodgeRate;
            }
           
        }
    }

    /// <summary>
    /// Co-Routine to perform the Dodge Action.
    /// </summary>
    private IEnumerator DodgeAction()
    {
        // beginning section - stop inputs and play animation
        _networkPlayer.DisableActionInputsTemporarily(0.7f, disableAttack: true, disableDodge: true);
        _animator.SetBool("isDodging", true);
        yield return new WaitForSeconds(0.1f);  // time till next section

        // middle section - disable hitbox (invincible), show dodge color effect, and play sound
        Debug.Log("hitbox down");
        _playerHitbox.enabled = false;
        _networkPlayer.ColorSpriteTemporarily(0.5f, Color.gray);
        if (Object.HasStateAuthority) _audioManager.RPC_PlayUniversalCharacterSFXAudio(PlayerActions.Dodge.ToString());
        yield return new WaitForSeconds(0.5f);  // time till next section

        // ending section - reenable hitbox (not invincible) and stop animation
        Debug.Log("hitbox back");
        _playerHitbox.enabled = true;
        _animator.SetBool("isDodging", false);

    }

}
