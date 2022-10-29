
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using System.Threading;

/// <summary>
/// Author: Roswell Doria
/// Date: 2022-10-29
/// 
/// Network Controller for PlayerItem Prefab. 
/// This is responsible for functionality of the Prefab object across the network.
///
/// </summary>
public class PlayerItemController : NetworkBehaviour
{
    protected NetworkCharacterControllerPrototype _ncc;
    protected PlayerItem _playerItem;
    protected NetworkTransform _nt;

    [SerializeField] private Image Avatar;
    [SerializeField] private Color[] Colors;
    [SerializeField] private int selected;

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-10-29
    /// 
    /// Method calls before a network playerItem is spawned.
    ///
    /// </summary>
    public void Awake()
    {
        CacheComponents();
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-10-29
    ///
    /// Method calls when network playerItem is spawned.
    ///
    /// </summary>
    public override void Spawned()
    {
        CacheComponents();
    }

    /// <summary>
    /// Author: Roswell Doria
    /// 
    /// Helper function to Initilize components.
    ///
    /// </summary>
    private void CacheComponents()
    {
        if (!_playerItem) _playerItem = GetComponent<PlayerItem>();
        if(!_ncc) _ncc = GetComponent<NetworkCharacterControllerPrototype>();
        if(!_nt ) _nt = GetComponent<NetworkTransform>();

        selected = 0;
        //if(!_color) _color = GetComponent<NetworkColor>();
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-10-29
    /// 
    /// Updates the network of a local client's changes to their ownership of this networked object.
    ///
    /// </summary>
    public override void FixedUpdateNetwork()
    {

        if (Avatar)
        {
            RPC_ChangeAvatar(Colors[selected]);
        }
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-10-29
    ///
    /// Remote procedure call used to update the avatar of the input authortive object and notify
    /// all other connected clients of this update.
    ///
    /// </summary>
    /// <param name="selectedColor"></param>
    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ChangeAvatar(Color selectedColor)
    {
        Avatar.color = selectedColor;
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-10-29
    ///
    /// Remote procedure call used to update the selected character of the input authoritve object and notify
    /// all other connected clients of this update. The intended use of this method is to be attached to a next button.
    /// </summary>
    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_OnNextBtnClick()
    {
        selected = (selected + 1) % Colors.Length;
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-10-29
    ///
    /// Remote procedure call used to update the selected character of the input authoritve object and notify
    /// all other connected clients of this update. The intended use of this method is to be attached to a previous button.
    /// </summary>
    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_OnPrevBtnClick()
    {
        selected--;
        if (selected < 0) selected = Colors.Length - 1;
    }
}
