using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles the attack of a fighter/player.
/// Author(s): Faiz Hassany
/// Date: Nov 07 2022
/// Remarks: Attack uses AttackArea gameobject, which is in the fighter prefab hierarchy.
/// Change History: Nov 18 2022 - Jason Cheung
/// - Renamed some methods to be more consistent with other capabilities.
/// </summary>
public class Attack : NetworkBehaviour
{
    // fighter prefab components
    protected Animator _animator; //player's animator controller
    protected NetworkMecanimAnimator _networkMecanimAnimator; // networked animator controller

    //public Collider2D[] attackHitboxes;

    // AttackArea component, which is inside player prefab hierarchy
    protected GameObject _attackArea;

    private bool isAttackPressed;

    private bool isAttacking = false;
    private float attackRate = 1f;
    private float timer = 0f;

    // reference the animator controller for player
    //public Animator animator;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        CacheComponents();
    }

    // Helper method to initialize fighter prefab components
    private void CacheComponents()
    {
        if (!_animator) _animator = GetComponent<Animator>();
        //if (!_networkMecanimAnimator) _networkMecanimAnimator = GetComponent<NetworkMecanimAnimator>();

        // cache AttackArea gameObject, which is inside Fighter prefab hierarchy
        if (!_attackArea) _attackArea = transform.Find("AttackArea").gameObject;
    }

    //private void OnDrawGizmosSelected()
    //{

    //}

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
            isAttackPressed = false;
            AttackAction();
        }

        if (isAttacking)
        {
            timer += Runner.DeltaTime;

            if (timer >= attackRate)
            {
                timer = 0;
                isAttacking = false;
                _attackArea.SetActive(isAttacking);

                // signal to stop attacking 
                _animator.SetBool("isAttacking", false);
            }
        }
    }

    // Perform attack action
    private void AttackAction()
    {
        // perform attack only if fighter is not currenty attacking
        if (!isAttacking)
        {
            isAttacking = true;
            _attackArea.SetActive(isAttacking);
            Debug.Log("Player Attacked! Enabling AttackArea hitbox!");

            // signal to attack
            _animator.SetBool("isAttacking", true);
        }
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
