using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HandCardsUI : MonoBehaviour
{
    public GameObject cardPrefab;

    public Transform handCardsTrans;

    public float radius = 50f;

    public RectTransform cardRect;

    private float CardWidth;
    public Transform centerPoint;

    //private List<Card> cardsInHands;

    private void Start()
    {
        //int total = transform.childCount;
        //float distance = 100 / 2; //size / 2
        //float x = 0;
        //float y = 0;

        //for (int i = 0; i < total; i++)
        //{
        //    transform.GetChild(i).transform.localPosition = new Vector2(x, y);
        //    x += distance;
        //}
        CardWidth = cardRect.rect.width;
    
    }

    // Call this method to update the card arrangement whenever a card is removed
    public void UpdateCardArrangement()
    {
        int childCount = transform.childCount;
        int mid = handCardsTrans.childCount / 2;
        int startNum = mid - (childCount / 2);
        int currentTrans = startNum;

        for (int i = 0; i < childCount; i++)
        {
            Transform card = transform.GetChild(i);
            Transform cardPos = handCardsTrans.GetChild(currentTrans);

            card.transform.localPosition = cardPos.transform.localPosition;
            card.transform.localEulerAngles = cardPos.transform.localEulerAngles;
            card.SetSiblingIndex(i);
            currentTrans++;
        }


        //Old Logic

        /*  int total = transform.childCount;
          float maxAngle = 30;
          float fullAngle = -maxAngle;
          float anglePerCard = fullAngle / total;
          float firstAngle = CalcFirstAngle(fullAngle);
          float handWidth = CalcHandWidth(total);

       //   float percTwistedAngleAddYPos = 0.5f;
          float initialXPos =  -handWidth * 0.5f;

          for (int i = 0; i < total; i++)
          {
              Transform card = transform.GetChild(i).transform;
            //  cardsInHands.Add(card);
              float angleTwist = firstAngle + i * anglePerCard;

              float yDistance = Mathf.Abs(angleTwist);
              float xPos = initialXPos + i * (CardWidth + 20);

              float yPos = (-25) - yDistance;

              card.localRotation = Quaternion.Euler(0, 0, angleTwist);

              card.localPosition = new Vector3(xPos, yPos, 0);

              //if(i == total-1)
              //{
              //    Invoke("ShowCardsFront", 1);
              //}
          }*/
    }


    List<Card> cardsInHand;
    public IEnumerator ShowPlayerCards()
    {
        cardsInHand = new List<Card>();
        foreach (Transform child in transform)
        {
            cardsInHand.Add(child.GetComponent<Card>());
        }

        foreach (Card card in cardsInHand)
        {
            SoundManager.Instance.PlaySoundEffect(Sound.Click);
            card.SwichSideAnimated();
            yield return new WaitForSeconds(0.1f);
        }
    }

   public void SortHandCards()
    {
        // Get the total number of children in the parent GameObject
        int childCount = transform.childCount;

        // Iterate over the children and arrange them based on their enum value
        for (int i = 0; i < childCount - 1; i++)
        {
            // Get the first child transform
            Transform child1 = transform.GetChild(i);
            // Get the script attached to the first child
            Card card1 = child1.GetComponent<Card>();

            for (int j = i+1; j < childCount; j++)
            {
                // Get the second child transform
                Transform child2 = transform.GetChild(j);
                // Get the script attached to the second child
                Card card2 = child2.GetComponent<Card>();

                // Compare the enum values of the scripts
                if (card1.data.score < card2.data.score)
                {
                    SwapPositionAndRotation(child1,child2);
                    Transform temp = child1;
                    child1 = child2;
                    child2 = temp;

                    card1 = child1.GetComponent<Card>();
                    card2 = child2.GetComponent<Card>();
                }

                if (card1.suit > card2.suit)
                {
                    SwapPositionAndRotation(child1, child2);
                    Transform temp = child1;
                    child1 = child2;
                    child2 = temp;

                    card1 = child1.GetComponent<Card>();
                    card2 = child2.GetComponent<Card>();
                }
            }
        }
    }

    void SwapPositionAndRotation(Transform child1, Transform child2)
    {
        int child1Index = child1.GetSiblingIndex();
        int child2Index = child2.GetSiblingIndex();

        child1.SetSiblingIndex(child2Index);
        child2.SetSiblingIndex(child1Index);

        Vector3 tempPosition = child1.localPosition;
        Vector3 tempAngle = child1.localEulerAngles;


        child1.localPosition = child2.localPosition;
        child1.localEulerAngles = child2.localEulerAngles;

        child2.localPosition = tempPosition;
        child2.localEulerAngles = tempAngle;
    }

    public void ActiveMainPlayerCards(bool allActive=true)
    {
        Card.Suit leadingSuit = Card.Suit.Spades;
        bool isFirstTurn = true;
        
        if (TrickManager.cards.Count > 0)
        {
            leadingSuit = TrickManager.cards[0].suit;
            isFirstTurn = false;
        }

        Debug.Log("######allActive:" + allActive + " isFirstTurn:" + isFirstTurn + " totalPlayerPlayed:" + GameplayManager.instance.totalPlayerPlayed + " cards.Count:" + TrickManager.cards.Count);
        bool hasNormalCards = HasNormalCards();
        bool hasLeadingSuit = HasLeadingSuit(leadingSuit);

        //old Logic
        //foreach (Card card in cardsInHand)
        //{
        //    bool active = true;
        //    if (allActive)
        //    {
        //        if (isFirstTurn)
        //        {
        //             if (hasNormalCards)
        //             {
        //                active = (card.suit != Card.Suit.Spades || GameplayManager.instance.isSpadeActive);
        //         //   active = true;
        //             }
        //        }
        //        else
        //        {
        //            if (hasLeadingSuit)
        //            {
        //                active = (card.suit == leadingSuit);
        //            }
        //            else
        //            {
        //                active = true;
        //            }

        //            // New condition, active spades if spades activated
        //            if (GameplayManager.instance.isSpadeActive && card.suit == Card.Suit.Spades)
        //            {
        //                active = true; 
        //            }
        //        }
        //    }
        //    else
        //    {
        //        active = allActive;
        //    }

        //    card.ActiveButton(active);
        //    card.ActiveDragable(active);
        //    card.HighlightCard(active);
        //}

        foreach (Card card in cardsInHand)
        {
            bool active = true;

            if (allActive)
            {
                if (isFirstTurn)
                {
                    Debug.Log("Active All Cards.");
                    active = true; // On the first turn, all cards are active
                }
                else
                {
                    if (hasLeadingSuit)
                    {
                        active = (card.suit == leadingSuit);
                    }
                    else
                    {
                        active = true;
                    }

                    if (Global.isSpadeActive && card.suit == Card.Suit.Spades)
                    {
                       // active = true;
                    }

                    // Only leading suit cards and active Spades cards are active
                    //if (card.suit != leadingSuit && card.suit != Card.Suit.Spades)
                    //{
                    //    active = false;
                    //}
                }
            }
            else
            {
                //Disale All Cards Logic : Understandings by Hunain
                active = allActive;
            }

            card.ActiveButton(active);
            card.ActiveDragable(active);
            card.HighlightCard(active);
        }
    }

    bool HasNormalCards()
    {
        foreach (Card card in cardsInHand)
        {
            if(card.suit != Card.Suit.Spades)
            {
                return true;
            }
        }
        return false;
    }

    bool HasLeadingSuit(Card.Suit leadingSuit)
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

    public void OnUseHandCard(Card card)
    {
        cardsInHand.Remove(card);
    }

    private float CalcHandWidth(int quantityOfCards)
    {
        var widthCards = quantityOfCards * (CardWidth - 30);
        var widthSpacing = (quantityOfCards) * (CardWidth / 2);
        return widthCards + widthSpacing;
    }

    private float CalcFirstAngle(float fullAngle)
    {
        var magicMathFactor = 0.1f;
        return -(fullAngle / 2) + fullAngle * magicMathFactor;
    }

    public void ClearHandCards()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
