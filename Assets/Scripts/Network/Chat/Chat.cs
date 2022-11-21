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
        if(HasStateAuthority) RPC_SendChat(_chatInputField.text, PlayerPrefs.GetString("PlayerName"));
        if (HasInputAuthority) RPC_SendClientChat(_chatInputField.text, PlayerPrefs.GetString("PlayerName"));
    }

    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SendChat(string message, string sender)
    {
        _chatText.text += "[" + sender + "] : " + message + "\n";
    }

    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SendClientChat(string message, string sender)
    {
        _chatText.text += "[" + sender + "] : " + message + "\n";
    }
}
