using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

enum MyButtons
{

}
public struct NetworkInputData : INetworkInput
{
    public Vector3 direction; // screen start-up

    public float move;

    public bool jump;

    // attack
    // dodge
    // special

}
