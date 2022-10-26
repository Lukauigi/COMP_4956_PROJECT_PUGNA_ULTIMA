using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public static class UserData
{

    public static void SetUserData(string key, string value)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
            { key, value }
        }
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public static void GetUserData(string myPlayFabId, string key)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result => {
            Debug.Log("Got user data:");
            //if (result.Data == null || !result.Data.ContainsKey("Info")) Debug.Log("No Info");
            //else Debug.Log("Info: " + result.Data["Info"].Value);
            Debug.Log("Database data: " + result.Data[key].Value);
        }, (error) => {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
