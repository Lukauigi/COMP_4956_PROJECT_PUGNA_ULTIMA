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

    /// <summary>
    /// Checks if left or right is pressed.
    /// Returns a raw float value of the horizontal input.
    /// </summary>
    /// <returns></returns>
    public override float RetrieveHorizontalInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    /// <summary>
    /// Checks if up or down is pressed.
    /// Returns a raw float value of the vertical input.
    /// </summary>
    /// <returns></returns>
    public override float RetrieveVerticalInput()
    {
        return Input.GetAxisRaw("Vertical");
    }

    /// <summary>
    /// Checks if Jump (space) is pressed.
    /// </summary>
    /// <returns></returns>
    public override bool RetrieveJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    /// <summary>
    /// Checks if Attack (g / left mouse button) is pressed
    /// </summary>
    /// <returns></returns>
    public override bool RetrieveAttackInput()
    {
        return Input.GetButtonDown("Attack");
    }

    /// <summary>
    /// Returns if Dodge (h / right mouse button) is pressed.
    /// </summary>
    /// <returns></returns>
    public override bool RetrieveDodgeInput()
    {
        return Input.GetButtonDown("Dodge");
    }
}
