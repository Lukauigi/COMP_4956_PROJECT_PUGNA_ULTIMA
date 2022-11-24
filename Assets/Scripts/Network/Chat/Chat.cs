using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Author: Roswell Doria
/// Date: 2022-11-24
/// 
/// Chat network behavior responsible for sending chat msgs between users using a chat prefab
///
/// </summary>
public class Chat : NetworkBehaviour
{

    [SerializeField] private GameObject _chatUI;
    [SerializeField] private TMP_Text _chatText;
    [SerializeField] private TMP_InputField _chatInputField;
    [SerializeField] private GameObject _sendBtn;
    
    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-24
    /// 
    /// OnClick method for send msg button.
    ///
    /// </summary>
    public void clickSendBtn()
    {
        if(Runner.IsServer) RPC_SendChat(_chatInputField.text, PlayerPrefs.GetString("PlayerName"));
        else RPC_SendClientChat(_chatInputField.text, PlayerPrefs.GetString("PlayerName"));
        _chatInputField.text = "";
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-24
    /// 
    /// RPC call responsbile for sending msgs from server host to all remote clients.
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="sender"></param>
    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SendChat(string message, string sender)
    {
        Debug.Log("Clicked send server");
        _chatText.text += "[" + sender + "] : " + message + "\n";
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-24
    /// 
    /// RPC call responsible for sneding msgs from clients to all remote simulationrs.
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="sender"></param>
    [Rpc(sources: RpcSources.Proxies, RpcTargets.All)]
    public void RPC_SendClientChat(string message, string sender)
    {
        Debug.Log("Clicked send client");
        _chatText.text += "[" + sender + "] : " + message + "\n";
    }
}
