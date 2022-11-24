using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Diagnostics;

/// <summary>
/// Class that checks the attack hitbox of a fighter/player.
/// If there is a health component in the hitbox, then damage it.
/// Author(s): Faiz Hassany
/// Date: Nov 07 2022
/// Remarks: AttackArea is enabled in Attack.cs
/// </summary>
public class AttackArea : NetworkBehaviour
{
    public List<Collider2D> overlappingColliders = new List<Collider2D>();

    // Adds to collision list
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check specifically one type of collider instead of adding all
        if (!overlappingColliders.Contains(collider) && (collider.GetType() == typeof(BoxCollider2D)))
        {
           overlappingColliders.Add(collider);
        }
    }

    // Removes from collision list
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (overlappingColliders.Contains(collider) && (collider.GetType() == typeof(BoxCollider2D)))
        {
            overlappingColliders.Remove(collider);
        }
    }

    // Keeps checking every frame
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!overlappingColliders.Contains(collider) && (collider.GetType() == typeof(BoxCollider2D)))
        {
            overlappingColliders.Add(collider);
        }
    }
}
