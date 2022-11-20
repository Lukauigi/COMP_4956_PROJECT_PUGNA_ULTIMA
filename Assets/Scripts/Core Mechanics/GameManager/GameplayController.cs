
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using System.Threading;

/// <summary>
/// Author: Jason Cheung, Roswell Doria
/// Date: 2022-11-06
/// 
/// Network Controller for Gameplay Prefab. 
/// This is responsible for functionality of the Prefab object across the network.
///
/// Remark(s): class is currently unused; may be implemented in lieu of replacing GameManager, and/or GameTimerControllers
/// </summary>
public class GameplayController : NetworkBehaviour
{
    protected NetworkCharacterControllerPrototype _ncc;
    protected PlayerItem _playerItem;
    //protected Gameplay _gameplay; // todo (jason): define Gameplay class
    protected NetworkTransform _nt;

    [SerializeField] private Image Avatar;
    [SerializeField] private GameObject nextBtn;
    [SerializeField] private GameObject prevBtn;
    [SerializeField] private Color[] Colors;
    [SerializeField] private int selected;

    public bool isLocal = false;

    /// <summary>
    /// Author: Jason Cheung, Roswell Doria
    /// Date: 2022-11-06
    /// 
    /// Method calls before a network playerItem is spawned.
    ///
    /// </summary>
    public void Awake()
    {
        CacheComponents();
    }

    /// <summary>
    /// Author: Jason Cheung, Roswell Doria
    /// Date: 2022-11-06
    ///
    /// Method calls when network Gameplay is spawned.
    ///
    /// </summary>
    public override void Spawned()
    {
        CacheComponents();
        if (Object.HasStateAuthority) isLocal = false;
        if (!Object.HasInputAuthority)
        {
            nextBtn.SetActive(false);
            prevBtn.SetActive(false);
        }
    }

    /// <summary>
    /// Author: Jason Cheung, Roswell Doria
    /// Date: 2022-11-06
    /// 
    /// Helper function to Initialize components.
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
    /// Author: Jason Cheung, Roswell Doria
    /// Date: 2022-11-06
    /// 
    /// Updates the network of a local client's changes to their ownership of this networked object.
    ///
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        Avatar.color = Colors[selected];
        if (isLocal)
        {
            RPC_ChangeAvatar(Avatar.color);
        }
    }

    /// <summary>
    /// Author: Jason Cheung, Roswell Doria
    /// Date: 2022-11-06
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
    /// Author: Jason Cheung, Roswell Doria
    /// Date: 2022-11-06
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
    /// Author: Jason Cheung, Roswell Doria
    /// Date: 2022-11-06
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
