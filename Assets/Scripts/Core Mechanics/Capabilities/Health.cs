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
    // other scene objects to reference
    protected NetworkFighterObserver _networkFighterObserver;
    protected NetworkPlayer _networkPlayer; //player's local instance
    protected GameObject _audioManager;

    // parent components of character
    private Rigidbody2D rb;
    private Vector2 characterPosition;


    // networked property of the fighter's CurrentHealth; listens for OnChanged and notifies others
    private int currentHealth = 0;
    [Networked(OnChanged = nameof(OnHealthChanged)), UnityNonSerialized]
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            // client update changes to host
            if (Object.HasInputAuthority)
            {
                RPC_SetHealth(value);
            }
        }
    }


    private void Start()
    {
        this._audioManager = GameObject.Find("SceneAudioManager");
        // find and set the parent components of this character
        this.rb = gameObject.GetComponentInParent<Rigidbody2D>();
        if (!_networkPlayer) _networkPlayer = GetComponent<NetworkPlayer>();
    }


    public void knockBack(Vector2 knockback)
    {
        characterPosition = rb.position;
        rb.AddForce(knockback, ForceMode2D.Impulse);
    }




    // Post Spawn callback
    public override void Spawned()
    {
        // Networked property can only be accessed after object has Spawned
        //CurrentHealth = maxHealth;
    }

    // Method to damage the player
    public void Damage(int amount)
    {
        if (amount < 0)
        {
            // negate the damage amount if negative
            amount = 0;
        }

        if (Object.HasStateAuthority) _audioManager.GetComponent<GameplayAudioManager>().RPC_PlaySpecificCharatcerSFXAudio(0, PlayerActions.ReceiveDamage.ToString());
        CurrentHealth += amount;

        _networkPlayer.DisableInputsTemporarily(0.5f);
        _networkPlayer.ColorSpriteTemporarily(0.5f, Color.red);
    }


    // Method to heal the player
    public void Heal(int amount)
    {
        if (amount < 0)
        {
            // negate the healing amount if negative
            amount = 0;
        }

        CurrentHealth -= amount;
    }

    // Method to reset the Health of the player
    public void ResetHealth()
    {
        CurrentHealth = 0;
    }

    // Networked OnChanged method for the Network Property Stocks
    static void OnHealthChanged(Changed<Health> changed)
    {
        changed.Behaviour.OnHealthChanged();
    }

    // OnChanged method to update the network fighter status ui
    private void OnHealthChanged()
    {
        NetworkFighterObserver.Observer.UpdateFighterStatus();
    }

    // RPC method for client to notify host its changes for CurrentHealth
    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetHealth(int health)
    {
        this.CurrentHealth = health;
    }

}
