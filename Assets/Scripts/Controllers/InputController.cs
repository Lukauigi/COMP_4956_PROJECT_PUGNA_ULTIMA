using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Input Controller is the abstraction from the actual input source.
/// We can use this class to represent a generic input.
/// 
/// Note: when adding more methods, please make sure that the scripts
/// abstracting this scriptable object have those new methods.
/// </summary>
public abstract class InputController : ScriptableObject
{
    // everything here might be subject to change
    public abstract float RetrieveMoveInput();

    public abstract bool RetrieveJumpInput();

    // TODO: add more inputs for different attacks
}