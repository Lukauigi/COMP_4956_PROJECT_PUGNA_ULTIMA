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
    // list of other colliders in the AttackArea collider
    private List<Collider2D> _overlappingColliders = new List<Collider2D>();
    public List<Collider2D> OverlappingColliders => _overlappingColliders; // getter
    
    
    /// <summary>
    /// Triggers when another collider makes contact with the AttackArea collider
    /// <param name="collider"></param>
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check specifically one type of collider instead of adding all
        if (collider.GetType() != typeof(BoxCollider2D))
            return;

        if (!_overlappingColliders.Contains(collider))
        {
            _overlappingColliders.Add(collider);
        }
    }

    /// <summary>
    /// Triggers when the collider leaves the AttackArea collider
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetType() != typeof(BoxCollider2D))
            return;

        if (_overlappingColliders.Contains(collider))
        {
            _overlappingColliders.Remove(collider);
        }
    }

    /// <summary>
    /// Triggers when another collider stays inside the AttackArea collider
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.GetType() != typeof(BoxCollider2D))
            return;

        if (!_overlappingColliders.Contains(collider))
        {
            _overlappingColliders.Add(collider);
        }
    }
}
