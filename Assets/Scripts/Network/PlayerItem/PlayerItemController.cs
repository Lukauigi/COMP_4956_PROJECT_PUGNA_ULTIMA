
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
/// Change History:
/// 2022-11-23 - Lukasz Bednarek
/// - Added sound to button presses
/// 2022-11-21 - Roswell Doria
/// - Added private field for remote username
/// - Added RPC for sending remote username to State authority
///
/// </summary>
public class PlayerItemController : NetworkBehaviour
{
    protected NetworkCharacterControllerPrototype _ncc;
    protected PlayerItem _playerItem;
    protected NetworkTransform _nt;
    protected PlayerItemObserver _playerObserver;
    //protected NetworkBehaviour _networkRunnerCallbacks;
    protected GameObject _audioManager;

    [SerializeField] private Image Avatar;
    [SerializeField] private GameObject nextBtn;
    [SerializeField] private GameObject prevBtn;
    [SerializeField] private GameObject selectBtn;
    [SerializeField] private GameObject dialogueText;
    [SerializeField] private Sprite[] Avatars;
    [SerializeField] private NetworkObject[] CharacterPrefabs;
    [SerializeField] private int selected;
    [SerializeField] private TMP_Text _username;

    public bool isLocal = true;
    public bool clientJoined { get; set;}


    private bool isHostReady = false;
    private bool isClientReady = false;

    private bool isPlayersReady = false;

    private string remoteUsername;

    // Database id's needed for mutliplayer
    private string _remoteId;
    private string _playerId;


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

    public void Start()
    {
        CacheOtherObjects();
        this._audioManager = GameObject.Find("SceneAudioManager");
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-10-29
    ///
    /// Method calls when network playerItem is spawned.
    ///
    /// Change History:
    /// 2022-11-21 - Roswell Doria
    /// - Added Call to RPC_SetRemoteUsername() to set remote player's username to state authority
    ///     
    /// </summary>
    public override void Spawned()
    {
        //Make chat visable for the client that spawned.
        Chat.Instance.ChatVisible(true);

        if (Object.HasStateAuthority) isLocal = false;
        if (!Object.HasInputAuthority)
        {
            nextBtn.SetActive(false);
            prevBtn.SetActive(false);
            selectBtn.SetActive(false);
        }

        if (!Object.HasStateAuthority) clientJoined = true;
        if (Object.HasStateAuthority) clientJoined = false;

        if (!Object.HasStateAuthority && Object.HasInputAuthority) RPC_SetRemoteUsername(PlayerPrefs.GetString("PlayerName"));
        if (!Object.HasStateAuthority && Object.HasInputAuthority) RPC_SetRemoteId(PlayerPrefs.GetString("PlayfabId"));

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
        //if(!_networkRunnerCallbacks) _networkRunnerCallbacks = gameObject.AddComponent<PlayerItemRunnerCallbacks>();

        selected = 0;
        //if(!_color) _color = GetComponent<NetworkColor>();
        
    }

    private void CacheOtherObjects()
    {
        if (!_playerObserver) _playerObserver = PlayerItemObserver.Observer;
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-10-29
    /// 
    /// Updates the network of a local client's changes to their ownership of this networked object.
    /// 
    /// Changes: Ross 2022-11-21
    ///     - Modified call to RPC_SetPlayerReady() to take paramters for usernames
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        // update to the selected avatar sprite
        Avatar.sprite = Avatars[selected];

        //Display This player's username
        if(Object.HasInputAuthority)
        {
            RPC_SetPlayerName(PlayerPrefs.GetString("PlayerName"));
            RPC_SetPlayerId(PlayerPrefs.GetString("PlayfabId"));
        }

        // only the Host has state authority; this removes the 'Local simulation is not allowed' errors on Client
        if (Object.HasStateAuthority && !isPlayersReady)
        {
            if (isClientReady)
            {
                //Debug.Log("Client is Local------------------------------------------------------------------------");
                _playerObserver.RPC_SetPlayerReady(PlayerPrefs.GetInt("ClientID"), selected, isLocal, remoteUsername, _remoteId);
            }
            if (isHostReady)
            {
                //Debug.Log("Host is Local -------------------------------------------------------------------------");
                _playerObserver.RPC_SetPlayerReady(PlayerPrefs.GetInt("HostID"), selected, !isLocal, _username.text, _playerId);
            }

            if (isClientReady && isHostReady)
            {
                isPlayersReady = true;
            }
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
    /// Date: 2022-11-10
    ///
    /// Remote procedure call used to finalize the selected character of the input authoritve object and notify
    /// all other connected clients of this update. The intended use of this method is to be attached to a select button.
    /// </summary>
    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SpawnSelectedPrefab()
    {
        Debug.Log("Entering SelectBtn Click method of ID:" + PlayerPrefs.GetInt("ClientID"));
        Debug.Log("Entering SelectBtn Click method of ID:" + PlayerPrefs.GetInt("HostID"));
        
        //Disable PlayerItem buttons
        selectBtn.SetActive(false);
        nextBtn.SetActive(false);
        prevBtn.SetActive(false);
        dialogueText.SetActive(true);

        InitiateAudio(true);

        Debug.Log("Object has Input Authority: --->>> " + Object.HasInputAuthority);
        if (!Object.HasInputAuthority)
        {
            Debug.Log("Clicked Select from Client");
            isClientReady = true;
        }
        else if (Object.HasInputAuthority)
        {
            Debug.Log("Clicked Select from Server");
            isHostReady = true;
        }

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
        selected = (selected + 1) % Avatars.Length;
        InitiateAudio(false); //plays audio only for client responsible for RPC.
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
        if (selected < 0) selected = Avatars.Length - 1;
        InitiateAudio(false); //plays audio only for client responsible for RPC.
    }

    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SetPlayerName(string username)
    {
        _username.text = username;
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-21
    /// 
    /// Remote procedure to inform the state authority the username of the remote client.
    ///
    /// </summary>
    /// <param name="username"></param>
    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetRemoteUsername(string username)
    {
        remoteUsername = username;
    }
    
    /// <summary>
    /// Author: Justin Payne
    /// Date: 2022-11-23
    /// 
    /// Remote procedure to inform the state authority the azure playfab id of the remote client.
    /// </summary>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetRemoteId(string id)
    {
        _remoteId = id;
    }
    
    /// <summary>
    /// Author: Justin Payne
    /// Date: 2022-11-23
    /// 
    /// Remote procedure to inform the state authority the azure playfab id of the remote client.
    /// </summary>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SetPlayerId(string id)
    {
        _playerId = id;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="isCharacterSelection">If button press is selection button press.</param>
    private void InitiateAudio(bool isCharacterSelection)
    {
        if (!isCharacterSelection) _audioManager.GetComponent<GameplayAudioManager>().PlayMenuSFXAudio(MenuActions.Navigate.ToString());
        else _audioManager.GetComponent<GameplayAudioManager>().PlayMenuSFXAudio(MenuActions.Confirm.ToString());
    }
}
