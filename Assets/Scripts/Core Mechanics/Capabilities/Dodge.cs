using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Dodge : NetworkBehaviour
{
    private Rigidbody2D body;
    private bool isDodgePressed;
    [SerializeField] private BoxCollider2D playerHitbox;

    private void dodgeAction()
    {
        //playerHitbox.enabled = false;
        Debug.Log("dodge pressed");
    }


    public void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            isDodgePressed |= data.dodge;
           
        }
        if (isDodgePressed)
        {
            isDodgePressed = false;
            dodgeAction();
        }
    }
}
