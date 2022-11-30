using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Class that handles the attack of a fighter/player.
/// Author(s): Faiz Hassany, John Ryue, Matthew Kan, Jason Cheung
/// Date: Nov 07 2022
/// Remarks: Attack uses AttackArea gameobject, which is in the fighter prefab hierarchy.
/// Change History: Nov 24 2022 - Matthew Kan
/// - Added knockback to the attack
/// - integrated Jaspers' animations using Animator controller and set triggers
/// - Renamed some methods to be more consistent with other capabilities.
/// - Add logic for RPC call for sound effect method.
/// </summary>
public class Attack : NetworkBehaviour
{
    // Fighter prefab components
    protected Animator _animator;
    protected NetworkPlayer _networkPlayer;
    // AttackArea component, which is inside player prefab hierarchy
    protected AttackArea _attackArea;

    // Other scene objects to reference
    protected GameplayAudioManager _audioManager;

    // Damage of the attack
    [SerializeField] private int _damage = 50;

    // If the attack input key was pressed
    private bool _isAttackPressed;

    // If the attack is already being performed
    private bool _isAttacking = false;

    // Time before next attack
    private float _attackRate = 0.75f;
    private float _timer = 0f;

    // knockback values, diff strengths are used depending on health
    private Vector2 _lowKnockback = new Vector2(3, 1);
    private Vector2 _medKnockback = new Vector2(6, 3);
    private Vector2 _highKnockback = new Vector2(10, 5);


    // The total amount of damage done by the player - For Database
    private int _damageDone = 0;
    public int DamageDone => _damageDone; // getter


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        CacheComponents();
    }


    /// <summary>
    /// Helper method to initialize components attached to self, from its own script or prefab.
    /// </summary>
    private void CacheComponents()
    {
        if (!_animator) _animator = GetComponent<Animator>();
        if (!_networkPlayer) _networkPlayer = GetComponent<NetworkPlayer>();

        // cache AttackArea gameObject, which is inside Fighter prefab hierarchy
        if (!_attackArea) _attackArea = transform.Find("AttackArea").gameObject.GetComponent<AttackArea>();
    }


    /// <summary>
    /// Start is called after Awake, and before Update.
    /// Generally used to reference other scene objects, after they have all been initialized.
    /// </summary>
    private void Start()
    {
        // cache other scene objects
        if (!_audioManager) _audioManager = GameObject.Find("SceneAudioManager").GetComponent<GameplayAudioManager>();
    }


    /// <summary>
    /// FixedUpdateNetwork is called once per frame. This is Fusion's Update() method.
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        // checking for input presses
        if (GetInput(out NetworkInputData data))
        {
            _isAttackPressed |= data.attack;
        }

        // if attack input key was pressed, perform the attack
        if (_isAttackPressed)
        {
            _isAttackPressed = false;
            AttackAction();
        }

        // if player is still attacking, run timer to check when to stop it
        if (_isAttacking)
        {
            _timer += Runner.DeltaTime;

            if (_timer >= _attackRate)
            {
                _timer = 0;
                _isAttacking = false;

                // stop animation
                _animator.SetBool("isAttacking", false);
            }
        }
    }


    /// <summary>
    /// Perform the Attack Action.
    /// </summary>
    protected void AttackAction()
    {
        // perform attack only if fighter is not currenty attacking
        if (!_isAttacking)
        {
            _isAttacking = true;

            // disable dodge input while attacking
            _networkPlayer.DisableActionInputsTemporarily(_attackRate, disableAttack: false, disableDodge: true);

            // play animation and sound
            _animator.SetBool("isAttacking", true);
            if (Object.HasStateAuthority) _audioManager.RPC_PlaySpecificCharatcerSFXAudio(0, PlayerActions.Attack.ToString());


            // check for hit objects and inflict damage if they are a player
            List<Collider2D> overlappingColliders = _attackArea.OverlappingColliders;
            foreach (Collider2D collider in overlappingColliders)
            {
                Health health = collider.GetComponent<Health>();
                if (health == null)
                    continue;

                print("Attack hit!");
                health.Knockback(CalculateKnockback(collider, health));
                health.Damage(_damage);
                RPC_AddDamageDone(_damage);
            }
        }
    }


    /// <summary>
    /// Calculate Knockback power and diection.
    /// Knockback is calculated based on its health and the relative position of collider.
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="health"></param>
    /// <returns></returns>
    protected Vector2 CalculateKnockback(Collider2D collider, Health health)
    {
        Vector2 knockbackStrength;

        // check health to determine knockback power
        if (health.CurrentHealth <= 150) knockbackStrength = _lowKnockback;
        else if (health.CurrentHealth <= 300) knockbackStrength = _medKnockback;
        else knockbackStrength = _highKnockback;

        // compare relative position of colliders
        // default knockback values are to the right and upwards

        // check if knockback should be to the left
        if (collider.transform.position.x < transform.position.x)
            knockbackStrength.x *= -1;

        // check if knockback should be downwards
        if (collider.transform.position.y > transform.position.y)
            knockbackStrength.y *= -1;

        return knockbackStrength;
    }


    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    protected void RPC_AddDamageDone(int damage)
    {
        _damageDone += damage;
        print("Updated damage done: " + _damageDone + " ; Object reference:" + Object.InputAuthority);
    }

}
