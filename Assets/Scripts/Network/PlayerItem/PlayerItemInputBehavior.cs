using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class PlayerItemInputBehavior : Fusion.Behaviour, INetworkRunnerCallbacks
{
    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var frameworkInput = new NetworkInputPlayerItem();
        if (Input.GetMouseButtonUp(0))
        {
            frameworkInput.Buttons.Set(NetworkInputPlayerItem.BUTTON_FIRE, true);
        }

        input.Set(frameworkInput);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

}

public struct NetworkInputPlayerItem : INetworkInput
{

    public const int BUTTON_USE = 0;
    public const int BUTTON_FIRE = 1;
    public const int BUTTON_FIRE_ALT = 2;

    public const int BUTTON_FORWARD = 3;
    public const int BUTTON_BACKWARD = 4;
    public const int BUTTON_LEFT = 5;
    public const int BUTTON_RIGHT = 6;

    public const int BUTTON_JUMP = 7;
    public const int BUTTON_CROUCH = 8;
    public const int BUTTON_WALK = 9;

    public const int BUTTON_ACTION1 = 10;
    public const int BUTTON_ACTION2 = 11;
    public const int BUTTON_ACTION3 = 12;
    public const int BUTTON_ACTION4 = 14;

    public const int BUTTON_RELOAD = 15;

    public NetworkButtons Buttons;
    public byte Weapon;
    public Angle Yaw;
    public Angle Pitch;

    public bool IsUp(int button)
    {
        return Buttons.IsSet(button) == false;
    }

    public bool IsDown(int button)
    {
        return Buttons.IsSet(button);
    }
}
