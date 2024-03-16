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
        if (Global.isMultiplayer)
        {
            addFriendBtn.gameObject.SetActive(true);
            if (AlreadyFriend())
            {
                addFriendBtn.gameObject.SetActive(false);
            }

        }
        else
        {
            addFriendBtn.gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        addFriendBtn.onClick.AddListener(() => SendFriendRequest());
    }

    void SendFriendRequest() 
    {
        ChangeInteractable(false);
        PhotonRPCManager.Instance.SendFriendRequest(PlayerProfile.Player_UserID,  GetComponentInParent<InGameProfile>().userId);
    }

    public void ChangeInteractable(bool state) 
    {
        addFriendBtn.interactable = state;

        addImage.SetActive(state);
        sentImage.SetActive(!state);
    }

    public bool AlreadyFriend() 
    {
        return PlayerProfile.instance.friendsUsers.Exists(x=> x.UserId.Equals(GetComponentInParent<InGameProfile>().userId));
    }
}
