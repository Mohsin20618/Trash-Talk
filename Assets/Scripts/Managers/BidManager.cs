using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BidManager : MonoBehaviour
{
    int currentPlayerIndex;
    int totalBidsPlaced;
    int totalPlayers;
    Player currentPlayer;


    public void StartBid(int totalPlayers, int currentPlayerIndex)
    {
        totalBidsPlaced = 0;
        this.totalPlayers = totalPlayers;


        if (Global.isMultiplayer)
        {
            ShowMultiplayerBids();
            return;
        }

        this.currentPlayerIndex = currentPlayerIndex;
        SelectBid();
    }

    void ShowMultiplayerBids()
    {
        UIEvents.UpdateData(Panel.PlayersUIPanel, PlacePlayerBid,  "ShowBidUI", 0,-1);
    }

    //Calling when photon player select bid
    public void OnGetPlayerBid(string playerId, int photonIndex, int selectedBid)//will use for multiplayer
    {
        //print("OnSelectPlayerBit playerId: " + playerId);
        //print("OnSelectPlayerBit photonIndex: " + photonIndex);
        //print("OnSelectPlayerBit selectedBid: " + selectedBid);


        Player p = PlayerManager.instance.GetPlayerByPhotonIndex(photonIndex);

     //   print("OnSelectPlayerBit tablePosition: " + p.tablePosition);

        p.SetBid(selectedBid);

        if (p.id == PlayerProfile.Player_UserID)
        {
            //Show UI to other player
        //    UpdateBidCount(p);
        }
        else
        {
            UIEvents.UpdateData(Panel.PlayersUIPanel, PlacePlayerBid, "ShowBidUI", p.tablePosition, selectedBid);
        }

        UpdateBidCount(p);

        if (Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            Invoke("SetBotBidByMaster",1);
        }

        totalBidsPlaced++;

        if (totalBidsPlaced == totalPlayers)
        {
            Invoke("OnCompleteMultiplayerBids", 1);
        }
    }

    void OnCompleteMultiplayerBids()
    {
        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "HideAllBidsUIs");
        GameplayManager.instance.StartGame();
    }

    void SetBotBidByMaster()
    {
        foreach(Player p in PlayerManager.instance.player)
        {
            if(p.isBot && p.bidPlaced == -1)
            {
                int bid = DecideBidByBot(p);
                BroadcastBid(p.id,p.photonIndex,bid);
                break;
            }
        }
    }

    void BroadcastBid(string playerId, int photonIndex, int selectedBid)
    {
        PhotonRPCManager.Instance.PlaceBid(playerId, photonIndex, selectedBid);
    }

    void SelectBid()
    {

        //string panel = "Panel1";
      //  System.Action<object[]> callback = PlacePlayerBid;

        //UpdateData(panel, callback, param1, param2, param3);

        currentPlayer = PlayerManager.instance.players[currentPlayerIndex];

        if (currentPlayerIndex == 0)//it is temproray later it will be decided by ID or other property
        {
            UIEvents.UpdateData(Panel.PlayersUIPanel, PlacePlayerBid,  "ShowBidUI", currentPlayerIndex,-1);
        }
        else
        {
            StartCoroutine(BotPlaceBid());
        }
    }

    public void PlacePlayerBid(object[] parameters)
    {
        if (Global.isMultiplayer)
        {
            Player p = PlayerManager.instance.GetMyPlayer();
            print("Paced Bid 1: " + (int)parameters[0]);
            BroadcastBid(p.id,p.photonIndex,(int)parameters[0]);
            return;
        }

        this.currentPlayer.SetBid((int)parameters[0]);
        UpdateBidCount(this.currentPlayer);
        DecideNext();
    }


    IEnumerator BotPlaceBid()
    {
        Player botPlayer = this.currentPlayer;
        //List<Card> hand = botPlayer.hand;
       

        //int spadeCount = 0;
        //int handStrength = 0;

        //foreach (Card card in hand)
        //{
        //    if (card.data.suit == Card.Suit.Spades.ToString())
        //    {
        //        spadeCount++;
        //        handStrength += card.data.rank;
        //    }
        //}

        int bid = DecideBidByBot(botPlayer);

        //if (handStrength >= 13 && spadeCount >= 1)
        //{
        //    bid = spadeCount + 1;
        //}
        //else if (handStrength >= 10 && spadeCount >= 2)
        //{
        //    bid = spadeCount;
        //}
        //else if (handStrength >= 7 && spadeCount >= 3)
        //{
        //    bid = spadeCount - 1;
        //}

        botPlayer.SetBid(bid);
        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "ShowBidUI", currentPlayerIndex,bid);
        UpdateBidCount(botPlayer);
        yield return new WaitForSeconds (1);
        DecideNext();
    }

    int DecideBidByBot(Player botPlayer)
    {
        List<Card> hand = botPlayer.hand;


        int spadeCount = 0;
        int handStrength = 0;

        foreach (Card card in hand)
        {
            if (card.data.suit == Card.Suit.Spades.ToString())
            {
                spadeCount++;
                handStrength += card.data.rank;
            }
        }

        int bid = 0;

        if (handStrength >= 13 && spadeCount >= 1)
        {
            bid = spadeCount + 1;
        }
        else if (handStrength >= 10 && spadeCount >= 2)
        {
            bid = spadeCount;
        }
        else if (handStrength >= 7 && spadeCount >= 3)
        {
            bid = spadeCount - 1;
        }

        return bid;
    }





    public void UpdateBidCount(Player player)
    {
        int playerPos = player.tablePosition;
        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "UpdateBidCount", playerPos, player.bidWon,player.bidPlaced);
    }

    void DecideNext()
    {
        totalBidsPlaced++;

        if(totalBidsPlaced == totalPlayers)
        {
            UIEvents.UpdateData(Panel.PlayersUIPanel, null, "HideAllBidsUIs");
            GameplayManager.instance.StartGame();
            return;
        }

        currentPlayerIndex = (currentPlayerIndex + 1) % 4;

        SelectBid();

    }

    public void ClearPlayersBid()
    {
        List<Player> pl = PlayerManager.instance.player;

        foreach(Player p in pl)
        {
            p.SetBid(-1);
        }
    }
}
