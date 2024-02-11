using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddFriend : MonoBehaviour
{
    public Button addFriendBtn;

    private void Awake()
    {
        addFriendBtn.gameObject.SetActive(Global.isMultiplayer);
    }
    private void Start()
    {
        addFriendBtn.onClick.AddListener(() => SendFriendRequest());
    }

    void SendFriendRequest() 
    {
        ChangeInteractable(false);
        PhotonRPCManager.Instance.SendFriendRequest(PlayerProfile.Player_UserID, GetComponent<PlayerUI>().userId);
    }

    public void ChangeInteractable(bool state) 
    {
        addFriendBtn.interactable = state;
    }
}
