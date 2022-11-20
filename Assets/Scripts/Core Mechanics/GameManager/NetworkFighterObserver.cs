using Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkFighterObserver : NetworkBehaviour
{
    // Static instance of NetworkFighterObserver so other scripts can access it
    public static NetworkFighterObserver Observer = null;

    protected GameManager _gameManager;

    // the fighter they are controlling
    private NetworkObject playerOne;
    private NetworkObject playerTwo;

    // player references - fusion gives them a player id
    private int playerOneRef = 0;
    private int playerTwoRef = 0;

    // is true when both playerOne and playerTwo are assigned
    private bool isPlayersAssigned = false;


    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        if (!Observer) Observer = this;
        Debug.Log("NetworkPlayerObserver instance awake: " + Observer);
    }


    // Start is called after Awake, and before Update
    public void Start()
    {
        CacheOtherObjects();
    }

    // Helper method to initialize OTHER game objects and their components
    private void CacheOtherObjects()
    {
        if (!_gameManager) _gameManager = GameManager.Manager;
    }

    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SetNetworkFighters(int playerOneRef, NetworkObject playerOne, int playerTwoRef, NetworkObject playerTwo)
    {
        // set player ref
        this.playerOneRef = playerOneRef;
        this.playerTwoRef = playerTwoRef;

        // set network fighter
        this.playerOne = playerOne;
        this.playerTwo = playerTwo;

        // change assigned status
        isPlayersAssigned = true;

        RPC_UpdateFighterStatusUI();
    }

    [Rpc(sources: RpcSources.All, RpcTargets.All)]
    public void RPC_UpdateFighterStatusUI()
    {
        Debug.Log("===== NetworkFighter Status ======");
        Debug.Log("Player One ID: " + playerOneRef + 
            ", Stocks - " + playerOne.gameObject.GetComponent<Stock>().Stocks + 
            ", Health - " + playerOne.gameObject.GetComponent<Health>().CurrentHealth);
        Debug.Log("Player Two ID: " + playerTwoRef +
            ", Stocks - " + playerTwo.gameObject.GetComponent<Stock>().Stocks +
            ", Health - " + playerTwo.gameObject.GetComponent<Health>().CurrentHealth);
    }


}
