using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// makes sure we can make an instance of it in the menu, just makes things easier xd
[CreateAssetMenu(fileName = "NetworkPlayerController", menuName = "InputController/NetworkPlayerController")]
public class NetworkPlayerController : InputController
{

    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override bool RetrieveJumpInput()
    {
        //TODO: using GetButton() will make the character jump continuously, change later
        return Input.GetButtonDown("Jump");
    }

    public bool RetrieveAttackInput()
    {
        return Input.GetKeyDown(KeyCode.G);
    }

    public override bool RetrieveAttackSideTiltInput()
    {

        return Input.GetButtonDown("A") && Input.GetButtonDown("LeftArrow");
    }

    public override bool RetrieveAttackUpTiltInput()
    {

        return Input.GetButtonDown("A") && Input.GetButtonDown("UpArrow");
    }

    public override bool RetrieveAttackDownTiltInput()
    {

        return Input.GetButtonDown("A") && Input.GetButtonDown("DownArrow");
    }
}
