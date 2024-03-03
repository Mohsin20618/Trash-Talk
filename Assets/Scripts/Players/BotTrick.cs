using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotTrick
{


    public Card GetBestCard(List<Card> cardsInHand)
    {
        Card.Suit leadingSuit = Card.Suit.Spades;
        bool isFirstTurn = true;
        Card bestCard = null;

        if (TrickManager.cards.Count > 0)
        {
            leadingSuit = TrickManager.cards[0].suit;
            isFirstTurn = false;
        }

        bool hasNormalCards = HasNormalCards(cardsInHand);
        bool hasLeadingSuit = HasLeadingSuit(cardsInHand, leadingSuit);

        foreach (Card card in cardsInHand)
        {

            if (isFirstTurn)
            {
                if (hasNormalCards)
                {
                    if (card.suit != Card.Suit.Spades && (bestCard == null || card.data.score > bestCard.data.score))
                  //  if ((bestCard == null || card.data.score > bestCard.data.score))
                        bestCard = card;
                }
                else if (bestCard == null || card.data.score > bestCard.data.score)
                {
                    bestCard = card;
                }
            }
            else
            {
                if (hasLeadingSuit)
                {
                    //if(card.suit == leadingSuit && (bestCard == null || card.data.score > bestCard.data.score))
                    if (card.suit == leadingSuit && (bestCard == null || card.data.score > bestCard.data.score))
                        bestCard = card;
                }
                else
                {
                    if (bestCard == null || card.data.score > bestCard.data.score)
                    {
                        bestCard = card;
                    }
                }
            }
        }

        if (!isFirstTurn)
        {
            //Hunain Logic 1
            if (hasLeadingSuit)
            {
                bool canBotPlayBestCard = true;

                foreach (var item in TrickManager.cards)
                {
                    if (bestCard.data.score < item.data.score)
                    {
                        canBotPlayBestCard = false;
                        break;
                    }
                }

                if (!canBotPlayBestCard)
                    bestCard = FindLowestCard(leadingSuit, cardsInHand);
                else
                    Debug.Log("Bot has leading card of the current suit.");
            }
            else
            {
                bestCard = FindLowestCard(cardsInHand);
            }


        }

        return bestCard;
    }

    private Card FindLowestCard(Card.Suit leadingSuit, List<Card> cardsInHand)
    {
        Debug.Log("FindLowestCard of " + leadingSuit.ToString());

        return cardsInHand.FindAll(card => card.suit.Equals(leadingSuit)).OrderBy(obj => obj.data.score).First();
    }

    private Card FindLowestCard(List<Card> cardsInHand)
    {
        Debug.Log("Just FindLowestCard of any suit");
        return cardsInHand.OrderBy(obj => obj.data.score).First();
    }

    bool HasNormalCards(List<Card> cardsInHand)
    {
        foreach (Card card in cardsInHand)
        {
            if (card.suit != Card.Suit.Spades)
            {
                return true;
            }
        }
        return false;
    }

    bool HasLeadingSuit(List<Card> cardsInHand,Card.Suit leadingSuit)
    {
        foreach (Card card in cardsInHand)
        {
            if (card.suit == leadingSuit)
            {
                return true;
            }
        }
        return false;
    }

}
