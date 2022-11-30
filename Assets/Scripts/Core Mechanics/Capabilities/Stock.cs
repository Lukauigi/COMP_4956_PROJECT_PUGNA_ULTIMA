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
/// Change History: Nov 22 2022 - Lukasz Bednarek
/// - Modified stock to be a networked property, will call the NetworkFighterObserver to update its UI.
/// - Add logic for RPC call for sound effect method.
/// </summary>
public class Stock : NetworkBehaviour
{
    // fighter prefab components
    protected Health _health;
    protected Rigidbody2D _body;

    // other scene objects to reference
    protected NetworkFighterObserver _networkFighterObserver;
    protected GameplayAudioManager _audioManager;

    // how many lives the fighter has
    [SerializeField] private int _stocks = 3;


    // networked property of the fighter's Stocks; listens for OnChanged and notifies others
    private int _currentStocks;

    [Networked(OnChanged = nameof(OnStocksChanged)), UnityNonSerialized]
    public int CurrentStocks
    {
        get
        {
            return _currentStocks;
        }
        set
        {
            _currentStocks = value;
            // client update changes to host
            if (Object.HasInputAuthority)
            {
                RPC_SetStocks(value);
            }
        }
    }

    // The amount of Deaths this player has - For Database, other player uses this number for Kills
    private int _deaths = 0;
    public int Deaths => _deaths; // getter

    // helper bool to pause respawn checks in update method WHILE respawning
    private bool isRespawning = false;

    // the out-of-map stage boundary
    private readonly int stageBoundaryTop = 15;
    private readonly int stageBoundaryBottom = -5;
    private readonly int stageBoundaryLeft = -15;
    private readonly int stageBoundaryRight = 15;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        CacheComponents();
    }

    /// <summary>
    /// Helper method to initialize components attached to self, from its own script or prefab.
    /// </summary>
    private void CacheComponents()
    {
        if (!_health) _health = GetComponent<Health>();
        if (!_body) _body = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Start is called after Awake, and before Update.
    /// Generally used to reference other scene objects, after they have all been initialized.
    /// </summary>
    private void Start()
    {
        // cache other scene objects
        if (!_audioManager) _audioManager = GameObject.Find("SceneAudioManager").GetComponent<GameplayAudioManager>();
    }


    /// <summary>
    /// Post Spawn callback.
    /// Generally used because Network Properties can only be accessed after the object has Spawned.
    /// </summary>
    public override void Spawned()
    {
        // Networked property can only be accessed after object has Spawned
        CurrentStocks = _stocks;
    }

    /// <summary>
    /// FixedUpdateNetwork is called once per frame. This is Fusion's Update() method.
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        // check if player has lost a life (out of stage)
        if (IsOutOfStage() && !isRespawning)
        {
            // pause update
            isRespawning = true;

            // local assignment of stocks is faster than always using the networked property
            int newStocks = CurrentStocks;

            newStocks--;
            _deaths++;

            CurrentStocks = newStocks;

            if (newStocks != 0)
            {
                // respawn player if they still have any stocks left
                Respawn();
            } else
            {
                TriggerLoss();
            }

            // end of code-block; can resume checks
            isRespawning = false;
        }

    }

    /// <summary>
    /// Check if the player is out of the stage bounds.
    /// </summary>
    /// <returns></returns>
    protected bool IsOutOfStage()
    {
        return (_body.position.y < stageBoundaryBottom) ||
            (_body.position.y > stageBoundaryTop) ||
            (_body.position.x > stageBoundaryRight) ||
            (_body.position.x < stageBoundaryLeft);
    }


    /// <summary>
    /// Respawn player by resetting their position & health.
    /// </summary>
    protected void Respawn()
    {
        //RPC_PlayHitDeathzoneAudio();
        _audioManager.RPC_PlayUniversalCharacterSFXAudio(PlayerActions.Death.ToString());

        // reset position and falling velocity
        _body.position = new Vector2(0, 3);

        Vector2 zeroVelocity = new Vector2(0, 0);
        _body.velocity = zeroVelocity;

        // reset health
        _health.ResetHealth();
    }

    /// <summary>
    /// Tell the GameManager to End the game.
    /// Method should only be called by itself, when all stocks are gone.
    /// </summary>
    protected void TriggerLoss()
    {
        Debug.Log("Player is dead!!! Ending game...");
        gameObject.SetActive(false);
        GameManager.Manager.RPC_SetGameStateGameOver();
    }


    /// <summary>
    /// Networked OnChanged method for the Network Property Health
    /// </summary>
    /// <param name="changed"></param>
    static void OnStocksChanged(Changed<Stock> changed)
    {
        changed.Behaviour.OnStocksChanged();
    }

    /// <summary>
    /// OnChanged method to update the network fighter status ui
    /// </summary>
    private void OnStocksChanged()
    {
        NetworkFighterObserver.Observer.UpdateFighterStatus();
    }

    /// <summary>
    /// RPC method for client to notify host its changes for Stocks
    /// </summary>
    /// <param name="stocks"></param>
    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetStocks(int stocks)
    {
        this.CurrentStocks = stocks;
    }
}

