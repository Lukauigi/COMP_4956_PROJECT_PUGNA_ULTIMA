using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : NetworkBehaviour
{
    [SerializeField] private InputController input = null; //generic input
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    //private float speed;
    private Rigidbody2D body;
    private Ground ground;

    private float maxSpeedChange;
    private float acceleration;
    private bool onGround;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GameState != GameStates.running)
            return;

        direction.x = input.RetrieveMoveInput(); //retrieve direction using InputController
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.GetFriction(), 0f);
    }

    public override void FixedUpdateNetwork()
    {
        if (GameManager.instance.GameState != GameStates.running)
            return;

        float inputHorizontal = input.RetrieveMoveInput();
        onGround = ground.GetOnGround();
        velocity = body.velocity;

        if (inputHorizontal != 0)
        {
            body.AddForce(new Vector2(inputHorizontal * Time.deltaTime, 0f));
        }

        if (inputHorizontal > 0)
        {
            body.transform.localScale = new Vector3(1, 1, 1);
        }

        if (inputHorizontal < 0)
        {
            body.transform.localScale = new Vector3(-1, 1, 1);
        }


        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Runner.DeltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        body.velocity = velocity;
    }
}
