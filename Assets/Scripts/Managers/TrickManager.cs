using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class TrickManager
{
    public static List<Card> cards;
    static Card leadingCard;
   // static Card.Suit leadingSuit;

    static TrickManager()
    {
        cards = new List<Card>();
    }

    public static void AddCard(Card card)
    {
        if (cards.Count == 0)
        {
            leadingCard = card;
      //      leadingSuit = card.suit;
        }

        cards.Add(card);
    }

    public static void HighlightLowCards()
    {
        if (cards.Count == 0)
            return;

        Card bestCard = GetBestCard();

        foreach (Card card in cards)
        {
            card.HighlightCard(false);
        }

        bestCard.HighlightCard(true);

//        Debug.Log("best card: " + bestCard.name);
    }

    static Card GetBestCard()
    {
        Card.Suit leadingSuit = TrickManager.cards[0].suit;


        Card bestCard = null;
        Card highJoker = null;
        Card lowJoker = null;
        Card spadeCard = null;
        Card leadingSuitCard = null;

        foreach (Card card in cards)
        {
            if (bestCard == null)
            {
                bestCard = card;
            }
            else if (card.data.score > bestCard.data.score)
            {
                bestCard = card;
            }

            // Check for Joker cards
            if (card.suit == Card.Suit.Joker)
            {
                if (card.data.shortCode == "highJoker")
                {
                    if (highJoker == null || card.data.score > highJoker.data.score)
                    {
                        highJoker = card;
                    }
                }
                else if (card.data.shortCode == "lowJoker")
                {
                    if (lowJoker == null || card.data.score > lowJoker.data.score)
                    {
                        lowJoker = card;
                    }
                }
            }

            // Check for Spades cards
            if (card.suit == Card.Suit.Spades)
            {
                // Spades cards win if no Joker is present
                if (spadeCard == null || card.data.score > spadeCard.data.score)
                {
                    spadeCard = card;
                }
            }

            // Check for leading suit cards
            if (card.suit == leadingSuit && lowJoker == null && highJoker==null && spadeCard == null)
            {
                if (leadingSuitCard == null || card.data.score > leadingSuitCard.data.score)
                {
                    leadingSuitCard = card;
                }
            }
        }

        // Determine the best card considering Jokers and Spades
        if (highJoker != null)
        {
            bestCard = highJoker; // High Joker wins
        }
        else if (lowJoker != null)
        {
            bestCard = lowJoker; // Low Joker wins if High Joker is not present
        }
        else if (spadeCard != null)
        {
            bestCard = spadeCard; // Low Joker wins if High Joker is not present
        }
        else if (leadingSuitCard != null)
        {
            bestCard = leadingSuitCard; // Leading suit card wins if no Jokers or Spades
        }

        return bestCard;
    }

    public static Player GetTrickWinner()
    {
        Player roundWinner = GetBestCard().cardOwner;

        return roundWinner;
    }

    public static void GiveCardsToWinner(Player winner)
    {

        Transform transform = TableController.instance.GetPlayerWonCardTransform(winner.tablePosition);

        foreach(Card card in cards)
        {
            card.MoveCard(transform,1,true,true);
        }

     //   winner.bidWon++;

        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "UpdateBidCount", winner.tablePosition, ++winner.bidWon, winner.bidPlaced);

    }

    public static void ResetTrick()
    {
        cards.Clear();
        if (cards.Count>0)
        {
            cards = new List<Card>();
        }
    }
}
