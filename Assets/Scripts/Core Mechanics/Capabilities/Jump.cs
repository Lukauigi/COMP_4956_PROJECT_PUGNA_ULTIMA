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
/// Change History: Nov 18 2022 - Jason Cheung
/// - Moved stage bounds / respawn logic to Stock.cs script
/// </summary>
public class Jump : NetworkBehaviour
{
    // fighter prefab components
    protected Rigidbody2D _body; //detect y velocity (jump / falling)
    //protected NetworkRigidbody2D _body;
    protected Ground _ground; //detect ground
    protected BoxCollider2D _playerCollider; // player's hitbox collider
    protected EdgeCollider2D _playerEdgeCollider; // player's ground hitbox collider
    protected Animator _animator; //player's animator controller

    [SerializeField, Range(0f, 4f)] private float jumpHeight = 3f;
    [SerializeField, Range(1, 3)] private int maxAirJumps = 2; //how many jumps character can make while in the air
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f; //how fast character will fall
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 6f; //affects how fast character moves vertically when jumping

    private Vector2 direction;
    private Vector2 velocity;

    private int currentJump; //how many times we have jumped
    private float defaultGravityScale;

    private bool isJumpPressed;
    private bool isDownPressed;

    private bool onGround;

    // reference the animator controller for player
    //public Animator animator;

    // Game object for platform on screen
    private GameObject currentLightPlatform;


    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        CacheComponents();

        defaultGravityScale = 1f;
    }

    // Helper method to initialize fighter prefab components
    private void CacheComponents()
    {
        if (!_body) _body = GetComponent<Rigidbody2D>();
        if (!_ground) _ground = GetComponent<Ground>();
        if (!_playerCollider) _playerCollider = GetComponent<BoxCollider2D>();
        if (!_playerEdgeCollider) _playerEdgeCollider = GetComponent<EdgeCollider2D>();
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
                isJumpPressed |= data.jump;
                direction.y = data.verticalMovement;
            }

        // checking if Down is pressed
        isDownPressed = direction.y < 0;

        onGround = _ground.GetOnGround();
        velocity = _body.velocity;

        // if player is on ground, reset jump counter and stop the jumping animation
        if (onGround && _body.velocity.y == 0)
        {
            currentJump = 0;

            // stop the jumping animation
            _animator.SetBool("isJumping", false);
            _animator.SetBool("isDoubleJumping", false);
        }

        // checking jump - if jump action is requested
        if (isJumpPressed)
        {
            isJumpPressed = false;
            JumpAction();
        }

        CheckFallThroughPlatform();

        UpdateVelocity();
    }


    //Method to perform jump action.
    private void JumpAction()
    {
        //check if we are on ground OR we still have jumps left
        if (onGround || currentJump < maxAirJumps)
        {

            if (onGround)
            {
                // play jumping animation
                _animator.SetBool("isJumping", true);
            }
            else {
                // replay jump animation
                _animator.SetBool("isDoubleJumping", true);
            }



            currentJump += 1;
            onGround = false;
            Debug.Log("Player Jumped! Jumps Left: " + (maxAirJumps - currentJump));

            float jumpSpeed = Mathf.Sqrt(-4f * Physics2D.gravity.y * jumpHeight);
            
            //jump speed never goes negative
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
        }
    }
    

    // Method to check and apply velocity
    private void UpdateVelocity()
    {
        //if going up, apply upward movement
        if (_body.velocity.y > 0)
        {
            _body.gravityScale = upwardMovementMultiplier;
            FastFall();
        }
        else if (_body.velocity.y < 0) //if going down, apply downward movement
        {
            _body.gravityScale = downwardMovementMultiplier;
            FastFall();
        }
        else if (_body.velocity.y == 0)
        {
            _body.gravityScale = defaultGravityScale;
        }
        _body.velocity = velocity; //apply velocity to rigidbody
    }


    // Method to activate fast falling
    private void FastFall()
    {
        if (isDownPressed)
        {
            _body.gravityScale = 3 * downwardMovementMultiplier;
        }
    }

    // Method to check if player should fall through a Light Platform
    private void CheckFallThroughPlatform()
    {
        if (isDownPressed && currentLightPlatform != null)
        {
            StartCoroutine(DisableCollision());
        }
    }

    // Triggers when player makes contact with the light platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TwoWayPlatform") && 
            currentLightPlatform != collision.gameObject)
        {
            currentLightPlatform = collision.gameObject;
        }
    }

    // Triggers when player stays on the light platform
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TwoWayPlatform") && 
            currentLightPlatform != collision.gameObject)
        {
            currentLightPlatform = collision.gameObject;
        }
    }

    // Triggers when player leave the light platform
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TwoWayPlatform") &&
            currentLightPlatform == collision.gameObject)
        {
            currentLightPlatform = null;
        }
    }

    // Disables collision of the light platform so the player can fall through it
    private IEnumerator DisableCollision()
    {
        Debug.Log("disabling collision of light platform...");
        BoxCollider2D platformCollider = currentLightPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(_playerCollider, platformCollider);
        Physics2D.IgnoreCollision(_playerEdgeCollider, platformCollider);
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(_playerCollider, platformCollider, false);
        Physics2D.IgnoreCollision(_playerEdgeCollider, platformCollider, false);
        StopCoroutine(DisableCollision());
    }

}
