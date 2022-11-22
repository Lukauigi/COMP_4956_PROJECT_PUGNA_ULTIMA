using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.Party;
using PlayFab.ClientModels;
public class ChatControl : MonoBehaviour
{
    // Log into playfab
var request = new LoginWithCustomIDRequest { CustomId = UnityEngine.Random.value.ToString(), CreateAccount = true };
PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);

private void OnLoginSuccess(LoginResult result)
{
    string networkId = "<your network id>";
PlayFabMultiplayerManager.Get().JoinNetwork(networkId);
PlayFabMultiplayerManager.Get().OnNetworkJoined += OnNetworkJoined;
PlayFabMultiplayerManager.Get().OnRemotePlayerJoined += OnRemotePlayerJoined;
PlayFabMultiplayerManager.Get().OnRemotePlayerLeft += OnRemotePlayerLeft;
PlayFabMultiplayerManager.Get().OnDataMessageReceived += LocalPlayer_OnDataMessageReceived;
PlayFabMultiplayerManager.Get().OnChatMessageReceived += OnChatMessageReceived;
}
  private void OnNetworkJoined(object sender, string networkId)
    {
        // Print the Network ID so you can give it to the other client.
        Debug.Log("Network joined!");
    }
private void OnLoginFailure(PlayFabError error)
{
}

PlayFabMultiplayerManager.Get().CreateAndJoinNetwork();
PlayFabMultiplayerManager.Get().OnNetworkJoined += OnNetworkJoined;
private void OnNetworkJoined(object sender, string networkId)
{
    // Print the Network ID so you can give it to the other client.
    Debug.Log(networkId);
}
private void OnRemotePlayerLeft(object sender, PlayFabPlayer player)
{
}

private void OnRemotePlayerJoined(object sender, PlayFabPlayer player)
{
    var localPlayer = PlayFabMultiplayerManager.Get().LocalPlayer;
}
private void OnDataMessageReceived(object sender, PlayFabPlayer from, byte[] buffer)
{
    if (Input.GetButtonDown("Fire1"))
{
    byte[] requestAsBytes = Encoding.UTF8.GetBytes("Hello (data message)");
    PlayFabMultiplayerManager.Get().SendDataMessageToAllPlayers(requestAsBytes);
}
    Debug.Log(Encoding.Default.GetString(buffer));

}
private void OnChatMessageReceived(object sender, PlayFabPlayer from, string message, ChatMessageType type)
{
    Debug.Log(message);
}
}
