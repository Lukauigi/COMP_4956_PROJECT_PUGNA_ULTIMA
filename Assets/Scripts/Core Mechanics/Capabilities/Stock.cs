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
/// Source(s):
///     FPS Unity & Photon Fusion EP4.1 (player names and RPCs): https://youtu.be/-opvmn_QKw0?t=647
/// Remarks: 
/// - Stock is affected by listening to player health.
/// - stock is a networked property so the NetworkFighterObserver will know of its changes.
/// Change History: Nov 19 2022 - Jason Cheung
/// - Modified stock to be a networked property, will call the NetworkFighterObserver to update its UI.
/// </summary>
public class Stock : NetworkBehaviour
{
    // fighter prefab components
    protected Health _health;
    protected Rigidbody2D _body;

    // other scene objects to reference
    protected NetworkFighterObserver _networkFighterObserver;

    // how many lives the fighter has
    [SerializeField] private int stocks = 3;

    // networked property of the fighter's Stocks; listens for OnChanged and notifies others
    private int _stocks;
    [Networked(OnChanged = nameof(OnStocksChanged)), UnityNonSerialized]
    public int Stocks
    {
        get
        {
            return _stocks;
        }
        set
        {
            _stocks = value;
            // client update changes to host
            if (Object.HasInputAuthority)
            {
                RPC_SetStocks(value);
            }
        }
    }

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

    
    // Post Spawn callback
    public override void Spawned()
    {
        // Networked property can only be accessed after object has Spawned
        Stocks = stocks;
    }


    // FixedUpdateNetwork is called once per frame; this is Fusion's Update() method
    public override void FixedUpdateNetwork()
    {
        //if (GameManager.instance.GameState != GameStates.running)
        //    return;

        // check if player has lost a life (out of stage or lost all their health)
        if (IsOutOfHealth() || IsOutOfStage())
        {
            // local assignment of stocks is faster than always using the networked property
            int newStocks = Stocks;

            newStocks--;

            if (newStocks > 0)
            {
                // respawn player if they still have any stocks left
                Respawn();
            } else
            {
                TriggerLoss();
            }

            Stocks = newStocks;
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
        _body.position = new Vector2(0, 3);
        //_velocity.y = 0;
        //_body.gravityScale = downwardMovementMultiplier;
        _health.ResetHealth();
    }

    private void TriggerLoss()
    {
        Debug.Log("Player is dead!!! Ending game...");
        gameObject.SetActive(false);
        GameManager.Manager.RPC_SetGameStateGameOver();
    }

    // Networked OnChanged method for the Network Property Stocks
    static void OnStocksChanged(Changed<Stock> changed)
    {
        changed.Behaviour.OnStocksChanged();
    }

    // OnChanged method to update the network fighter status ui
    private void OnStocksChanged()
    {
        NetworkFighterObserver.Observer.UpdateFighterStatusUI();
    }

    // RPC method for client to notify host its changes for Stocks
    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetStocks(int stocks, RpcInfo info = default)
    {
        this.Stocks = stocks;
    }

}

