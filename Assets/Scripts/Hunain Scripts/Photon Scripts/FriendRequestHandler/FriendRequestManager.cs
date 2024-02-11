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
            ShowRequestPopUp(senderId);
            return;
        }

        Debug.Log("Someone send friend request to someone!! Security Breach. Hahaha");
    }

    #region Requests Handler
    public void ShowRequestPopUp(string senderId) 
    {
        PlayerUI senderUI = ReturnUiPlayerObj(senderId);
        senderUI.friendRequestPopUp.SetData(senderId);
       
        senderUI.addFriendBtn.ChangeInteractable(false);
    }



    public void RequestAcceptCallback(string senderId, string recieverId)
    {
        if (PlayerProfile.Player_UserID == recieverId)
        {
            PlayerUI senderUI = ReturnUiPlayerObj(senderId);

            Debug.Log(senderUI.nameText.text + " accept my friend Request");
        }
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
