using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// Class that handles the current / max health of a fighter/player.
/// Author(s): Faiz Hassany, Jason Cheung
/// Date: Nov 07 2022
/// Remarks: Attack affects player health. Also, fighter respawn on losing all their health is in Stock.cs
/// Change History: Nov 18 2022 - Jason Cheung
/// - Reorganized code to be more consistent with other capabilities.
/// - Moved some of the logic to Stock.cs
/// </summary>
public class Health : NetworkBehaviour
{
    [SerializeField] private int health = 300;
    public int CurrentHealth => health; //getter


    private int MAX_HEALTH;


    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        MAX_HEALTH = health;
    }

    // Method to damage the player
    public void Damage(int amount)
    {
        if (amount < 0)
        {
            // negate the damage amount if negative
            amount = 0;
        }

        health -= amount;

        Debug.Log("Player Lost Health! HEALTH LEFT = " + health);

    }

    // Method to heal the player
    public void Heal(int amount)
    {
        if (amount < 0)
        {
            // negate the healing amount if negative
            amount = 0;
        }

        bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;

        if (wouldBeOverMaxHealth)
        {
            health = MAX_HEALTH;
        }
        else
        {
            health += amount;
        }
    }

    // Method to reset the Health of the player
    public void ResetHealth()
    {
        health = MAX_HEALTH;
    }

}
