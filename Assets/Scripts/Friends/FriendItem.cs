using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab.ClientModels;
using PlayFab;
using UnityEngine.EventSystems;

/// <summary>
/// Author: Roswell Doria
/// Date: 2022-11-26
/// 
/// This script is responsible controlling the behavior for friendItem Prefabs that are dynamically created inside the friendsList prefab.
///
/// Change History: 2022-11-28 - Lukasz Bednarek
/// - Add method calls to audio effects manager static member.
/// - Implement interface IPointerEventHandler to utilize audio cue on OnPointerEnter event
/// </summary>
public class FriendItem : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    public TMP_Text username;

    [SerializeField]
    public GameObject removeBtn;

    public List<FriendInfo> friendCache;

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-16
    /// 
    /// This function is responsible for removing this selected FriendItem from the friends cache of the logged in user.
    ///
    /// </summary>
    public void RemoveFriendBtnClick()
    {
        if (friendCache != null)
        {
            friendCache.ForEach(friend => { 
                if (friend.Username == username.text)
                {
                    PlayFabClientAPI.RemoveFriend(new PlayFab.ClientModels.RemoveFriendRequest
                    {
                        FriendPlayFabId = friend.FriendPlayFabId
                    }, results =>
                    {
                        friendCache.Remove(friend);
                        Destroy(this.gameObject);
                        AudioEffectsManager.Instance2.PlaySoundClipOnce(MenuActions.Confirm);
                    }, DisplayPlayFabError);
                }
            });
        }
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-16
    /// 
    /// This function is responsible for displaying PlayFab API errors for console logging.
    ///
    /// </summary>
    /// <param name="error"></param>
    void DisplayPlayFabError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    /// <inheritdoc/>
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioEffectsManager.Instance2.PlaySoundClipOnce(MenuActions.Navigate);
    }
}
