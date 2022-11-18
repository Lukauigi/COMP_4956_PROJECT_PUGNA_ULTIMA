using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Health : NetworkBehaviour
{
    [SerializeField] private int health = 100;

    private int MAX_HEALTH = 100;

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetButtonDown("D"))
        {
            // Damage(10);
        }

        if (Input.GetButtonDown("H"))
        {
            // Heal(10);
        }*/
    }

    public void Damage(int amount)
    {
        if (amount < 0)
        {
            //throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
            // should not throw exceptions; rather, negate the healing amount
            amount = 0;
        }

        this.health -= amount;

        Debug.Log("Health.Damage() triggered : HEALTH LEFT = " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            //throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
            // should not throw exceptions; rather, negate the healing amount
            amount = 0;
        }

        bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;

        if (wouldBeOverMaxHealth)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            this.health += amount;
        }
    }

    private void Die()
    {
        Debug.Log("I am Dead!");
        Destroy(gameObject);
    }
}
