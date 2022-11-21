using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chat : NetworkBehaviour
{

    [SerializeField] private GameObject _chatUI;
    [SerializeField] private TMP_Text _chatText;
    [SerializeField] private TMP_InputField _chatInputField;
    [SerializeField] private GameObject _sendBtn;
    
    public void clickSendBtn()
    {
        if(Runner.IsServer) RPC_SendChat(_chatInputField.text, PlayerPrefs.GetString("PlayerName"));
        else RPC_SendClientChat(_chatInputField.text, PlayerPrefs.GetString("PlayerName"));
        _chatInputField.text = "";
    }

    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SendChat(string message, string sender)
    {
        Debug.Log("Clicked send server");
        _chatText.text += "[" + sender + "] : " + message + "\n";
    }

    [Rpc(sources: RpcSources.Proxies, RpcTargets.All)]
    public void RPC_SendClientChat(string message, string sender)
    {
        Debug.Log("Clicked send client");
        _chatText.text += "[" + sender + "] : " + message + "\n";
    }
}
