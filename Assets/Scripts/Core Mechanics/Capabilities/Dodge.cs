using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// Class that handles the dodge of a fighter/player.
/// Author(s): Richard Mac
/// Date: Nov 18 2022
/// </summary>
public class Dodge : NetworkBehaviour
{
    // fighter prefab components
    protected Rigidbody2D _body; // affects jump velocity

    [SerializeField] private BoxCollider2D playerHitbox;

    private bool isDodgePressed;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        CacheComponents();
    }

    // Helper method to initialize fighter prefab components
    private void CacheComponents()
    {
        if (!_body) _body = GetComponent<Rigidbody2D>();
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
            DodgeAction();
        }
    }
    private void DodgeAction()
    {
        //playerHitbox.enabled = false;
        Debug.Log("Player Dodged!");
    }

}
