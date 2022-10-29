
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using System.Threading;

public class PlayerItemController : NetworkBehaviour
{
    protected NetworkCharacterControllerPrototype _ncc;
    protected PlayerItem _playerItem;
    protected NetworkTransform _nt;

    [SerializeField] private Image Avatar;
    [SerializeField] private Color[] Colors;
    [SerializeField] private int selected;

    public void Awake()
    {
        CacheComponents();
    }

    public override void Spawned()
    {
        CacheComponents();
    }

    private void CacheComponents()
    {
        if (!_playerItem) _playerItem = GetComponent<PlayerItem>();
        if(!_ncc) _ncc = GetComponent<NetworkCharacterControllerPrototype>();
        if(!_nt ) _nt = GetComponent<NetworkTransform>();

        selected = 0;
        //if(!_color) _color = GetComponent<NetworkColor>();
    }

    public override void FixedUpdateNetwork()
    {

        if (Avatar)
        {
            RPC_ChangeAvatar(Colors[selected]);
        }
    }

    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ChangeAvatar(Color selectedColor)
    {
        Avatar.color = selectedColor;
    }

    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_OnNextBtnClick()
    {
        selected = (selected + 1) % Colors.Length;
    }

    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_OnPrevBtnClick()
    {
        selected--;
        if (selected < 0) selected = Colors.Length - 1;
    }
}
