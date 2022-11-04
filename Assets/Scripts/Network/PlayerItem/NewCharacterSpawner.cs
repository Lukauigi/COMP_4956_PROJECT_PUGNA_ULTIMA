
using Fusion;
using UnityEngine;

/// <summary>
/// Prototyping component for spawning Player avatars.
/// </summary>
[SimulationBehaviour(Stages = SimulationStages.Forward, Modes = SimulationModes.Server | SimulationModes.Host)]
public class NewCharacterSpawner : SpawnerPrototype<PlayerSpawnPointPrototype>, IPlayerJoined, IPlayerLeft, ISceneLoadDone {

    private bool _firstPlayerLoaded;
    private bool _secondPlayerLoaded;

    public NetworkPrefabRef[] _playerPrefabs;
    public Transform _transform;

#if UNITY_EDITOR

    protected virtual void Reset() {
    var protoPlayer = FusionPrototypingPrefabs.BasicPlayer;
    if (protoPlayer)
      PlayerPrefab = protoPlayer.GetComponent<NetworkObject>();
  }


  [BehaviourButtonAction("Add Player Spawn Point Manager", false, true, nameof(_spawnPointManagerMissing))]
  private void InspectorWarnMissingSpawnPointManager() {
    AddBehaviour<PlayerSpawnPointManagerPrototype>();
  }

#endif

  protected override void RegisterPlayerAndObject(PlayerRef player, NetworkObject playerObject)
    {
        //base.RegisterPlayerAndObject(player, playerObject);

        //Runner.SetPlayerObject(player, playerObject);
        updatePlayers(player, playerObject);
    }

    public void updatePlayers(PlayerRef player, NetworkObject networkPlayerObject)
    {
        Debug.Log(player);
        if (!_firstPlayerLoaded)
        {
            _firstPlayerLoaded = !_firstPlayerLoaded;

            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3(-3, 2, 0);
            //Set the parent object of the spawned networked object
            
            networkPlayerObject.transform.SetParent(_transform);
            networkPlayerObject.transform.position = spawnPosition;
            networkPlayerObject.transform.localScale = new Vector3(1, 1, 1);

            // Keep track of the player avatars so we can remove it when they disconnect
            base.RegisterPlayerAndObject(player, networkPlayerObject);

            Runner.SetPlayerObject(player, networkPlayerObject);

            return;
        }
        else
        {
            _secondPlayerLoaded = !_secondPlayerLoaded;
            // Create a unique position for the player

            Vector3 spawnPosition = new Vector3(3, 2, 0);
            networkPlayerObject.transform.SetParent(_transform);
            networkPlayerObject.transform.position = spawnPosition;
            networkPlayerObject.transform.localScale = new Vector3(1, 1, 1);

            // Keep track of the player avatars so we can remove it when they disconnect
            base.RegisterPlayerAndObject(player, networkPlayerObject);

            Runner.SetPlayerObject(player, networkPlayerObject);
        }
    }

}





