using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
//using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class PhotonCallBacksDontDestroy : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //private Hashtable _myCustomProperties = new Hashtable();
    public static GameObject single;


    // Start is called before the first frame update
    void Start()
    {
        if (single && single != this.gameObject)
        {
            /// If a singleton already exists, destroy the old one - TODO: Not sure about this behaviour yet. Allows for settings changes with scene changes.
            Destroy(single);
        }

        single = this.gameObject;
        DontDestroyOnLoad(this.gameObject);
    }


    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
    }


    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    { 

    }


    public void CheckPlayersCount()
    {

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 4)
        {
            //**********Room is full**********\\
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = true;
        }
    }


    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
