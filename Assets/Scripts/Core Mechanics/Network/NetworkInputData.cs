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
    public bool down;

    // attack
    // dodge
    // special

    // attack
    public bool neutralAttack;

    public bool attackSideTilt;

    public bool attackUpTilt;

    public bool attackDownTilt;
}
