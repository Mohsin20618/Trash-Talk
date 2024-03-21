using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using TrashTalk;
using System.Linq;

public class PhotonConnectionController : MonoBehaviourPunCallbacks
{
    public static bool isChatConnected;
    //[Space]
    //[Header("Invitation Panel")]
    //public InvitationPanel invitationPanel;
    public static PhotonConnectionController Instance;

    public TabPanels tabPanels;

    private void Awake()
    {
        Instance = this;
    }


    internal void JoinPhotonLobbyAgain()
    {
        if (PhotonNetwork.InRoom)
        {
            WaitingLoader.instance.ShowHide(true);
            PhotonNetwork.LeaveRoom();
            TypedLobby Default = new TypedLobby("US", LobbyType.Default);
            PhotonNetwork.JoinLobby(Default);
        }
        if (!PhotonNetwork.InLobby)
        {
            WaitingLoader.instance.ShowHide(true);
            TypedLobby Default = new TypedLobby("US", LobbyType.Default);
            PhotonNetwork.JoinLobby(Default);
        }

    }

    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            //TypedLobby Default = new TypedLobby("US", LobbyType.Default);
            ////Debug.LogError("OnConnectedToMaster Region: " + PhotonNetwork.CloudRegion);
            //PhotonNetwork.JoinLobby(Default);

        }
        if (!PhotonNetwork.InLobby)
        {
            //TypedLobby Default = new TypedLobby("US", LobbyType.Default);
            //Debug.Log("OnConnectedToMaster Region: " + PhotonNetwork.CloudRegion);
            //PhotonNetwork.JoinLobby(Default);
            //invitationPanel.ChatSetting();
        }

        PhotonRoomCreator.instance.setPhotonProps();
        if (isChatConnected == false)
        {
        }
    
        PhotonChat.Instance.Connect();

        if (WaitingLoader.instance.gameObject.activeInHierarchy)
        {
            WaitingLoader.instance.ShowHide(false);
        }


    }


    public void ConnectingToPhoton()
    {
        tabPanels.UpdateUI("coins");
        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        keyValuePairs.Add("PageNo", 1);

        //  PageNo
        WebServiceManager.instance.APIRequest(WebServiceManager.instance.globalDatabaseUsers, Method.POST, null, keyValuePairs, OnGetGlobalUsers, OnFailGlobalUser, CACHEABLE.NULL, true, null);
        WebServiceManager.instance.APIRequest(WebServiceManager.instance.getFriends, Method.POST, null, keyValuePairs, OnGetFriendsUsers, OnFailGlobalUser, CACHEABLE.NULL, true, null);
        WebServiceManager.instance.APIRequest(WebServiceManager.instance.getLatestPlayedMembers, Method.POST, null, keyValuePairs, OnGetRecentUsers, OnFailGlobalUser, CACHEABLE.NULL, true, null);

        Debug.Log("ConnectingToPhoton. . .");
        string gameVersion = "0.0.1";
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AutomaticallySyncScene = true;   // was set to true
        PhotonNetwork.LocalPlayer.NickName = PlayerProfile.Player_UserName;
        PhotonNetwork.NickName = PlayerProfile.Player_UserName;
        PhotonNetwork.AuthValues.UserId = PlayerProfile.Player_UserID; // alternatively set by server
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
    }


    public void GetFriends()
    {
        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        keyValuePairs.Add("PageNo", 1);
        WebServiceManager.instance.APIRequest(WebServiceManager.instance.getFriends, Method.POST, null, keyValuePairs, OnGetFriendsUsers, OnFailGlobalUser, CACHEABLE.NULL, true, null);

        Dictionary<string, object> keyValuePairs2 = new Dictionary<string, object>();
        keyValuePairs2.Add("userID", PlayerProfile.Player_UserID);
        WebServiceManager.instance.APIRequest(WebServiceManager.instance.getProfileFunction, Method.POST, null, keyValuePairs2, OnProfileSuccess, OnFailGlobalUser, CACHEABLE.NULL, false, null);
    }

    private void OnProfileSuccess(JObject resp, long arg2)
    {
        Debug.Log("OnProfileSuccess: " + resp.ToString());

        var playerData = PlayerData.FromJson(resp.ToString());
        PlayerProfile.UpdatePlayerData(playerData.User);
        tabPanels.UpdateUI("coins");

    }
    public void OnGetGlobalUsers(JObject resp, long arg2)
    {
        var globalUsers = GlobalUsers.FromJson(resp.ToString());
        if (globalUsers != null && globalUsers.data != null)
        {
            PlayerProfile.instance.globalUsers = globalUsers.data.data;
        }

        Debug.Log("Total globalUsers: " + PlayerProfile.instance.globalUsers.Count);
    }
    public void OnGetRecentUsers(JObject resp, long arg2)
    {
        var globalUsers = DeSerialize.FromJson<GlobalUsers>(resp.ToString());
        if (globalUsers != null && globalUsers.data != null)
        {
            PlayerProfile.instance.recentUsers = globalUsers.data.data;
        }

        Debug.Log("Total recentUsers: " + PlayerProfile.instance.recentUsers.Count);
    }
    public void OnGetFriendsUsers(JObject resp, long arg2)
    {
        var globalUsers = GlobalUsers.FromJson(resp.ToString());

        if (globalUsers != null && globalUsers.friends != null)
        {
            PlayerProfile.instance.friendsUsers = globalUsers.friends.data;
        }

        Debug.Log("Total friendsUsers: " + PlayerProfile.instance.friendsUsers.Count);
    }

    void OnFailGlobalUser(string msg)
    {
        Debug.Log("Fail Users:  " + msg);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.LogError("Photon Disconnect: " + cause);
        PhotonNetwork.Reconnect();
    }

}
