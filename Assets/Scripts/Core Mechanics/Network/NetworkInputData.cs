using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct NetworkInputData : INetworkInput
{
    // direction
    public float horizontalMovement;
    public float verticalMovement;

    // jump
    public bool jump;

    // attack
    public bool attack;

    // dodge
    public bool dodge;
}
