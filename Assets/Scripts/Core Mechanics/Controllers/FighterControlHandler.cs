using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that gathers local input data and returns it to the Network.
/// Author(s): Jun Earl Solomon, Jason Cheung
/// Date: Oct 29 2022
/// Remarks: InputController prefab should be 'PlayerController (Fighter Input Controller)
/// </summary>
public class FighterControlHandler : MonoBehaviour
{
    // The Input Controller attached to this fighter
    [SerializeField] private InputController input = null;

    // Data values for input direction and actions
    private Vector2 direction;
    private bool jump;
    private bool attack;
    private bool dodge;


    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // Get all the local input data
        direction.x = input.RetrieveHorizontalInput();
        direction.y = input.RetrieveVerticalInput();

        if (input.RetrieveJumpInput())
            jump = true;
       
        if (input.RetrieveAttackInput())
            attack = true;

        if (input.RetrieveDodgeInput())
            dodge = true;
    }

    /// <summary>
    /// Takes the local input data and reutns it as NetworkInputData.
    /// </summary>
    /// <returns></returns>
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
