using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

/// <summary>
/// Handles some network functionalities on the player side.
/// Author(s): Jun Earl Solomon, Jason Cheung
/// Date: Oct 29 2022
/// Source(s):
///     Online multiplayer with Photon Fusion - 2D Arcade Style Car Controller in Unity tutorial Part 12: https://youtu.be/yrXQSM1cleU
///     Online multiplayer FPS Unity & Photon Fusion EP1 (Fusion setup + movement): https://youtu.be/hqIZCoLHOig
///     Online multiplayer FPS Unity & Photon Fusion EP4.1 (player names and RPCs): https://youtu.be/-opvmn_QKw0
/// Change History: Nov 20 2022 - Jason Cheung
/// - added networked property nickname that's above the fighter prefab's head
/// </summary>
public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    // our local player
    public static NetworkPlayer Local { get; set; }

    // fighter prefab UI components
    [SerializeField] private TextMeshProUGUI _playerNickname;

    // networked property of the fighter's nickname; listens for OnChanged and notifies others
    private NetworkString<_16> nickName;
    [Networked(OnChanged = nameof(OnNickNameChanged)), UnityNonSerialized]
    public NetworkString<_16> NickName
    {
        get
        {
            return nickName;
        }
        set
        {
            nickName = value;
            // client update changes to host
            if (Object.HasInputAuthority)
            {
                RPC_SetNickName(value);
            }
        }
    }

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
        else
        {
            Debug.Log("Spawned other player's character");
        }
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

    // Networked OnChanged method for the Network Property Stocks
    static void OnNickNameChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.OnNickNameChanged();
    }

    // OnChanged method to update the nickname displayed on top of their fighter
    private void OnNickNameChanged()
    {
        Debug.Log($"Nickname changed to {NickName} for player ref: ->");
        _playerNickname.text = NickName.ToString();
    }

    // RPC method for client to notify host its changes for fighter nickname
    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetNickName(NetworkString<_16> nickName, RpcInfo info = default)
    {
        Debug.Log($"[RPC] SetNickName {NickName}");
        this.NickName = nickName;
    }

}