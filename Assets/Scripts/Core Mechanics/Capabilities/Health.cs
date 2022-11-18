using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Health : NetworkBehaviour
{
    [SerializeField] private int health = 300;

    private int MAX_HEALTH;
    private Jump jump;

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

    private void Awake()
    {
        MAX_HEALTH = health;
        jump = GetComponent<Jump>();
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
            jump.Respawn();
            this.health = MAX_HEALTH;
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
