using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

/// <summary>
/// Author: Roswell Doria
/// Date: 2022-11-26
/// 
/// This script is responbile for controlling the FriendList prefab.
///
/// </summary>
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

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-26
    ///
    /// Enumerated types of types from PlayFab API to display user details.
    ///
    /// </summary>
    enum FriendIdType {  PlayFabId, Username, Email, DisplayName };

    private void Awake()
    {
        if (_friends == null) GetFriends();
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-26
    ///
    /// This fuction is responsible for Adding friends. The function will reset the scrollView content, Add the friend with PlayFab api
    /// and create instantiate each friend as a FriendItem prefab.
    ///
    /// </summary>
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

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-26
    /// 
    /// This function is responsible for instantiating friendItem prefabs for each friend in the friendsCache.
    ///
    /// </summary>
    /// <param name="friendsCache"></param>
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

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-26
    /// 
    /// This function is responsible for displaying PlayFab errors as console logs.
    ///
    /// </summary>
    /// <param name="error"></param>
    void DisplayPlayFabError(PlayFabError error)
    { 
        Debug.Log(error.GenerateErrorReport());
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-26
    /// 
    /// This function Displays a string paramter as a console log.
    ///
    /// </summary>
    /// <param name="error">String represneting an error msg</param>
    void DisplayError(string error)
    {
        Debug.Log(error);
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-26
    /// 
    /// This function is repsonbile for calling PlayFab api to obtain the friends from the DB and store them in a cache.
    /// This function will then call the DisplayFriends function on call.
    ///
    /// </summary>
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

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-26
    /// 
    /// This function takes in a friendID and the friend type and makes the appropiate request call to PlayFab DB based on type.
    ///
    /// </summary>
    /// <param name="idType">A string of friend's ID</param>
    /// <param name="friendId">Am emum type of friend ID</param>
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

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-11-26
    /// 
    /// This function is responbile for removing friends using PlayFab API and modifying the friends cache.
    ///
    /// </summary>
    /// <param name="friendInfo"></param>
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
