using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardDeck : MonoBehaviour
{
    public CardDeckDB cardsDB;
    public GameObject cardPrefab;
    public Transform deckTransform;

    private List<Card> cards;

    int totalCardsInDeck;

    void Start()
    {
    //   CreateInitialDeck();
    }

    List<CardData> cardsList;
    public void CreateInitialDeck(List<CardData> shuffledList = null)
    {
        Vector2 position = deckTransform.position;
        cardsList = new List<CardData>(cardsDB.list);

        cardsList = shuffledList==null?ShuffleCards(cardsList):shuffledList;

        cards = new List<Card>();


        totalCardsInDeck = cardsList.Count;
        float size = 2f;
        for (int i=0;i< totalCardsInDeck; i++)
        {
            CardData cardData = cardsList[i];
            GameObject cardObj = Instantiate(cardPrefab, this.transform);
            Card card = cardObj.GetComponent<Card>();
            card.SetData(cardData);
            cardObj.transform.SetSiblingIndex(0);
            cardObj.name = cardData.name;
            cardObj.transform.eulerAngles = new Vector3(0, 0, 90);
            cardObj.transform.localScale *= size;
            cardObj.transform.position = position;
            position.y -= 1f;

            cards.Add(card);
        }
    }

    public string GetShuffleCardsString()
    {
        cardsList = new List<CardData>(cardsDB.list);

        //for (int i = 0; i < cardsList.Count; i++)
        //{
        //    print("Cards without shuffle: " + cardsList[i].shortCode);
        //}

        cardsList = ShuffleCards(cardsList);

        //for (int i = 0; i < cardsList.Count; i++)
        //{
        //    print("Cards after shuffle: " + cardsList[i].shortCode);
        //}

        string s = string.Join(",", cardsList.ConvertAll(x => x.shortCode.ToString()).ToArray());


        //print("shuffled string:     " + s);


        return s;
    }

    public List<CardData> CardsStringToList(string s)
    {
        cardsList = new List<CardData>(cardsDB.list);
        List<string> shuffledShortCodes = s.Split(',').ToList();

        List<CardData> shuffledList = cardsList
            .OrderBy(item => shuffledShortCodes.IndexOf(item.shortCode))
            .ToList();


        //for(int i=0;i< shuffledList.Count; i++)
        //{
        //    print("Now again from String to List: " + shuffledList[i].name);
        //}

        return shuffledList;
    }

    //public void ShowHideCardContainer(bool isActive)
    //{
    //    this.gameObject.SetActive(isActive);
    //}

    public List<T> ShuffleCards<T>(List<T> list)
    {
        int count = list.Count;
        for (int i = 0; i < count - 1; i++)
        {
            int j = Random.Range(i, count);
            T temp = list[j];
            list[j] = list[i];
            list[i] = temp;
        }
        
        return list;
    }

    public void DistributeCards()
    {
        StartCoroutine(DistributeCardsWithDelay());
    }


    public IEnumerator DistributeCardsWithDelay()
    {
        //   int playerCount = playerPositions.Length;
        int cardsPerPlayer = 13;
        int cardNumber = 0;

        int currentPlayerIndex = Global.isMultiplayer? PlayerManager.instance.GetMasterIndex(): 0;

   //     print("card starting index is: " + currentPlayerIndex);

        int playerCount = PlayerManager.instance.players.Count;
        // Iterate through the deck and distribute the cards to players
        for (int i = 0; i < cardsPerPlayer; i++)
        {
            //for (int j = 0; j < 4; j++)
            for (int j = 0; j < playerCount; j++)
            {
                // Get the next card from the deck

                // Set the target position for the card to move to (player's position)
                //   card.targetPosition = playerPositions[j].position;
                SoundManager.Instance.PlaySoundEffect(Sound.Hint);
                Card card = GetNextCard(cardNumber);
                cardNumber++;

                //Transform playerPosition = playerPositions[currentPlayerIndex];
                //Vector3 targetPosition = playerPosition.position;

                //card.targetPosition = targetPosition;

                Player player =  PlayerManager.instance.GetPlayer(currentPlayerIndex);
                //    bool isOwn = player.isOwn;
//                print("Player: " + currentPlayerIndex + " Table position is:  " + player.tablePosition);
                player.AddCardToHand(card);
                card.SetOwner(player);
             //   if (isOwn)
             //       card.SwitchSide();

                // Move the card to the target position using Lean Tween
                card.MoveToPlayerPosition(player.tablePosition);
                currentPlayerIndex = (currentPlayerIndex + 1) % playerCount;

                yield return new WaitForSeconds(0.1f);

            }
        }

        GameplayManager.instance.Invoke("StartBid", 1);
    }

    //public int GetMasterIndex()
    //{
    //    return PlayerManager.instance.players.FindIndex(x=>x.isMaster==true);
    //}


    private Card GetNextCard(int index)
    {
        return cards[index];
    }

    public void ClearDeck()
    {
        if(cards != null)
            cards.Clear();

        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public Card GetCard(string shortCode)
    {
        return cards.Find(x => x.data.shortCode == shortCode);
    }
}
