using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RoomListCaching : MonoBehaviourPunCallbacks
{
    public static Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        cachedRoomList.Clear();
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name)) //Remove Row
                {
                    cachedRoomList.Remove(info.Name);
                }
            }
            else
            {
                if (cachedRoomList.ContainsKey(info.Name)) //Clear Old data 
                {
                    cachedRoomList.Remove(info.Name);
                }
                    cachedRoomList.Add(info.Name , info); //Update New Data
            }
        }
    }

    public override void OnJoinedLobby()
    {
        cachedRoomList.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate...");
        UpdateCachedRoomList(roomList);
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        cachedRoomList.Clear();
    }
}
