using System.Collections;
using System.Collections.Generic;
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

        //Debug.Log("isFirstTurn: " + isFirstTurn);
        //Debug.Log("Has leading suit: " + hasLeadingSuit);
        //Debug.Log("Has normal cards: " + hasNormalCards);


        //foreach (Card card in cardsInHand)
        //{
        //    Debug.Log("bot has: " + card.name);
        //}

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
            return bestCard;
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
