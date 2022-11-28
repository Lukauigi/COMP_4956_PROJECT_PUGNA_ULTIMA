using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// Static class to control the stage "select" behaviour.
/// In reality, the controller randomly selects a stage.
/// Author(s): Jason Cheung
/// Date: Nov 21 2022
/// Remarks: the call to select the stage is in PlayerItemObserver.FixedUpdateNetwork()
/// </summary>
public class GameStageController : NetworkBehaviour
{
    // Static instance of GameManager so other scripts can access it
    public static GameStageController Instance = null;

    // stage game object prefabs
    [SerializeField] protected GameObject _stageOne;
    [SerializeField] protected GameObject _stageTwo;
    [SerializeField] protected GameObject _stageThree;

    // list of stages
    private List<GameObject> _stages = new List<GameObject>();

    // random int value to choose a stage
    private int _randomIndex;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }


    /// <summary>
    /// RPC method to initialize stage values and spawn it.
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_SelectRandomStage()
    {
        // init list of stages
        _stages.Add(_stageOne);
        _stages.Add(_stageTwo);
        _stages.Add(_stageThree);

        // cache random number 0, 1, 2
        _randomIndex = Random.Range(0, 3);
        Debug.Log("random int selected: " + _randomIndex);

        RPC_CacheStage(_randomIndex);
    }

    /// <summary>
    /// RPC Helper method to spawn the selected stage.
    /// </summary>
    /// <param name="stageNumber"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    private void RPC_CacheStage(int stageNumber)
    {
        int index = 0;

        if (stageNumber >= _stages.Count || stageNumber < 0)
            stageNumber = _randomIndex;

        foreach (GameObject stage in _stages)
        {
            // show the selected stage
            if (stageNumber == index)
                stage.SetActive(true);
            else // hide the rest
                stage.SetActive(false);

            index++;
        }

    }


}
