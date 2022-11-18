using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : NetworkBehaviour
{
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 1f;
    [SerializeField, Range(0, 2)] private int maxAirJumps = 2; //max 2 jumps
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f; //how fast character will fall
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f; //affects how fast character moves vertically when jumping

    private Rigidbody2D body; //detect jump velocity
    //private NetworkRigidbody2D body;
    private Ground ground; //detect ground
    private Vector2 direction;
    private Vector2 velocity;

    private int currentJump; //how many times we have jumped
    private float defaultGravityScale;

    private bool isJumpPressed;
    private bool onGround;
    private bool isDownPressed;

    // Game object for platform on screen
    private GameObject currentLightPlatform;

    // Player BoxCollider2D field
    private BoxCollider2D playerCollider;
    // Player EdgeCollider2D field
    private EdgeCollider2D playerEdgeCollider;

    // Get width of screen
    private int screenSizeX;
    // Get height of screen
    private int screenSizeY;

    // Lives remaining
    private int livesRemaining = 3;

    //public Transform groundCheck;
    //public float checkRadius;
    //public LayerMask whatIsGround;

    
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        //body = GetComponent<NetworkRigidbody2D>();
        ground = GetComponent<Ground>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerEdgeCollider = GetComponent<EdgeCollider2D>();

        defaultGravityScale = downwardMovementMultiplier;

        // Set screen size variables
        screenSizeX = Screen.width;
        screenSizeY = Screen.height;

        print("================Screen width: " + screenSizeX + "=======================");
        print("================Screen height: " + screenSizeY + "=======================");
    }

    //Method to perform jump action.
    private void JumpAction()
    {
        
        Debug.Log("Update Jump");
        //check if we are on ground OR we still have jumps left
        if (onGround || currentJump < maxAirJumps)
        {
            currentJump += 1;
            onGround = false;
            Debug.Log(currentJump);

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
        if (body.velocity.y > 0 )
        {

            body.gravityScale = upwardMovementMultiplier;
            FastFall();
        }
        else if (body.velocity.y < 0) //if going down, apply downward movement
        {
            body.gravityScale = downwardMovementMultiplier;
            FastFall();
        }
        else if (body.velocity.y == 0)
        {
            body.gravityScale = defaultGravityScale;
        }
        body.velocity = velocity; //apply velocity to rigidbody
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
        if (direction.y < 0)
        {
            isDownPressed = true;
        } else
        {
            isDownPressed = false;
        }

        onGround = ground.GetOnGround();
        velocity = body.velocity;

        // if player is on ground, reset jump counter
        if (onGround && body.velocity.y == 0)
        {
            currentJump = 0;
        }

        // checking jump - if jump action is requested
        if (isJumpPressed)
        {
            isJumpPressed = false;
            JumpAction();
        }

        CheckRespawn();
        CheckFallThroughPlatform();

        UpdateVelocity();
    }

    private void CheckRespawn()
    {
        int stageBoundaryBottom = -5;
        int stageBoundaryTop = 15;
        int stageBoundaryLeft = -15;
        int stageBoundaryRight = 15;

        // check if player should respawn
        if (livesRemaining != 0 && 
            ((body.position.y < stageBoundaryBottom) ||
            (body.position.y > stageBoundaryTop) ||
            (body.position.x > stageBoundaryRight) ||
            (body.position.x < stageBoundaryLeft)))
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        livesRemaining--;
        isDownPressed = false;
        velocity.y = 0;
        body.gravityScale = downwardMovementMultiplier;
        body.position = new Vector2(0, 3);
        print("====================== player respawned =====================");
        print("lives remaining: " + livesRemaining);
    }

    private void FastFall()
    {
        if (isDownPressed)
        {
            body.gravityScale = 3 * downwardMovementMultiplier;
            if (onGround)
            {
                body.gravityScale = defaultGravityScale;
                //isDownPressed = false;
            }

        }
    }

    private void CheckFallThroughPlatform()
    {
        if (isDownPressed && currentLightPlatform != null)
        {
            StartCoroutine(DisableCollision());
        }
    }

    // Triggers when player makes contact with the platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print("platform not assigned");
        if (collision.gameObject.CompareTag("TwoWayPlatform"))
        {
            print("platform assigned");
            currentLightPlatform = collision.gameObject;
        }
    }

    // Triggers when player passes through the platform from above
    /*private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TwoWayPlatform"))
        {
            currentLightPlatform = null;
        }
    }*/

    // Method used to ignore the collision field of the platform 
    private IEnumerator DisableCollision()
    {
        print("disable collision method called");
        BoxCollider2D platformCollider = currentLightPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        Physics2D.IgnoreCollision(playerEdgeCollider, platformCollider);
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        Physics2D.IgnoreCollision(playerEdgeCollider, platformCollider, false);
        currentLightPlatform = null;
        StopCoroutine(DisableCollision());
    }

}
