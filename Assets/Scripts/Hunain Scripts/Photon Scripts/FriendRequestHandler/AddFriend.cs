using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddFriend : MonoBehaviour
{
    public Button addFriendBtn;
    public GameObject addImage;
    public GameObject sentImage;

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

        addImage.SetActive(state);
        sentImage.SetActive(!state);
    }
}
