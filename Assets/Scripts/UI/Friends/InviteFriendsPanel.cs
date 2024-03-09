using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TrashTalk;
using Photon.Pun;

public class InviteFriendsPanel : MonoBehaviour
{
    public Transform container;
    public GameObject friendItem;
    public Button sendRequestBtn;
    public GameObject emptyDataText;
    public ListType currentListType;

    public enum ListType 
    {
        friends,
        recent,
        global
    }
    //  List<GameObject> selectedList;
    [SerializeField] public List<User> selectedUsers;

    private void Start()
    {
        //    selectedList = new List<GameObject>();
    }

    private void OnEnable()
    {
        selectedUsers = new List<User>();
        sendRequestBtn.interactable = false;
        StartCoroutine(ShowList());
    }

    private void OnDisable()
    {
        selectedUsers = new List<User>();
        ClearContianer(container);
    }
    public List<User> GetUsers()
    {
        List<User> users = new();

        switch (currentListType)
        {
            case ListType.friends:
                users = PlayerProfile.instance.friendsUsers;
                break;
            case ListType.recent:
                users = PlayerProfile.instance.recentUsers;
                break;
            case ListType.global:
                users = PlayerProfile.instance.globalUsers;

                break;
            default:
                break;
        }
        return users;   
    }

    IEnumerator ShowList()
    {
        yield return null;
        List<User> users = GetUsers();

        print("users COUNT: " + users.Count);

        emptyDataText.SetActive(users.Count == 0);

        foreach (User user in users)
        {
            yield return null;

            GameObject obj = Instantiate(friendItem, container, false);
            obj.GetComponent<FriendItem>().SetData(transform.GetSiblingIndex(), user, OnSelect);
        }

        yield return null;

        //for (int i = 0; i < users.Count; i++)
        //{
        //    GameObject obj = Instantiate(friendItem, container, false);
        //    obj.GetComponent<FriendItem>().SetData( transform.GetSiblingIndex(), OnSelect);
        //}
    }

     public void OnSelect(User user, bool isSelected)
    {
        if (isSelected)
            selectedUsers.Add(user);
        else
            selectedUsers.Remove(user);
       
        sendRequestBtn.interactable = (selectedUsers.Count > 0);
    }

    public void OnInviteButton()
    {
        if(PlayerProfile.Player_coins < Global.coinsRequired)
        {
            UIEvents.ShowPanel(Panel.Popup);
            UIEvents.UpdateData(Panel.Popup, (data) => {}, "SetData", $"You need at least {Global.coinsRequired} Coins!", "", "OK");

            return;
        }

        //UIEvents.ShowPanel(Panel.Popup);
        //UIEvents.UpdateData(Panel.Popup, null, "SetData", "Invite sent to your selected friends", "","OK");

        string roomName = Global.roomName;

        Debug.Log("roomName : "  +roomName);

        foreach (var friendsScreen in transform.parent.GetComponentsInChildren<InviteFriendsPanel>())
        {
            foreach (var selectedUser in friendsScreen.selectedUsers)
            {
                SendGameRequest(roomName, PlayerProfile.Player_UserName, PlayerProfile.Player_UserID, selectedUser.UserId);
            }
        }

        UIEvents.HidePanel(Panel.FriendsPanel);
        UIEvents.ShowPanel(Panel.GameplayPanel);

    }

    void SendGameRequest(string roomName, string playerName, string playerUserID, string friendUserID)
    {
        Debug.Log(roomName);
        Debug.Log(playerName);
        Debug.Log(playerUserID);
        Debug.Log(friendUserID);

        //SendGameRequest to friend using Pun chat
        if (PhotonChat.Instance.chatClient == null)
        {
            Debug.LogError("Chat Client was null");
            PhotonChat.Instance.Connect();
        }
        PhotonChat.Instance.RequestAndSendMessage(friendUserID, roomName,()=>{

         //   UIEvents.HidePanel(Panel.TabPanels);
         //   UIEvents.ShowPanel(Panel.GameplayPanel);

            //UIEvents.ShowPanel(Panel.Popup);
            //UIEvents.UpdateData(Panel.Popup, (data)=> {
            //if ((int)data[0] == 2)//on ok
            //{
            //        Global.isMultiplayer = true;
            //    //    UIEvents.HidePanel(Panel.TabPanels);
            //    //    UIEvents.ShowPanel(Panel.GameplayPanel);
            //}

            //}, "SetData", "Invite sent to your selected friends", "", "OK");
        });

        PhotonRoomCreator.instance.CreateRoomOnPhoton(false, roomName);

    }



    void ClearContianer(Transform container)
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
    }
}
