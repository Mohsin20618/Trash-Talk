using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class PlayersUIPanel : UIPanel
{
    public GameObject yourTurnHeading;
    public Sprite[] botImages;
    public Sprite[] bonusImages;
    public PlayerUI[] playerUI;
    private Action<object[]> callBack;
    object[] data;
    

    private void Start()
    {
        //playerUI = GetComponentsInChildren<PlayerUI>();
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void UpdateData(Action<object[]> callBack, params object[] parameters)
    {
        string type = (string)parameters[0];
        int playerNumber = 0;
        switch (type)
        {
            case "ShowBidUI":
                playerNumber = (int)parameters[1];
                int bidCount = (int)parameters[2];
                this.callBack = callBack;
                ShowBidUI(playerNumber, bidCount);
                break;
            case "HideAllBidsUIs":
                HideAllBidsUIs();
                break;
            case "UpdateCardCount":
                playerNumber = (int)parameters[1];
                int count = (int)parameters[2];
                UpdateCardCount(playerNumber, count);
                break;
            case "UpdateBidCount":
                playerNumber = (int)parameters[1];
                UpdateBidCount(playerNumber, (int)parameters[2], (int)parameters[3]);
                break;
            case "WinnerAnimation":
                playerNumber = (int)parameters[1];
                ShowWinnerAnimation(playerNumber);
                break;

            case "SetTurnIndication":
                playerNumber = (int)parameters[1];
                bool isTurn = (bool)parameters[2];
                Debug.Log("SetTurnIndication: " + isTurn);
                SetTurnIndication(playerNumber,isTurn);
                break;
            case "ShowBunus":
                playerNumber = (int)parameters[1];
                string bonusType = (string)parameters[2];
                ShowBonus(bonusType, playerNumber);
                break;

            case "ShowHideYourTurnHeading":
                bool show = (bool)parameters[1];
                ShowHideYourTurnHeading(show);
                break;
            case "SetPlayersData":
                SetPlayersData();
                break;
            case "StopTimer":
                playerNumber = (int)parameters[1];
                StopTimer(playerNumber);
                break;
            case "ResetUI":
                ResetUI();
                break;

        }
    }

    void StopTimer(int playerNumber)
    {
        playerUI[playerNumber].StopTimer();
    }

    public void SetPlayersData()
    {
        //if (Global.isMultiplayer)
        //{
        //    List<Player> players = PlayerManager.instance.players;
        //    for (int i = 0; i < players.Count; i++)
        //    {
        //        playerUI[i].SetUI(players[i].name, 0);
        //    }
        //}
        //else
        //{
        print("setting player here now");
        print("playerUI == null " + (playerUI==null));

        if (playerUI == null)
            return;

       // List<Player> players = PlayerManager.instance.players;

        print("also coming here: " + PlayerManager.instance.players.Count);
            for (int i=0;i< PlayerManager.instance.players.Count; i++)
            {
                Player player = PlayerManager.instance.players[i];
                print("playerUi: " + playerUI);
                print("players[i].name: " + player.name);
                playerUI[i].SetUI(player.name,player.id, player.isBot? botImages[i]:null, player.roundTotalPoints, player.imageURL);
            }
    //    }
    }

    public void ResetUI()
    {
        //temp condition
        if (playerUI == null)
            return;

        foreach (PlayerUI ui in playerUI)
        {
            ui.ResetUI();
        }

        yourTurnHeading.SetActive(false);
    }

    void ShowBidUI(int playerNumber, int bidCout = -1)
    {
        playerUI[playerNumber].ShowBidUI(bidCout, SelectBid);
    }

    void UpdateCardCount(int playerNumber, int count)
    {
        //        print("here come for updateding card playerNumber: " + playerNumber);
//        print("here player number is: " + playerNumber);
        playerUI[playerNumber].UpdateCardCount(count);
    }

    void UpdateBidCount(int playerNumber, int bidWon, int totalBid)
    {
        playerUI[playerNumber].UpdateBids(totalBid, bidWon);
    }

    public void SetTurnIndication(int playerNumber, bool isTurn)
    {
        playerUI[playerNumber].SetTurnIndication(isTurn);
    }

    void ShowWinnerAnimation(int playerNumber)
    {
        playerUI[playerNumber].WinAnimation();
    }

    void ShowHideYourTurnHeading(bool show)
    {
        yourTurnHeading.SetActive(show);
    }

    void SelectBid(int bid)
    {
        this.data = new object[] { bid };
        this.callBack(this.data);
    }

    void ShowBonus(string type,int playerNumber)
    {
        Sprite sprite = null;
        switch (type)
        {
            case "10For200":
                sprite = bonusImages[0];
                break;
            case "Boston":
                sprite = bonusImages[1];
                break;
            case "Nil":
                sprite = bonusImages[2];
                break;
            case "DoubleNil":
                sprite = bonusImages[3];
                break;
        }

        playerUI[playerNumber].ShowBonusImage(sprite);


        //this.data = new object[] { bid };
        //this.callBack(this.data);
    }

    void HideAllBidsUIs()
    {
        foreach (PlayerUI player in playerUI)
        {
            player.HideBidUI();
        }
    }


}
