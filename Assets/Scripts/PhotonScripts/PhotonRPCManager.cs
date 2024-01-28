using UnityEngine;
using Photon.Pun;
using System;

public class PhotonRPCManager : MonoBehaviourPunCallbacks
{
    private static PhotonRPCManager instance;


    // Singleton pattern to ensure only one instance of PhotonRPCManager exists
    public static PhotonRPCManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PhotonRPCManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("PhotonRPCManager");
                    instance = obj.AddComponent<PhotonRPCManager>();
                }
            }
            return instance;
        }
    }

    // Method to send an RPC to all relevant clients
    public void SendRPC(string methodName, RpcTarget target, params object[] parameters)
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC(methodName, target, parameters);
        }
    }


    [PunRPC]
    public void SpawnPlayers_Rpc(string shuffledCards ,string sortedIds)
    {
        //Call spawning herh
        print("Got Shuffled cards from Master: " + shuffledCards);
        GameplayManager.instance.AnimateCardsScreen(shuffledCards, sortedIds);
    }


    [PunRPC]
    public void RPC_DisplayChatMessage(string senderId, string chatType, int index)
    {
        ChatHandler.instance.DisplayMessage(senderId, chatType, index);
    }

    internal void SpawnPlayers(string shuffledCards,string sortedIds)
    {
        SendRPC("SpawnPlayers_Rpc", RpcTarget.All, shuffledCards, sortedIds);
    }

    [PunRPC]
    public void Place_Bid(string playerId, int photonIndex, int selectedBid)
    {
        //Call spawning herh
        GameplayManager.instance.GetBidFromPlayers(playerId, photonIndex, selectedBid);
    }

    internal void PlaceBid(string playerId, int photonIndex, int selectedBid)
    {

        SendRPC("Place_Bid", RpcTarget.All, playerId, photonIndex, selectedBid);
    }

    [PunRPC]
    public void Set_Player_Turn(string playerId, int photonIndex)
    {
        //Call spawning herh
        GameplayManager.instance.GetPlayerTurn(playerId, photonIndex);
    }

    internal void SetPlayerTurn(string playerId, int photonIndex)
    {

        SendRPC("Set_Player_Turn", RpcTarget.All, playerId, photonIndex);
    }


    [PunRPC]
    public void ResetTrick_RPC()
    {
        TrickManager.ResetTrick();
    }

    internal void ResetTrick()
    {
        SendRPC("ResetTrick_RPC", RpcTarget.All);
    }

    [PunRPC]
    public void Placed_Card_By_Player(string playerId, string cardCode)
    {
        //Call spawning herh
        GameplayManager.instance.OnPlacedCardByMultiplayer(playerId, cardCode);
    }

    internal void PlacedCardByPlayer(string playerId, string cardCode)
    {

        SendRPC("Placed_Card_By_Player", RpcTarget.All, playerId, cardCode);
    }

    [PunRPC]
    public void Anounce_Round_Winner(string playerId, string cardCode)
    {
        //Call spawning herh
       // GameplayManager.instance.OnPlacedCardByMultiplayer(playerId, cardCode);
    }

    internal void AnounceRoundWinner(string playerId)
    {

        SendRPC("Anounce_Round_Winner", RpcTarget.All, playerId);
    }
}
