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
    private bool down;
    private bool neutralAttack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = input.RetrieveMoveInput();
        
        if (input.RetrieveJumpInput())
            jump = true;

        if (input.RetrieveDownInput())
            down = true;
       
        if (input.RetrieveAttackNeutralInput())
            neutralAttack = true;
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        //if (GameManager.Manager.GameState != GameStates.Running)
        //    return networkInputData;

        networkInputData.move = direction.x;
        networkInputData.jump = jump;
        networkInputData.down = down; 
        networkInputData.neutralAttack = neutralAttack;

        jump = false;
        down = false; 
        neutralAttack = false;

        return networkInputData;
    }
}
