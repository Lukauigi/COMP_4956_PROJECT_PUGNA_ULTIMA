using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// Class that handles the stocks (lives) of a fighter/player
/// Note that it also deals with the checks that player should lose a life,
/// ie. falling out of the map, or losing all their health.
/// Author(s): Jason Cheung, Matthew Kan
/// Date: Nov 18 2022
/// </summary>
public class Stock : NetworkBehaviour
{
    // fighter prefab components
    protected Health _health;
    protected Rigidbody2D _body;

    // how many lives the player has
    [SerializeField] private int stocks = 3;
    public int Stocks => stocks; //getter

    // the out-of-map stage boundary
    private readonly int stageBoundaryTop = 15;
    private readonly int stageBoundaryBottom = -5;
    private readonly int stageBoundaryLeft = -15;
    private readonly int stageBoundaryRight = 15;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        CacheComponents();
    }

    // Helper method to initialize fighter prefab components
    private void CacheComponents()
    {
        if (!_health) _health = GetComponent<Health>();
        if (!_body) _body = GetComponent<Rigidbody2D>();
    }

    // FixedUpdateNetwork is called once per frame; this is Fusion's Update() method
    public override void FixedUpdateNetwork()
    {
        //if (GameManager.instance.GameState != GameStates.running)
        //    return;

        // check if player has lost a life (out of stage or lost all their health)
        if (IsOutOfHealth() || IsOutOfStage())
        {
            stocks--;

            if (stocks > 0)
            {
                // respawn player if they still have any stocks left
                Respawn();
            } else
            {
                TriggerLoss();
            }
        }

    }

    // check if player is out of the stage bounds
    private bool IsOutOfStage()
    {
        return (_body.position.y < stageBoundaryBottom) ||
            (_body.position.y > stageBoundaryTop) ||
            (_body.position.x > stageBoundaryRight) ||
            (_body.position.x < stageBoundaryLeft);
    }

    // check if player has lost all their health
    private bool IsOutOfHealth()
    {
        return _health.CurrentHealth <= 0;
    }

    // Helper method to reset player position & health
    private void Respawn()
    {
        //velocity.y = 0;
        //_body.gravityScale = downwardMovementMultiplier;

        _body.position = new Vector2(0, 3);
        _health.ResetHealth();
        Debug.Log("===== Player Respawned =====");
        Debug.Log("Player Stocks: " + stocks);
        Debug.Log("Player Health:" + _health.CurrentHealth);
    }

    private void TriggerLoss()
    {
        Debug.Log("Player is dead!!!");
        Destroy(gameObject);
    }

}

