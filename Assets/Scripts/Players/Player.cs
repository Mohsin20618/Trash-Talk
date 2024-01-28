using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player : IPlayer
{
    //public string Name { get; set; }
    //public List<Card> Hand { get; set; }
    //public int Score { get; set; }

    public string name;
    public List<Card> hand;
    public int score = 0;
    public int tablePosition;
    public int bidPlaced;
    public int bidWon;
    public bool isBot = false;
    public bool isOwn = false;
    public List<Card> winningCards;

    public int photonIndex = 0;
    public string id;
    public bool isMaster;
    public string imageURL;
    //  public Sprite sprite;

    public int roundBonus;
    public int teamBidPlaced;
    public int teamBidWon;
    public int roundBags;
    public int roundBounus;
    public int roundPoints;
    public int roundTotalBags;
    public int roundGabPenalty;
    public int roundTotalPoints;
    public bool bostonWon;

    public Player partner;
    public Player gameWinner;

    public Player(string playerName, string id, string image, bool isOwn, bool isMaster, bool isBot, int tablePos)
    {
        this.name = playerName;
        this.isBot = isBot;
        this.tablePosition = tablePos;
        this.isOwn = isOwn;


        this.id = id;
        this.isMaster = isMaster;
        this.imageURL = image;

        winningCards = new List<Card>();
        hand = new List<Card>();

        //Round Related Details
        this.bidPlaced = -1;
        this.bidWon = 0;
        this.roundBags = 0;
        this.roundBounus = 0;
        this.roundPoints = 0;
        this.roundTotalBags = 0;
        this.roundGabPenalty = 0;
        this.roundTotalPoints = 0;
        this.bostonWon = false;
        this.gameWinner = null;

    }

    public void SetPartner(Player partner)
    {
        this.partner = partner;
    }

    public void AddWinningCard(Card card)
    {
        winningCards.Add(card);
    }

    public void AddCardToHand(Card card)
    {
        hand.Add(card);
    }

    public void ResetCards()    //Reset cards in each rounds
    {
        winningCards.Clear();
        hand.Clear();
    }

    public Card PlayCard(int index)
    {
        if (index >= 0 && index < hand.Count)
        {
            Card card = hand[index];
            hand.RemoveAt(index);
            return card;
        }
        return null;
    }

    public void SetBid(int bid)
    {
        this.bidWon = 0;
        this.bidPlaced = bid;
    }



    //public Player(string name)
    //{
    //    Name = name;
    //    Hand = new List<Card>();
    //    Score = 0;
    //}
}
