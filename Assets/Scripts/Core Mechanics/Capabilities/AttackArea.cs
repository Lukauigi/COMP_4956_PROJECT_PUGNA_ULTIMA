using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class AttackArea : NetworkBehaviour
{
    private int damage = 50;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("BEFORE ATTACKING BAM BAM");
        if (collider.GetComponent<Health>() != null)
        {
            Debug.Log("AttackArea : Collider2D health not null & triggered!");
            Health health = collider.GetComponent<Health>();
            health.Damage(damage);
        }
    }
}
