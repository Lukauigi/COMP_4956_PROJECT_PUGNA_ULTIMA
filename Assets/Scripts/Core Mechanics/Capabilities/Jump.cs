using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles the jump and down-press of a fighter/player
/// Down-press capability checks for FastFall and Falling through a Light Platform.
/// Author(s): Richard Mac, John Ryue, Jun Solomon, Matthew Kan, Jason Cheung
/// Date: Oct 29 2022
/// Source(s):
///     The ULTIMATE 2D Character CONTROLLER in UNITY (2021): https://youtu.be/lcw6nuc2uaU
/// Change History: Nov 28 2022 - Jason Cheung
/// - moved LightPlatform object checks into its own script
/// - integrated Jaspers' animations using Animator controller and set triggers
/// - Moved stage bounds / respawn logic to Stock.cs script
/// - Add logic for RPC call for sound effect method.
/// </summary>
public class Jump : NetworkBehaviour
{
    // Fighter prefab components
    // Detect y velocity for jumping and falling
    protected Rigidbody2D _body;
    // Detect ground object
    protected Ground _ground;
    // Player's hitbox collider
    protected BoxCollider2D _playerCollider;
    // Player's ground hitbox collider
    protected EdgeCollider2D _playerEdgeCollider;
    protected LightPlatform _lightPlatform;

    // Other scene objects to reference
    protected Animator _animator;
    private GameplayAudioManager _audioManager;

    // Player properties for Jump and Gravity
    [SerializeField, Range(0f, 4f)] private float _jumpHeight = 2.5f;
    [SerializeField, Range(1, 3)] private int _maxJumps = 2; //how many jumps character can make while in the air
    [SerializeField, Range(0f, 5f)] private float _downwardMovementMultiplier = 3f; //how fast character will fall
    [SerializeField, Range(0f, 5f)] private float _upwardMovementMultiplier = 1.7f; //affects how fast character moves vertically when jumping

    // Character direction and velocity
    private Vector2 _direction;
    private Vector2 _velocity;

    // Jump counter
    private int _currentJump;
    
    // Default gravity value
    private float _defaultGravityScale;

    // If the jump and down input keys were presesd
    private bool _isJumpPressed;
    private bool _isDownPressed;

    // If the player is on ground; touching a platform
    private bool _onGround;

    // Game object for platform on screen
    private GameObject _currentLightPlatform;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        CacheComponents();

        _defaultGravityScale = 1f;
    }

    /// <summary>
    /// Helper method to initialize components attached to self, from its own script or prefab.
    /// </summary>
    private void CacheComponents()
    {
        if (!_body) _body = GetComponent<Rigidbody2D>();
        if (!_ground) _ground = GetComponent<Ground>();
        if (!_lightPlatform) _lightPlatform = GetComponent<LightPlatform>();
        if (!_playerCollider) _playerCollider = GetComponent<BoxCollider2D>();
        if (!_playerEdgeCollider) _playerEdgeCollider = GetComponent<EdgeCollider2D>();
        if (!_animator) _animator = GetComponent<Animator>();
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
                _isJumpPressed |= data.jump;
                _direction.y = data.verticalMovement;
            }

        // checking if Down is pressed
        _isDownPressed = _direction.y < 0;

        // get current status for on ground and velocity
        _onGround = _ground.GetOnGround();
        _velocity = _body.velocity;

        // get upwards movement for player
        _animator.SetFloat("jumping", _velocity.y);

        // if player is on ground, reset jump counter and stop the jumping animation
        if (_onGround && _body.velocity.y == 0)
        {
            _currentJump = 0;

            // stop the jumping animation
            _animator.SetBool("isJumping", false);
        }

        // checking jump - if jump action is requested
        if (_isJumpPressed)
        {
            _isJumpPressed = false;
            JumpAction();
        }

        _currentLightPlatform = _lightPlatform.CurrentLightPlatform;
        CheckLightPlatformCollision();

        UpdateVelocity();
    }


    /// <summary>
    /// Perform the Jump Action.
    /// </summary>
    protected void JumpAction()
    {
        //check if we are on ground OR we still have jumps left
        if (_onGround || _currentJump < _maxJumps)
        {

            if (_onGround)
            {
                // play jumping animation
                _animator.SetBool("isJumping", true);
            }
            // play jump sound
            if (Object.HasStateAuthority) _audioManager.RPC_PlayUniversalCharacterSFXAudio(PlayerActions.Jump.ToString());

            _currentJump += 1;
            _onGround = false;
            Debug.Log("Player Jumped! Jumps Left: " + (_maxJumps - _currentJump));

            // jump height
            float jumpSpeed = Mathf.Sqrt(-4f * Physics2D.gravity.y * _jumpHeight);

            // reset y velocity before jumping; so mid-air jumps are always the same distance
            _velocity.y = 0f;
            _velocity.y += jumpSpeed;
        }
    }
    

    /// <summary>
    /// Update the Player's Velocity and Gravity.
    /// </summary>
    protected void UpdateVelocity()
    {
        //if going up, apply upward movement
        if (_body.velocity.y > 0)
        {
            _body.gravityScale = _upwardMovementMultiplier;
            FastFall();
        }
        else if (_body.velocity.y < 0) //if going down, apply downward movement
        {
            _body.gravityScale = _downwardMovementMultiplier;
            FastFall();
        }
        else if (_body.velocity.y == 0)
        {
            _body.gravityScale = _defaultGravityScale;
        }
        _body.velocity = _velocity; //apply velocity to rigidbody
    }


    /// <summary>
    /// Activate Fast Falling and increase the player's gravity.
    /// Only activates if down is being pressed.
    /// </summary>
    protected void FastFall()
    {
        if (_isDownPressed)
        {
            _body.gravityScale = 2.5f * _downwardMovementMultiplier;
        }
    }

    /// <summary>
    /// Checks if Player should fall through the Light Platform they are on.
    /// </summary>
    protected void CheckLightPlatformCollision()
    {
        if (_isDownPressed && _currentLightPlatform != null)
        {
            StartCoroutine(DisableLightPlatformCollision());
        }
    }

    /// <summary>
    /// Disables collision of the Light Platform for the player.
    /// </summary>
    /// <returns></returns>
    protected IEnumerator DisableLightPlatformCollision()
    {
        Debug.Log("disabling collision of light platform...");
        BoxCollider2D platformCollider = _currentLightPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(_playerCollider, platformCollider);
        Physics2D.IgnoreCollision(_playerEdgeCollider, platformCollider);
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(_playerCollider, platformCollider, false);
        Physics2D.IgnoreCollision(_playerEdgeCollider, platformCollider, false);
        StopCoroutine(DisableLightPlatformCollision());
    }


}
