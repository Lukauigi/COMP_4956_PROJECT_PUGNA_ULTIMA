using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab.ClientModels;
using PlayFab;

public class FriendItem : MonoBehaviour
{
    [SerializeField]
    public TMP_Text username;

    [SerializeField]
    public GameObject removeBtn;

    public List<FriendInfo> friendCache;

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
                    }, DisplayPlayFabError);
                }
            });
        }
    }

    void DisplayPlayFabError(PlayFabError error) { Debug.Log(error.GenerateErrorReport()); }
}
