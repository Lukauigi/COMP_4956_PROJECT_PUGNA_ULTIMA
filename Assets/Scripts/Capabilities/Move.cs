using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : NetworkBehaviour
{
    //[SerializeField] public InputController input = null;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
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
        //direction.x = input.RetrieveMoveInput();
        /*desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.GetFriction(), 0f);*/
    }

    public override void FixedUpdateNetwork()
    {
        // direction.x = input.RetrieveMoveInput();
        // WTFFFF AM I DOING
        if (GetInput(out NetworkInputData input))
        {
            direction.x = input.move;
        }

        onGround = ground.GetOnGround();
        velocity = body.velocity;

        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.GetFriction(), 0f);

        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        /*maxSpeedChange = acceleration * Time.deltaTime;*/
        maxSpeedChange = acceleration * Runner.DeltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        body.velocity = velocity;
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();
        // direction.x or input.RetrieceMoveInput() idk
        networkInputData.move = direction.x;

        return networkInputData;
    }
}
