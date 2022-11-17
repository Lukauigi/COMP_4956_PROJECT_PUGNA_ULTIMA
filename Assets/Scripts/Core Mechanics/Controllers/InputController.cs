using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Input Controller is the abstraction from the actual input source.
/// We can use this class to represent a generic input.
/// 
/// Note: when adding more methods, please make sure that the scripts
/// abstracting this scriptable object have those new methods.
/// 
/// Author(s): Jun Earl Solomon
/// Date: Oct 29 2022
/// Source(s):
///     The ULTIMATE 2D Character CONTROLLER in UNITY (2021): https://youtu.be/lcw6nuc2uaU
/// Remark(s):
///     The Input Controls are initialized in Unity, under 'Edit' > 'Project Settings' > 'Input Manager'
/// Change History: Nov 16 2022 - Jason Cheung
///     Modified Controller so all inputs from Unity's Input Manager
/// </summary>
public abstract class InputController : ScriptableObject
{

    public abstract float RetrieveHorizontalInput();
    public abstract float RetrieveVerticalInput();

    public abstract bool RetrieveJumpInput();

    public abstract bool RetrieveAttackInput();

    public abstract bool RetrieveDodgeInput();

}
