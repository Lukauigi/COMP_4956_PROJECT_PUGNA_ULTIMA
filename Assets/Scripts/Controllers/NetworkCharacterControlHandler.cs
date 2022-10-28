using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCharacterControlHandler : MonoBehaviour
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
        direction.x = input.RetrieveMoveInput();
        jump = input.RetrieveJumpInput();
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();
        networkInputData.move = direction.x;
        networkInputData.jump = jump;

        return networkInputData;
    }
}
