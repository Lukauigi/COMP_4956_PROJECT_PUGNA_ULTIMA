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
    // Fighter prefab components
    // Detect x velocity for horizontal movement
    protected Rigidbody2D _body;
    // Detect ground object
    protected Ground _ground;
    protected Animator _animator;

    // Other scene objects to reference
    private GameplayAudioManager _audioManager;

    // Player properties for Movement speed
    [SerializeField, Range(0f, 100f)] private float _maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 20f;

    // Character direction, velocity, and acceleration
    private Vector2 _direction;
    private Vector2 _desiredVelocity;
    private Vector2 _velocity;
    private float _maxSpeedChange;
    private float _acceleration;

    // If the player is on ground; touching a platform
    private bool onGround;
    
    // If the player direction is facing right
    private bool isFacingRight;

    // If the movement audio is playing
    private bool _isMovingAudioPlaying = false;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        CacheComponents();

        // assumes both players are facing right
        isFacingRight = true;
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
    /// Helper method to initialize components attached to self, from its own script or prefab.
    /// </summary>
    private void CacheComponents()
    {
        if (!_body) _body = GetComponent<Rigidbody2D>();
        if (!_ground) _ground = GetComponent<Ground>();
        if (!_animator) _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// FixedUpdateNetwork is called once per frame. This is Fusion's Update() method.
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        // checking for input presses
        if (GetInput(out NetworkInputData data))
        {
            _direction.x = data.horizontalMovement;
        }

        // get current status for on ground and velocity
        onGround = _ground.GetOnGround();
        _velocity = _body.velocity;

        // flipping the entire body if player switched directions
        if ((_direction.x > 0 && !isFacingRight) ||
            (_direction.x < 0 && isFacingRight))
        {
            transform.RotateAround(transform.position, transform.up, 180f);
            isFacingRight = !isFacingRight;
        }

        // determine resulting velocity
        _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed - _ground.GetFriction(), 0f);
        _acceleration = onGround ? _maxAcceleration : _maxAirAcceleration;
        _maxSpeedChange = _acceleration * Runner.DeltaTime;
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

        // Update animator variable to tell when to play movement animation
        _animator.SetFloat("Speed", Mathf.Abs(_velocity.x));

        // set the resulting velocity
        _body.velocity = _velocity;

        // Plays move audio clip
        if (!_isMovingAudioPlaying && onGround && _velocity.x != 0 && _velocity.y == 0 && _direction.x != 0)
        {
            _isMovingAudioPlaying = true;
            _audioManager.RPC_PlayMoveAudio(PlayerActions.Move.ToString(), Object.Id);
        }

        // Stops move audio clip
        if (_isMovingAudioPlaying && (_velocity.x == 0 || !onGround) && _direction.x == 0)
        {
            _isMovingAudioPlaying = false;
            _audioManager.RPC_StopMoveAudio(Object.Id);
        }

    }
}
