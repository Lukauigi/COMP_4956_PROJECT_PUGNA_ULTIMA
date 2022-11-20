using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterControlHandler : MonoBehaviour
{

    //PlayerController input = new PlayerController();

    [SerializeField] public InputController input = null;

    private Vector2 direction;
    private bool jump;
    //private bool down;
    private bool attack;
    private bool dodge;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = input.RetrieveHorizontalInput();
        direction.y = input.RetrieveVerticalInput();
        //if (input.RetrieveVerticalInput())
        //    down = true;

        if (input.RetrieveJumpInput())
            jump = true;
       
        if (input.RetrieveAttackInput())
            attack = true;

        if (input.RetrieveDodgeInput())
            dodge = true;
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        networkInputData.horizontalMovement = direction.x;
        networkInputData.verticalMovement = direction.y;
        networkInputData.jump = jump;
        networkInputData.attack = attack;
        networkInputData.dodge = dodge;

        jump = false;
        attack = false;
        dodge = false;

        return networkInputData;
    }
}
