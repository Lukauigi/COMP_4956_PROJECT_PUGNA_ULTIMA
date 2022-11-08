using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherits from InputController, basically acts as an input listener.
/// Author(s): Jun Earl Solomon
/// Date: Oct 29 2022
/// Source(s):
///     The ULTIMATE 2D Character CONTROLLER in UNITY (2021): https://youtu.be/lcw6nuc2uaU
/// </summary>
[CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")] // makes sure we can make an instance of it in the menu
public class PlayerController : InputController
{
    // returns the raw axis for horizontal movements
    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    // returns a bool to check if space is pressed
    public override bool RetrieveJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public override bool RetrieveAttackNeutralInput()
    {
        return Input.GetKeyDown(KeyCode.G);
    }



    public override bool RetrieveAttackSideTiltInput()
    {

        return Input.GetKeyDown(KeyCode.G) && Input.GetKeyDown(KeyCode.LeftArrow);
    }

    public override bool RetrieveAttackUpTiltInput()
    {

        return Input.GetKeyDown(KeyCode.G) && Input.GetKeyDown(KeyCode.UpArrow);
    }

    public override bool RetrieveAttackDownTiltInput()
    {

        return Input.GetKeyDown(KeyCode.G) && Input.GetKeyDown(KeyCode.DownArrow);
    }

    // TODO: (for jason) disable player input when GameManager.GameState is not 'running'
}
