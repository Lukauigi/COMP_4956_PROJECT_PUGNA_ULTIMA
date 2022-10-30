using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// Handles some network functionalities on the player side.
/// Author(s): Jun Earl Solomon
/// Date: Oct 29 2022
/// Source(s):
///     Online multiplayer with Photon Fusion - 2D Arcade Style Car Controller in Unity tutorial Part 12: https://youtu.be/yrXQSM1cleU
///     Online multiplayer ?? FPS Unity & Photon Fusion EP1 (Fusion setup + movement): https://youtu.be/hqIZCoLHOig
/// </summary>
public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    // our local player
    public static NetworkPlayer Local { get; set; }

    /// <summary>
    /// gives input authority to the local player
    /// </summary>
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            Debug.Log("Spawned own character");
        }
        else Debug.Log("Spawned other player's character");
    }

    /// <summary>
    /// despawn the object when it has left the game
    /// </summary>
    /// <param name="player">The player</param>
    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }
}