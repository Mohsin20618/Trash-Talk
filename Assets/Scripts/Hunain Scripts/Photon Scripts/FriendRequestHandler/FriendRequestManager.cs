using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FriendRequestManager : Singleton<FriendRequestManager>
{
    public List<PlayerUI> profiles = new();


    // Update is called once per frame
    public void RequestSentCallBack(string senderId, string recieverId)
    {
        if (senderId == PlayerProfile.Player_UserID) //it's me
        {
            Debug.Log("Request Sent Successfully.");
            return;
        }

        if (recieverId == PlayerProfile.Player_UserID) //Someone send me a friend Request
        {
            Debug.Log(senderId +   " send me a friend request.");

            ShowRequestPopUp(senderId);
            return;
        }

        Debug.Log("Someone send friend request to someone!! Security Breach. Hahaha");
    }

    #region Requests Handler
    public void ShowRequestPopUp(string senderId) 
    {
        PlayerUI senderUI = ReturnUiPlayerObj(senderId);
        Debug.Log("Sender Name: " + senderUI.playerData.name , senderUI.gameObject);
        senderUI.friendRequestPopUp.SetData(senderId);
       
        //senderUI.addFriendBtn.ChangeInteractable(false);
    }



    public void RequestAcceptCallback(string senderId, string recieverId)
    {
        if (PlayerProfile.Player_UserID == recieverId)
        {
            PlayerUI senderUI = ReturnUiPlayerObj(senderId);

            Debug.Log(senderUI.nameText.text + " accept my friend Request");
       
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();

            string userId1 = recieverId;
            string userId2 = senderId;
            keyValuePairs.Add("user_id1", userId1);
            keyValuePairs.Add("user_id2", userId2);

            WebServiceManager.instance.APIRequest(WebServiceManager.instance.addFriends, Method.POST, null, keyValuePairs, OnFriendShipSuccess, OnFail, CACHEABLE.NULL, false, null);
        }
    }

    private void OnFriendShipSuccess(JObject arg1, long arg2)
    {
        Debug.Log(arg1.ToString());
    }

    private void OnFail(string obj)
    {
        Debug.LogError(obj);
    }

    public void RequestRejectCallback(string senderId, string recieverId)
    {
        if (PlayerProfile.Player_UserID == recieverId)
        {
            PlayerUI senderUI = ReturnUiPlayerObj(senderId);

            Debug.Log(senderUI.nameText.text + " reject my friend Request");
        }
    }

    #endregion


    PlayerUI ReturnUiPlayerObj(string id) 
    {
        return profiles.Find(user => user.userId.Equals(id));
    }
}
