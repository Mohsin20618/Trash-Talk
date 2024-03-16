using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendRequestPopUp : MonoBehaviour
{
    public string senderId;

    public Button acceptRequestBtn;
    public Button rejectRequestBtn;

    // Start is called before the first frame update
    void Start()
    {
        acceptRequestBtn.onClick.AddListener(() => AcceptRequestCallBack());
        rejectRequestBtn.onClick.AddListener(() => RejectRequestCallBack());
    }

    private void AcceptRequestCallBack()
    {
        PhotonRPCManager.Instance.AcceptRequest(PlayerProfile.Player_UserID, senderId);
        //Hit Friend Api
        ShowHidePopUp(false);

    }

    private void RejectRequestCallBack()
    {
        PhotonRPCManager.Instance.RejectRequest(PlayerProfile.Player_UserID, senderId);
        ShowHidePopUp(false);
    }

    // Update is called once per frame
    public void SetData(string senderId)
    {
        this.senderId = senderId;
        ShowHidePopUp(true);
    }


    void ShowHidePopUp(bool state) 
    {
        gameObject.SetActive(state);
    }


}
