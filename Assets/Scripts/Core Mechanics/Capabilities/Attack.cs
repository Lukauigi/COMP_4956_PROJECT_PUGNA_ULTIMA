using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Class that handles the attack of a fighter/player.
/// Author(s): Faiz Hassany, John Ryue
/// Date: Nov 07 2022
/// Remarks: Attack uses AttackArea gameobject, which is in the fighter prefab hierarchy.
/// Change History: Nov 22 2022 - Lukasz Bednarek
/// - integrated Jaspers' animations using Animator controller and set triggers
/// - Renamed some methods to be more consistent with other capabilities.
/// - Add logic for RPC call for sound effect method.
/// </summary>
public class Attack : NetworkBehaviour
{
    // fighter prefab components
    protected Animator _animator; //player's animator controller

    // AttackArea component, which is inside player prefab hierarchy
    protected AttackArea _attackArea;

    [SerializeField] private int damage = 50;
    private GameObject audioManager;

    private bool isAttackPressed;

    private bool isAttacking = false;
    private float attackRate = 1f;
    private float timer = 0f;

    // for database - the amount of damage done by the player
    [UnityNonSerialized] public int DamageDone { get; set; } = 0;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        CacheComponents();
    }

    // Start is called after Awake, and before Update
    private void Start()
    {
        this.audioManager = GameObject.Find("SceneAudioManager");
    }

    // Helper method to initialize fighter prefab components
    private void CacheComponents()
    {
        if (!_animator) _animator = GetComponent<Animator>();

        // cache AttackArea gameObject, which is inside Fighter prefab hierarchy
        if (!_attackArea) _attackArea = transform.Find("AttackArea").gameObject.GetComponent<AttackArea>();
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

            // signal to attack
            _animator.SetBool("isAttacking", true);
            if (Object.HasStateAuthority) audioManager.GetComponent<GameplayAudioManager>().RPC_PlaySpecificCharatcerSFXAudio(0, PlayerActions.Attack.ToString());


            print("Player Attacked! finding objects to hit in AttackArea hitbox...");

            List<Collider2D> overlappingColliders = _attackArea.overlappingColliders;
            foreach (Collider2D collider in overlappingColliders)
            {
                if (collider.GetComponent<Health>() != null && isAttacking == true)
                {
                    print("Attack hit!");
                    Health health = collider.GetComponent<Health>();
                    health.Damage(damage);
                    DamageDone += damage;
                }
            }

        }
    }
}
