
using Fusion;

/// <summary>
/// Author: Roswell Doria
/// Date: 2022-12-03
/// 
/// DEPRICATED: Currently not being used and is safe to remove from the project.
/// 
/// Prototyping component for spawning Player avatars.
/// </summary>
[SimulationBehaviour(Stages = SimulationStages.Forward, Modes =  SimulationModes.Server | SimulationModes.Host)]
public class PlayerSpawner : SpawnerPrototype<PlayerSpawnPointPrototype>, IPlayerJoined, IPlayerLeft, ISceneLoadDone {

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

  protected override void RegisterPlayerAndObject(PlayerRef player, NetworkObject playerObject) {
    base.RegisterPlayerAndObject(player, playerObject);

    Runner.SetPlayerObject(player, playerObject);
  }

}





