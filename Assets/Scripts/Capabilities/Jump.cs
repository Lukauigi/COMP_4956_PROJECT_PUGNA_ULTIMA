using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : NetworkBehaviour
{
    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 1f;
    [SerializeField, Range(0, 2)] private int maxAirJumps = 2; //max 2 jumps
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f; //how fast character will fall
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 6f; //affects how fast character moves vertically when jumping

    private Rigidbody2D body; //detect jump velocity
    private Ground ground; //detect ground
    private Vector2 velocity;

    private int currentJump; //how many times we have jumped
    private float defaultGravityScale;

    private bool isJumpPressed;
    private bool onGround;

    private bool desiredJump;

    // reference the animator controller for player
    public Animator animator;

    //public Transform groundCheck;
    //public float checkRadius;
    //public LayerMask whatIsGround;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();

        defaultGravityScale = 10f;
    }


    // The Jump boolean variable remains set in new update cycle until we
    // manually set to false
    void Update()
    {
        if (GameManager.instance.GameState != GameStates.running)
            return;

        //Need input to be true once and if it is used, set it to false
        isJumpPressed |= input.RetrieveJumpInput();
    }

    //Method to perform jump action.
    private void JumpAction()
    {
        
        Debug.Log("Update Jump");
        //check if we are on ground AND we still have jumps left
        if (onGround && currentJump < maxAirJumps)
        {
            
            currentJump += 1;
            float jumpSpeed = Mathf.Sqrt(-4f * Physics2D.gravity.y * jumpHeight);
            
            //jump speed never goes negative
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
        }

        // play jumping animation
        animator.SetBool("isJumping", true);
    }

    public override void FixedUpdateNetwork()
    {
        if (GameManager.instance.GameState != GameStates.running)
            return;

        if (GetInput(out NetworkInputData data))
            {
                //desiredJump |= data.jump;
                isJumpPressed |= data.jump;
            } else
            {
                //desiredJump |= input.RetrieveJumpInput();
            }

        onGround = ground.GetOnGround();
        //onGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        velocity = body.velocity;

        //if object on ground, reset nth jump to 0
        if (onGround)
        {
            currentJump = 0;
        }

        //if jump action is requested
        if (isJumpPressed)
        {

            isJumpPressed = false;
            //while (currentJump < maxAirJumps)
            //{
            //    JumpAction();
            //}
            JumpAction();
            //MoveDuringJumping();
        }

        //if going up, apply upward movement
        if (body.velocity.y > 0)
        {
            body.gravityScale = upwardMovementMultiplier;
            if (Input.GetKey(KeyCode.DownArrow))
            {
                Debug.Log("press down to fastfall");
                body.gravityScale = 4 * downwardMovementMultiplier;
            }
        }
        else if (body.velocity.y < 0) //if going down, apply downward movement
        {
            body.gravityScale = downwardMovementMultiplier;
            if (Input.GetKey(KeyCode.DownArrow))
            {
                Debug.Log("press down to fastfall");
                body.gravityScale = 4*downwardMovementMultiplier;
            }
        }
        else if (body.velocity.y == 0)
        {
            body.gravityScale = defaultGravityScale;
        }
        body.velocity = velocity; //apply velocity to rigidbody
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();
        // desiredJump or input.RetrieveJumpInput() idk
        networkInputData.jump = desiredJump;

        return networkInputData;
    }

    //private void MoveDuringJumping()
    //{
    //    float moveSpeed = 40f;
    //    float midAirControl = 1.5f;
    //    if (Input.GetKey(KeyCode.LeftArrow))
    //    {
    //        if (onGround)
    //        {
    //            body.velocity = new Vector2(-moveSpeed, body.velocity.y);
    //        } else
    //        {
    //            body.velocity += new Vector2(-moveSpeed * midAirControl * Time.deltaTime, 0);
    //            body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -moveSpeed, moveSpeed), body.velocity.y);
    //        }
    //    } else
    //    {
    //        if (Input.GetKey(KeyCode.RightArrow))
    //        {
    //            if (onGround)
    //            {
    //                body.velocity = new Vector2(moveSpeed, body.velocity.y);
    //            }
    //            else
    //            {
    //                body.velocity += new Vector2(moveSpeed * midAirControl * Time.deltaTime, 0);
    //                body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -moveSpeed, moveSpeed), body.velocity.y);
    //            }
    //        }
    //    }
    //}
}
