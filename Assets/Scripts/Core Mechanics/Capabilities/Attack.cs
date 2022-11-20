using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : NetworkBehaviour
{

    //public Collider2D[] attackHitboxes;

    private GameObject attackArea;

    private bool isAttackPressed;

    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        attackArea = transform.Find("AttackArea").gameObject;
    }

    private void OnDrawGizmosSelected()
    {

    }

    // FixedUpdateNetwork is called once per frame; this is Fusion's Update() method
    public override void FixedUpdateNetwork()
    {
        //if (GameManager.instance.GameState != GameStates.running)
        //    return;

        // checking for input presses
        if (GetInput(out NetworkInputData data))
        {
            isAttackPressed |= data.attack;
        }

        if (isAttackPressed)
        {
            Debug.Log("Attack.isAttackPressed : Attack Key (G) pressed!");
            isAttackPressed = false;
            iAttack();
        }

        if (attacking)
        {
            timer += Runner.DeltaTime;

            if (timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
            }
        }
    }

    //nov 7
    private void iAttack()
    {
        attacking = true;
        attackArea.SetActive(attacking);
        Debug.Log("Attack.iAttack() : Enabling AttackArea hitbox!");
    }

/*    private void LaunchAttack(Collider2D col)
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(col.bounds.center, col.bounds.extents, col.transform.rotation.x, LayerMask.GetMask("Hitbox"));
        foreach (Collider2D c in cols)
        {
            //float damage = 1;
            //Debug.Log("Hit Registered"); //Test if attack is going through

            if (c.transform.parent.parent == transform) // Check if attack hitbox is colliding with the player that used the attack
                continue;                               // If so do not register a hit and continue foreach loop

            Debug.Log(c.name); //If this shows the attack is hitting another player


        }
    }*/

    /*    public override void FixedUpdateNetwork()
        {

        }*/

}
