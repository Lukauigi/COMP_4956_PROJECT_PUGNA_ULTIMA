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
}
