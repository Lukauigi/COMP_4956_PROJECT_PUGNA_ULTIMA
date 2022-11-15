using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class AttackArea : MonoBehaviour
{
    private int damage = 50;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("BEFORE ATTACKING BAM BAM");
        if (collider.GetComponent<Health>() != null)
        {
            Debug.Log("ATTACKING BAM BAM");
            Health health = collider.GetComponent<Health>();
            health.Damage(damage);
        }
    }
}
