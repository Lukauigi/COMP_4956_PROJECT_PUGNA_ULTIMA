using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class FriendController : MonoBehaviour
{

    List<FriendInfo> _friends = null;

    [SerializeField]
    private Transform _scrollViewContent;

    [SerializeField]
    private GameObject _addButton;

    [SerializeField]
    private TMP_InputField _InputField;

    [SerializeField]
    private GameObject _friendItemPrefab;

    enum FriendIdType {  PlayFabId, Username, Email, DisplayName };

    private void Awake()
    {
        if (_friends == null) GetFriends();
    }

     void ClickAddButton()
    {
        if (_InputField.text.Length > 0)
        {
            //Clear the components before fetching
            FriendItem[] currentItems = _scrollViewContent.GetComponentsInChildren<FriendItem>();
            foreach (FriendItem item in currentItems)
            {
                Destroy(item.gameObject);
            }

            //After clear, Add the friend to DB
            AddFriend(FriendIdType.Username, _InputField.text);
            Debug.Log("Added: " + _InputField.text);

            //Fill the scroll view with friends
            GetFriends();
        }
    }

    void DisplayFriends(List<FriendInfo> friendsCache)
    {
        friendsCache.ForEach(friend =>
        {
            GameObject newFriendItem = Instantiate(_friendItemPrefab, _scrollViewContent);
            if (newFriendItem.TryGetComponent<FriendItem>(out FriendItem item)) {
                item.friendCache = friendsCache;
                item.username.text = friend.Username;
            }
            Debug.Log("CachedFriends: " + friend.Username);
        });
    }

    void DisplayPlayFabError(PlayFabError error) {  Debug.Log(error.GenerateErrorReport()); }

    void DisplayError(string error) { Debug.Log(error); }

    void GetFriends()
    {
        PlayFabClientAPI.GetFriendsList(new PlayFab.ClientModels.GetFriendsListRequest
        {
            IncludeSteamFriends = false,
            IncludeFacebookFriends = false,
            XboxToken = null
        }, result =>
        {
            _friends = result.Friends;
            DisplayFriends(_friends);
        }, DisplayPlayFabError);
    }

    void AddFriend(FriendIdType idType, string friendId)
    {
        var request = new AddFriendRequest();
        switch (idType)
        {
            case FriendIdType.PlayFabId:
                request.FriendPlayFabId = friendId;
                break;
            case FriendIdType.Username:
                request.FriendUsername = friendId;
                break;
            case FriendIdType.Email:
                request.FriendEmail = friendId;
                break;
            case FriendIdType.DisplayName:
                request.FriendTitleDisplayName = friendId;
                break;
        }

        PlayFabClientAPI.AddFriend(request, result =>
        {
            Debug.Log("Friend added succesfully!");
        }, DisplayPlayFabError);
    }

    public void RemoveFriend(FriendInfo friendInfo)
    {
        PlayFabClientAPI.RemoveFriend(new PlayFab.ClientModels.RemoveFriendRequest
        {
            FriendPlayFabId = friendInfo.FriendPlayFabId
        }, results =>
        {
            _friends.Remove(friendInfo);
        }, DisplayPlayFabError);
    }
}
