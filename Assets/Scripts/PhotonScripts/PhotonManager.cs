using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private static PhotonManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    //private void Start()
    //{
    //    ConnectToPhoton();
    //}

    //public void ConnectToPhoton()
    //{
    //    PhotonNetwork.ConnectUsingSettings();
    //}

    //public override void OnConnectedToMaster()
    //{
    //    Debug.Log("Connected to Photon");
    //    //PhotonNetwork.JoinRandomRoom();
    //}

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room. Creating a new room.");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        //Debug.Log("Joined a room");
        //Global.isMultiplayer = true;
        //UIEvents.HidePanel(Panel.TabPanels);
        //UIEvents.HidePanel(Panel.FriendsPanel);
        //UIEvents.ShowPanel(Panel.GameplayPanel);
    }
}
