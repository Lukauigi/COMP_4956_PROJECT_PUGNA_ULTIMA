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

    // how much health the fighter has per stock
    [SerializeField] private int maxHealth = 300;
    private GameObject audioManager;

    // parent components of character
    private Attack attack;
    private Jump jump;
    private Move move;
    private Rigidbody2D rb;
    private Vector2 characterPosition;

    private void Start()
    {
        this.audioManager = GameObject.Find("SceneAudioManager");
        // find and set the parent components of this character
        this.attack = gameObject.GetComponentInParent<Attack>();
        this.jump = gameObject.GetComponentInParent<Jump>();
        this.move = gameObject.GetComponentInParent<Move>();
        this.rb = gameObject.GetComponentInParent<Rigidbody2D>();
    }

    // disable character inputs relating to these components temporarily
    IEnumerator disableInputsTemporarily()
    {
        GameObject player = gameObject;
        player.GetComponent<Renderer>().material.color = Color.red;
        attack.enabled = false;
        jump.enabled = false;
        move.enabled = false;
        yield return new WaitForSeconds(0.5f);
        attack.enabled = true;
        jump.enabled = true;
        move.enabled = true;
        player.GetComponent<Renderer>().material.color = Color.white;
    }

    public void knockBack(Vector2 knockback)
    {
        characterPosition = rb.position;
        rb.AddForce(knockback, ForceMode2D.Impulse);
    }

    // networked property of the fighter's CurrentHealth; listens for OnChanged and notifies others
    private int currentHealth;
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


    // Post Spawn callback
    public override void Spawned()
    {
        // Networked property can only be accessed after object has Spawned
        CurrentHealth = maxHealth;
    }

    // Method to damage the player
    public void Damage(int amount)
    {
        if (amount < 0)
        {
            // negate the damage amount if negative
            amount = 0;
        }

        if (Object.HasStateAuthority) audioManager.GetComponent<GameplayAudioManager>().RPC_PlaySpecificCharatcerSFXAudio(0, PlayerActions.ReceiveDamage.ToString());
        CurrentHealth -= amount;
        StartCoroutine(disableInputsTemporarily());
    }

    // Method to heal the player
    public void Heal(int amount)
    {
        if (amount < 0)
        {
            // negate the healing amount if negative
            amount = 0;
        }


        bool wouldBeOverMaxHealth = CurrentHealth + amount > maxHealth;

        if (wouldBeOverMaxHealth)
        {
            CurrentHealth = maxHealth;
        }
        else
        {
            CurrentHealth += amount;
        }

    }

    // Method to reset the Health of the player
    public void ResetHealth()
    {
        CurrentHealth = maxHealth;
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
    public void RPC_SetHealth(int health, RpcInfo info = default)
    {
        this.CurrentHealth = health;
    }
}
