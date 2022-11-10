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
[CreateAssetMenu(fileName ="FighterInputController", menuName ="InputController/FighterInputController")] // makes sure we can make an instance of it in the menu
public class FighterInputController : InputController
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

    // returns a bool to check if attack is pressed
    public bool RetrieveAttackInput()
    {
        return Input.GetKeyDown(KeyCode.G);
    }

    // TODO: (for jason) disable player input when GameManager.GameState is not 'running'
}
