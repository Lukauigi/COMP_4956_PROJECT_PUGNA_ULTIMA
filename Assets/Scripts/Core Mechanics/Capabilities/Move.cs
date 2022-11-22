using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles all the movement (horizontal) of a fighter/player
/// Author(s): Jun Earl Solomon
/// Date: Oct 29 2022
/// Source(s):
///     The ULTIMATE 2D Character CONTROLLER in UNITY (2021): https://youtu.be/lcw6nuc2uaU
/// Change History: Nov 18 2022 - Jason Cheung
/// - integrated Jaspers' animations using Animator controller and set triggers
/// </summary>
public class Move : NetworkBehaviour
{
    // fighter prefab components
    protected Rigidbody2D _body; //detect x velocity (horizontal movement)
    protected Ground _ground; //detect ground
    protected Animator _animator; //player's animator controller

    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;
    [SerializeField] private AudioClip moveAudioClip;
    private AudioSource audioSource;
    private bool isMoveSoundPlaying = false;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;


    private float maxSpeedChange;
    private float acceleration;
    private bool onGround;
    
    private bool isFacingRight;

    // reference the animator controller for player
    //public Animator animator;


    // Awake is called when the script instance is being loaded
    void Awake()
    {
        CacheComponents();
        // TODO: might have to change, right now its under the assumption
        //  that both players are facing right.
        isFacingRight = true;
    }

    private void Start()
    {
        this.audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Helper method to initialize fighter prefab components
    private void CacheComponents()
    {
        if (!_body) _body = GetComponent<Rigidbody2D>();
        if (!_ground) _ground = GetComponent<Ground>();
        if (!_animator) _animator = GetComponent<Animator>();
    }

    // FixedUpdateNetwork is called once per frame; this is Fusion's Update() method
    public override void FixedUpdateNetwork()
    {
        //if (GameManager.instance.GameState != GameStates.running)
        //    return;

        // checking for input presses
        if (GetInput(out NetworkInputData data))
        {
            direction.x = data.horizontalMovement;
        }
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - _ground.GetFriction(), 0f);

        ////Update animator variable to tell when to play movement animation
        //_animator.SetFloat("Speed", Mathf.Abs(direction.x));

        onGround = _ground.GetOnGround();
        velocity = _body.velocity;

        // flipping the entire body
        if ((direction.x > 0 && !isFacingRight) ||
            (direction.x < 0 && isFacingRight))
        {
            transform.RotateAround(transform.position, transform.up, 180f);
            isFacingRight = !isFacingRight;
        }

        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Runner.DeltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        // Try this: Update animator variable to tell when to play movement animation
        _animator.SetFloat("Speed", Mathf.Abs(velocity.x));

        _body.velocity = velocity;
        /*
        if (_ground && (velocity.x != 0) && !isMoveSoundPlaying)
        {
            RPC_PlayAudioClip();
            isMoveSoundPlaying = true;
        } 
        if (isMoveSoundPlaying && velocity.x == 0)
        {
            RPC_StopAudioClip();
            isMoveSoundPlaying = false;
        }*/
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    private void RPC_PlayAudioClip()
    {
        audioSource.loop = true;
        print("RPC Move Audio Call");
        audioSource.PlayOneShot(moveAudioClip);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    private void RPC_StopAudioClip()
    {
        audioSource.loop = false;
        print("RPC Stop Audio Call");
        audioSource.Stop();
    }

}
