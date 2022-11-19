using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// Class that checks the attack hitbox of a fighter/player.
/// If there is a health component in the hitbox, then damage it.
/// Author(s): Faiz Hassany
/// Date: Nov 07 2022
/// Remarks: AttackArea is enabled in Attack.cs
/// </summary>
public class AttackArea : NetworkBehaviour
{
    private int damage = 50;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Health>() != null)
        {
            Debug.Log("AttackArea hitbox found something to damage...");
            Health health = collider.GetComponent<Health>();
            health.Damage(damage);
        }
    }
}
