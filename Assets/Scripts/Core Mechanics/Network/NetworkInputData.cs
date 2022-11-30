using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// Data values to Gather from Local Input and store into NetworkInputData.
/// </summary>
public struct NetworkInputData : INetworkInput
{
    // direction x
    public float horizontalMovement;

    // direction y
    public float verticalMovement;

    // jump
    public bool jump;

    // attack
    public bool attack;

    // dodge
    public bool dodge;
}
