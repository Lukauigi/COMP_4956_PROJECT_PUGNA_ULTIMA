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
/// Change History: Nov 25 2022 - Lukasz Bednarek
/// - integrated Jaspers' animations using Animator controller and set triggers
/// - Add logic for RPC call for sound effect method. Does not work properly; therefore, it is commencted out.
/// - Fixed logic movement audio calls.
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
    private GameObject _audioManager;
    private bool _isMovingAudioPlaying = false;

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
        this._audioManager = GameObject.Find("SceneAudioManager");
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

        //print("moving: " + _isMovingAudioPlaying + ".. onGround: " + onGround + ".. velocity.x: " + velocity.x + ".. velocity.y: " + velocity.y + ".. direction.x: " + direction.x);

        // Plays move audio clip
        if (!_isMovingAudioPlaying && onGround && velocity.x != 0 && velocity.y == 0 && direction.x != 0)
        {
            _isMovingAudioPlaying = true;
            _audioManager.GetComponent<GameplayAudioManager>().RPC_PlayMoveAudio(PlayerActions.Move.ToString(), Object.Id);
        }

        // Stops move audio clip
        if (_isMovingAudioPlaying && (velocity.x == 0 || !onGround) && direction.x == 0)
        {
            _isMovingAudioPlaying = false;
            _audioManager.GetComponent<GameplayAudioManager>().RPC_StopMoveAudio(Object.Id);
        }

    }
}
