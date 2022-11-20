
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using System.Threading;
using TMPro;

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
    protected PlayerItemObserver _playerObserver;
    //protected NetworkBehaviour _networkRunnerCallbacks;

    [SerializeField] private Image Avatar;
    [SerializeField] private GameObject nextBtn;
    [SerializeField] private GameObject prevBtn;
    [SerializeField] private GameObject selectBtn;
    [SerializeField] private GameObject diaglogueText;
    [SerializeField] private Color[] Colors;
    [SerializeField] private NetworkObject[] CharacterPrefabs;
    [SerializeField] private int selected;
    [SerializeField] private TMP_Text _username;

    public bool isLocal = true;
    public bool clientJoined{get; set;}
    public bool isReady { get; set;}
    
    public bool isHost { get; set; }
    public bool isClient { get; set; }


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
        if (Object.HasStateAuthority) isLocal = false;
        if (!Object.HasInputAuthority)
        {
            nextBtn.SetActive(false);
            prevBtn.SetActive(false);
            selectBtn.SetActive(false);
        }

        if (!Object.HasStateAuthority) clientJoined = true;
        if (Object.HasStateAuthority) clientJoined = false;
       // if (clientJoined) RPC_UpdatePlayerJoined();

        isReady = false;

        //if (_networkRunnerCallbacks != null) _networkRunnerCallbacks.enabled = true;

    }

    /// <summary>
    /// Author: Roswell Doria
    /// 
    /// Helper function to Initialize components.
    ///
    /// </summary>
    private void CacheComponents()
    {
        if (!_playerItem) _playerItem = GetComponent<PlayerItem>();
        if(!_ncc) _ncc = GetComponent<NetworkCharacterControllerPrototype>();
        if(!_nt ) _nt = GetComponent<NetworkTransform>();
        if (!_playerObserver) _playerObserver = PlayerItemObserver.Observer;
        //if(!_networkRunnerCallbacks) _networkRunnerCallbacks = gameObject.AddComponent<PlayerItemRunnerCallbacks>();

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
        Avatar.color = Colors[selected];

        if(Object.HasInputAuthority)
        {
            RPC_SetPlayerName(PlayerPrefs.GetString("PlayerName"));
        }

        if (isClient)
        {
            Debug.Log("Client is Local------------------------------------------------------------------------");
            
            _playerObserver.RPC_SetPlayerReady(PlayerPrefs.GetInt("ClientID"), selected, isLocal);
        }
        else if(isHost)
        {
            Debug.Log("Host is Local -------------------------------------------------------------------------");
            _playerObserver.RPC_SetPlayerReady(PlayerPrefs.GetInt("HostID"), selected, !isLocal);

        }
        if(!isLocal)
        {
            // Debug.Log("Entered PlayerItemFixedUpdateNetwork from Server Host");
            // Debug.Log("Calling PlayerItemObserver RPC Method START------->");
            // Debug.Log("Observer Object In Item Controller----- :" + _playerObserver);
            // _playerObserver.RPC_SetPlayerReady(PlayerPrefs.GetInt("HostID"), selected, !isLocal);
           // Debug.Log("Calling PlayerItemObserver RPC Method END------->");
        }
        // if (isLocal)
        // {
        //     RPC_ChangeAvatar(Avatar.color);
        // }
        
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

    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_UpdatePlayerJoined()
    {
        //CountdownController.instance.BeginStartGameCountdown();
    }

    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SpawnSelectedPrefab()
    {
        Debug.Log("Entering SelectBtn Click method of ID:" + PlayerPrefs.GetInt("ClientID"));
        Debug.Log("Entering SelectBtn Click method of ID:" + PlayerPrefs.GetInt("HostID"));

        //Set player Ready
        isReady = true;
        
        //Disable PlayerItem buttons
        selectBtn.SetActive(false);
        nextBtn.SetActive(false);
        prevBtn.SetActive(false);
        diaglogueText.SetActive(true);

        Debug.Log("Object has Input Authority: ->>>>>>"+Object.HasInputAuthority);
        if (!Object.HasInputAuthority)
        {
            Debug.Log("Clicked Select  from Client");
            isClient = true;
        }
        else if (Object.HasInputAuthority)
        {
            Debug.Log("Clicked Select from Server");
            isHost = true;
        }
        Debug.Log("isReady? : " + isReady);
        //Vector3 spawnLocation = new Vector3(0, 0, 0);
        //Runner.Spawn(CharacterPrefabs[selected], spawnLocation, Quaternion.identity, PlayerPrefs.GetInt("ClientID"));
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

    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SetPlayerName(string username)
    {
        _username.text = username;
    }
}
