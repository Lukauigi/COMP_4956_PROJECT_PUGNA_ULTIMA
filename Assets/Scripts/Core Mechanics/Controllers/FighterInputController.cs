using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherits from InputController, basically acts as an input listener.
/// Author(s): Jun Earl Solomon
/// Date: Oct 29 2022
/// Source(s):
///     The ULTIMATE 2D Character CONTROLLER in UNITY (2021): https://youtu.be/lcw6nuc2uaU
/// Remark(s):
///     The Fighter Inputs are initialized in Unity, under 'Edit' > 'Project Settings' > 'Input Manager'
/// Change History: Nov 16 2022 - Jason Cheung
///     Modified Controller so all inputs from Unity's Input Manager
/// </summary>
[CreateAssetMenu(fileName ="FighterInputController", menuName ="InputController/FighterInputController")] // makes sure we can make an instance of it in the menu
public class FighterInputController : InputController
{
    // returns the raw axis for horizontal movements
    // checks if left or right is pressed
    public override float RetrieveHorizontalInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }
    // returns the raw axis for vertical movements
    // checks if up or down is pressed
    public override float RetrieveVerticalInput()
    {

        return Input.GetAxisRaw("Vertical");
    }

    // checks if jump (space) is pressed
    public override bool RetrieveJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }


    // checks if attack (g) is pressed
    public override bool RetrieveAttackInput()
    {
        return Input.GetButtonDown("Attack");
        //return Input.GetKeyDown(KeyCode.G);
    }

    // checks if dodge (h) is pressed
    public override bool RetrieveDodgeInput()
    {
        return Input.GetButtonDown("Dodge");
    }
}
