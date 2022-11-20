using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// Class that handles the current / max health of a fighter/player.
/// Author(s): Faiz Hassany, Jason Cheung
/// Date: Nov 07 2022
/// Source(s):
///     FPS Unity & Photon Fusion EP4.1 (player names and RPCs): https://youtu.be/-opvmn_QKw0?t=647
/// Remarks: 
/// - Health value is affected by attack, and it affects stock.
/// - health is a networked property so the NetworkFighterObserver will know of its changes.
/// Change History: Nov 19 2022 - Jason Cheung
/// - Modified health to be a networked property, will call the NetworkFighterObserver to update its UI.
/// - Reorganized code to be more consistent with other capabilities.
/// - Moved some of the logic to Stock.cs
/// </summary>
public class Health : NetworkBehaviour
{
    // other game object references
    protected NetworkFighterObserver _networkFighterObserver;

    // how much health the fighter has per stock
    [SerializeField] private int maxHealth = 300;

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


    // Start is called after Awake, and before Update
    private void Start()
    {
        // Networked property has to be set after Awake and all other objects have been initialized
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

        CurrentHealth -= amount;


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

    static void OnHealthChanged(Changed<Health> changed)
    {
        changed.Behaviour.OnHealthChanged();
    }

    private void OnHealthChanged()
    {
        // update the fighter status ui
        NetworkFighterObserver.Observer.RPC_UpdateFighterStatusUI();
    }

    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetHealth(int health, RpcInfo info = default)
    {
        this.CurrentHealth = health;
    }


}
