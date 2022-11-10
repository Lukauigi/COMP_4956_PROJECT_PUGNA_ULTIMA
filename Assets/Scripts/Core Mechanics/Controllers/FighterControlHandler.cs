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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // check move input
        direction.x = input.RetrieveMoveInput();

        // check jump input
        if (input.RetrieveJumpInput())
            jump = true;

        jump = input.RetrieveJumpInput();
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        if (GameManager.instance.GameState != GameStates.running)
            return networkInputData;

        networkInputData.move = direction.x;
        networkInputData.jump = jump;

        jump = false;

        return networkInputData;
    }
}
