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
    [SerializeField] private BoxCollider2D playerCollider;
    // Player EdgeCollider2D field
    [SerializeField] private EdgeCollider2D playerEdgeCollider;

    //public Transform groundCheck;
    //public float checkRadius;
    //public LayerMask whatIsGround;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        //body = GetComponent<NetworkRigidbody2D>();
        ground = GetComponent<Ground>();

        defaultGravityScale = 10f;
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
                //isDownPressed |= data.down; 
            }

        if (direction.y < 0)
        {
            isDownPressed = true;
        }

        onGround = ground.GetOnGround();
        velocity = body.velocity;

        //if object on ground, reset nth jump to 0
        if (onGround && body.velocity.y == 0)
        {
            currentJump = 0;
        }

        //if jump action is requested
        if (isJumpPressed)
        {
            isJumpPressed = false;
            JumpAction();
        }

        UpdateVelocity();
        CheckPlatformFall();
    }


    private void FastFall()
    {
        if (isDownPressed)
        {
            body.gravityScale = 4 * downwardMovementMultiplier;
            if (onGround)
            {
                body.gravityScale = defaultGravityScale;
                isDownPressed = false;
            }

        }
    }

    private void CheckPlatformFall()
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
        yield return new WaitForSeconds(3f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        Physics2D.IgnoreCollision(playerEdgeCollider, platformCollider, false);
        currentLightPlatform = null;
        StopCoroutine(DisableCollision());
    }

}
