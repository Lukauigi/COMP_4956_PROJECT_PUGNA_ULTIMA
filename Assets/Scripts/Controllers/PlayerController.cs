using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// makes sure we can make an instance of it in the menu, just makes things easier xd
[CreateAssetMenu(fileName ="PlayerController", menuName ="InputController/PlayerController")]
public class PlayerController : InputController
{

    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override bool RetrieveJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

}
