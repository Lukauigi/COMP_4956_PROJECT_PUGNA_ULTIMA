using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : NetworkBehaviour
{
    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 1f;
    [SerializeField, Range(0, 2)] private int maxAirJumps = 2; //max 2 jumps
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 9f; //how fast character will fall
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f; //affects how fast character moves vertically when jumping

    private Rigidbody2D body; //detect jump velocity
    private Ground ground; //detect ground
    private Vector2 velocity;

    private int currentJump; //how many times we have jumped
    private float defaultGravityScale;

    private bool desiredJump;
    private bool onGround;
    //private bool canJump = true;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();

        defaultGravityScale = 1f;
    }
    /*// Start is called before the first frame update
    void Start()
    {
        
    }*/

    // The Jump boolean variable remains set in new update cycle until we
    // manually set to false
    void Update()
    {
        //Need input to be true once and if it is used, set it to false
        desiredJump |= input.RetrieveJumpInput();
    }

    //Method to perform jump action.
    private void JumpAction()
    {
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

    }

    public override void FixedUpdateNetwork()
    {
        onGround = ground.GetOnGround();
        velocity = body.velocity;

        //if object on ground, reset nth jump to 0
        if (onGround)
        {
            currentJump = 0;
        }

        //if jump action is requested
        if (desiredJump)
        {
            desiredJump = false;
            while (currentJump < maxAirJumps)
            {
                JumpAction();
            }
            
        }

        //if going up, apply upward movement
        if (body.velocity.y > 0)
        {
            body.gravityScale = upwardMovementMultiplier;
        }
        else if (body.velocity.y < 0) //if going down, apply downward movement
        {
            body.gravityScale = downwardMovementMultiplier;
        }
        else if (body.velocity.y == 0)
        {
            body.gravityScale = defaultGravityScale;
        }
        body.velocity = velocity; //apply velocity to rigidbody
    }

    //private IEnumerator JumpCoolDown()
    //{
    //    canJump = false;
    //    yield return new WaitForSeconds(1);
    //    canJump = true;
    //}
}
