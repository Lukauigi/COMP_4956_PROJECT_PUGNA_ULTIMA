using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class addFriends : MonoBehaviour
 {
        [SerializeField] private string displayName;

        public static Action<string> OnAddFriend = delegate { };

        public void SetAddFriendName(string name)
        {
            displayName = name;
        }
        public void AddFriend()
        {
            Debug.Log($"UI Add Friend Clicked: {displayName}");
            if (string.IsNullOrEmpty(displayName)) return;
            OnAddFriend?.Invoke(displayName);
        }
    }

