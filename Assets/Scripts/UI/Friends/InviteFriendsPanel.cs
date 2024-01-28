using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TrashTalk;

public class InviteFriendsPanel : MonoBehaviour
{
    public Transform container;
    public GameObject friendItem;
    public Button inviteButton;
    public GameObject emptyDataText;

    //  List<GameObject> selectedList;
    [SerializeField] public List<User> selectedUsers;

    private void Start()
    {
        //    selectedList = new List<GameObject>();
    }

    private void OnEnable()
    {
        selectedUsers = new List<User>();
        inviteButton.interactable = false;
        ShowList();
    }

    private void OnDisable()
    {
        selectedUsers = new List<User>();
        ClearContianer(container);
    }

    void ShowList()
    {
        List<User> users = PlayerProfile.instance.globalUsers;

        emptyDataText.SetActive(users.Count == 0);

        foreach (User user in users)
        {
            GameObject obj = Instantiate(friendItem, container, false);
            obj.GetComponent<FriendItem>().SetData(transform.GetSiblingIndex(), user, OnSelect);
        }

        //for (int i = 0; i < users.Count; i++)
        //{
        //    GameObject obj = Instantiate(friendItem, container, false);
        //    obj.GetComponent<FriendItem>().SetData( transform.GetSiblingIndex(), OnSelect);
        //}
    }

    void OnSelect(User user, bool isSelected)
    {
        if (isSelected)
            selectedUsers.Add(user);
        else
            selectedUsers.Remove(user);

        inviteButton.interactable = (selectedUsers.Count > 0);
    }

    public void OnInviteButton()
    {
        if(PlayerProfile.Player_coins < Global.coinsRequired)
        {
            UIEvents.ShowPanel(Panel.Popup);
            UIEvents.UpdateData(Panel.Popup, (data) => {

                //if ((int)data[0] == 2)//on yes
                //{
                //    print("Go To Shop");
                //}
                //else
                //{
                //    print("Cancel");

                //}

            }, "SetData", $"You need at least {Global.coinsRequired} Coins!", "", "OK");

            return;
        }

        //UIEvents.ShowPanel(Panel.Popup);
        //UIEvents.UpdateData(Panel.Popup, null, "SetData", "Invite sent to your selected friends", "","OK");

        string roomName = "SAND_" + Random.Range(99, 9999);

       // SendGameRequest(roomName, PlayerProfile.Player_UserName, PlayerProfile.Player_UserID, "guest_7041971c4761418da8bd264305cf1d1a_userID");

        foreach (var item in selectedUsers)
        {
            SendGameRequest(roomName, PlayerProfile.Player_UserName, PlayerProfile.Player_UserID, item.UserId);
        }

        PhotonRoomCreator.instance.CreateRoomOnPhoton(true, roomName);
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
    }



    void ClearContianer(Transform container)
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
    }
}
