using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles all the movement (horizontal) of a fighter/player
/// /// Author(s): Jun Earl Solomon
/// Date: Oct 29 2022
/// Source(s):
///     The ULTIMATE 2D Character CONTROLLER in UNITY (2021): https://youtu.be/lcw6nuc2uaU
/// </summary>
public class Move : NetworkBehaviour
{
    [SerializeField] public InputController input = null; // generic input
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
        if (GameManager.instance.GameState != GameStates.running)
            return;
        //direction.x = input.RetrieveMoveInput();
        /*desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.GetFriction(), 0f);*/
    }

    public override void FixedUpdateNetwork()
    {
        // For Host-Client Mode
        if (GetInput(out NetworkInputData data))
        {
            direction.x = data.move;
        } else
        {
            //direction.x = input.RetrieveMoveInput();
        }
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.GetFriction(), 0f);

        float inputHorizontal = input.RetrieveMoveInput();
        onGround = ground.GetOnGround();
        velocity = body.velocity;

        // region : johnny & richard's feature branch
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
        // end-region


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
