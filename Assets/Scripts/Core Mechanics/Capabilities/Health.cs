using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
using System.Security.Cryptography;

/// <summary>
/// Class that handles the current / max health of a fighter/player.
/// Author(s): Faiz Hassany, Jason Cheung
/// Date: Nov 07 2022
/// Source(s):
///     FPS Unity & Photon Fusion EP4.1 (player names and RPCs): https://youtu.be/-opvmn_QKw0?t=647
/// Remarks: 
/// - Health value is affected by attack, and it affects stock.
/// - health is a networked property so the NetworkFighterObserver will know of its changes.
/// Change History: Nov 22 2022 - Lukasz Bednarek
/// - Modified health to be a networked property, will call the NetworkFighterObserver to update its UI.
/// - Reorganized code to be more consistent with other capabilities.
/// - Moved some of the logic to Stock.cs
/// - Add logic for RPC call for sound effect method.
/// </summary>
public class Health : NetworkBehaviour
{
    // Fighter prefab components and self reference to character position
    protected Rigidbody2D _body;
    protected NetworkPlayer _networkPlayer;
    protected Vector2 _characterPosition;

    // other scene objects to reference
    protected NetworkFighterObserver _networkFighterObserver;
    protected GameplayAudioManager _audioManager;


    // networked property of the fighter's CurrentHealth; listens for OnChanged and notifies others
    private int _currentHealth = 0;

    [Networked(OnChanged = nameof(OnHealthChanged)), UnityNonSerialized]
    public int CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
            // client update changes to host
            if (Object.HasInputAuthority)
            {
                RPC_SetHealth(value);
            }
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        CacheComponents();
    }


    /// <summary>
    /// Helper method to initialize components attached to self, from its own script or prefab.
    /// </summary>
    private void CacheComponents()
    {
        if (!_body) _body = gameObject.GetComponentInParent<Rigidbody2D>();
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
    /// Post Spawn callback.
    /// Generally used because Network Properties can only be accessed after the object has Spawned.
    /// </summary>
    public override void Spawned()
    {
        // Networked property can only be accessed after object has Spawned
        //CurrentHealth = maxHealth;
    }


    /// <summary>
    /// Damage the player.
    /// </summary>
    /// <param name="amount"></param>
    public void Damage(int amount)
    {
        if (amount < 0)
        {
            // negate the damage amount if negative
            amount = 0;
        }

        if (Object.HasStateAuthority) _audioManager.RPC_PlaySpecificCharatcerSFXAudio(0, PlayerActions.ReceiveDamage.ToString());
        CurrentHealth += amount;

        _networkPlayer.DisableInputsTemporarily(0.5f);
        _networkPlayer.ColorSpriteTemporarily(0.5f, Color.red);
    }


    /// <summary>
    /// Knockback the player.
    /// </summary>
    /// <param name="knockback"></param>
    public void Knockback(Vector2 knockback)
    {
        _characterPosition = _body.position;
        _body.AddForce(knockback, ForceMode2D.Impulse);
    }


    /// <summary>
    /// Heal the player.
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(int amount)
    {
        if (amount < 0)
        {
            // negate the healing amount if negative
            amount = 0;
        }

        CurrentHealth -= amount;
    }


    /// <summary>
    /// Reset the player's health.
    /// </summary>
    public void ResetHealth()
    {
        CurrentHealth = 0;
    }


    /// <summary>
    /// Networked OnChanged method for the Network Property Health
    /// </summary>
    /// <param name="changed"></param>
    static void OnHealthChanged(Changed<Health> changed)
    {
        changed.Behaviour.OnHealthChanged();
    }

    /// <summary>
    /// OnChanged method to update the network fighter status ui
    /// </summary>
    private void OnHealthChanged()
    {
        NetworkFighterObserver.Observer.UpdateFighterStatus();
    }

    /// <summary>
    /// RPC method for client to notify host its changes for CurrentHealth
    /// </summary>
    /// <param name="health"></param>
    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetHealth(int health)
    {
        this.CurrentHealth = health;
    }

}
