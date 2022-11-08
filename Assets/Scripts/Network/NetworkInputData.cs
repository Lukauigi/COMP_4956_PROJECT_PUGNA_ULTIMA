using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

enum MyButtons
{

}
public struct NetworkInputData : INetworkInput
{
    public float move;

    public bool jump;

    public bool neutralAttack;

    public bool attackSideTilt;

    public bool attackUpTilt;

    public bool attackDownTilt;
}
