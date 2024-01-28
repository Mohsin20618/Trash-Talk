using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Photon.Realtime;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using TrashTalk;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{

    public static GameplayManager instance;

    public Text gameStartingText;

    public GameObject playButton;
    public HandCardsUI cardHand;

    public SelectPartnerPanel partnerPanel;
  //  private Trick currentTrick;

    public int totalPlayers;
    int currentPlayerIndex;
    public int totalPlayerPlayed;  //total Player played one round

    CardDeck cardDeck;
    BidManager bidManager;
    BotTrick botTrick;

    private Hashtable _myCustomProperties = new Hashtable();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private GameplayManager()
    {

    }

    private void OnEnable()
    {
        //print("onenable");
        //ResetPanelData();

        if (!Global.isMultiplayer)
        {
            PlayerManager.instance.ClearPlayers();
            PlayerManager.instance.AddPlayer("YOU", $"P{0}", null, true,true, false, 0);
            PlayerManager.instance.AddPlayer("Bot 1", $"BP{1}", null, false, false, true, 1);
            PlayerManager.instance.AddPlayer("Bot 2", $"BP{2}", null, false, false, true, 2);
            PlayerManager.instance.AddPlayer("Bot 3", $"BP{3}", null, false, false, true, 3);
        }
        else
        {
            ShowMultiplayerMessage(true, Photon.Pun.PhotonNetwork.IsMasterClient?"Waiting for other players to join the game.":"Waiting for master to start the game.");
        }

        cardDeck = GetComponentInChildren<CardDeck>();
        bidManager = GetComponent<BidManager>();
        botTrick = new BotTrick();
        RoundManager.instance.ClearAllRounds();
      //  this.totalPlayers = PlayerManager.instance.players.Count;
        //     this.totalPlayerPlayed = 0;
        SoundManager.Instance.PlayBackgroundMusic(Sound.Music);

        print("is multiplayer: " + Global.isMultiplayer);

        SetPlayButton(!Global.isMultiplayer);
        StartNewGame();
    }

    private void Start()
    {
    //    if (!Global.isMultiplayer)
            SetPlayersData();
    }

    public void SetPlayersData()
    {
        print("setting player data");
        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "SetPlayersData");
    }



    void ResetContainers()
    {
        cardDeck.ClearDeck();
        cardHand.ClearHandCards();
        TableController.instance.ClearAllContainers();
    }

    public void SetPlayButton(bool isActive)
    {
        playButton.SetActive(isActive);
    }


    public void StartNewGame()
    {
        TableController.instance.ShowSideTable(false);
        //playButton.SetActive(true);
        ResetGame();
    }


    public void ResetGame()
    {
        ResetContainers();
        bidManager.ClearPlayersBid();
        //playButton.SetActive(true);
        this.totalPlayers = PlayerManager.instance.players.Count;
        Global.isSpadeActive = false;

        if (Global.isMultiplayer)
            this.currentPlayerIndex = PlayerManager.instance.GetMasterIndex();
        else
            this.currentPlayerIndex = Random.Range(0, totalPlayers);


        //    PlayerManager.instance.ClearPlayersData();

        TrickManager.ResetTrick();
      //  TableController.instance.ClearCards();

        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "ResetUI");

        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "SetPlayersData");
    }


    public IEnumerator AutomaticallyStartGame()
    {

        gameStartingText.gameObject.SetActive(true);
        for (int i = 5; i >= 0; i--)
        {
            gameStartingText.text = "Round will starts in " + i + " sec . . .";
            yield return new WaitForSeconds(1);
        }

        OnPlayGameButton();
    }

    public void OnPlayGameButton()
    {
        StopCoroutine(nameof(AutomaticallyStartGame)); //Not using

        gameStartingText.gameObject.SetActive(false);

        if (Global.isMultiplayer && PhotonNetwork.InRoom)
        {
            //Hunain
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            string shuffledCards = cardDeck.GetShuffleCardsString();

            #region Comments

            /*
                        partnerPanel.gameObject.SetActive(true);
                        partnerPanel.SetData(PlayerManager.instance.player,(id)=>
                        {
                            CreateBotPlayerForMultiplayer();

                            for(int i=0;i< PlayerManager.instance.player.Count; i++)
                            {
                                print("Before Swiping: " + PlayerManager.instance.player[i].id);
                            }

                            if(id != "")
                            {
                                List<Player> players = PlayerManager.instance.player;

                                int selectedIDIndex = players.FindIndex(x => x.id == id);
                                int replacingIndex = 2;

                                Player temp = players[replacingIndex];
                                players[replacingIndex] = players[selectedIDIndex];
                                players[selectedIDIndex] = temp;
                            }

                            string sortedIds = string.Join(",", PlayerManager.instance.player.ConvertAll(x => x.id.ToString()).ToArray());

                            print("all ids: " + sortedIds);

                            for (int i = 0; i < PlayerManager.instance.player.Count; i++)
                            {
                                print("After Swiping: " + PlayerManager.instance.player[i].id);
                            }

                             PhotonRPCManager.Instance.SpawnPlayers(shuffledCards, sortedIds);

                        });

             */

            #endregion Comments

            CreateBotPlayerForMultiplayer();
            string sortedIds = string.Join(",", PlayerManager.instance.player.ConvertAll(x => x.id.ToString()).ToArray());
            PhotonRPCManager.Instance.SpawnPlayers(shuffledCards, sortedIds);

        }
        else if(!Global.isMultiplayer)
        {
            AnimateCardsScreen();
        }
        //if (Photon.Pun.PhotonNetwork.InRoom && Photon.Pun.PhotonNetwork.IsMasterClient)
        //{
        //    cardDeck.CreateInitialDeck();
        //    cardDeck.ShowHideCardContainer(false);
        //}


        //  playButton.SetActive(false);
        //  UIEvents.UpdateData(Panel.GameplayPanel, OnCardsCoverScreen, "ShowCardIntro");
    }

    string multiplayerCards = null;
    string sortedPlayersIds = "";
    public void AnimateCardsScreen(string shuffledCards=null, string sortedIds = "")
    {

        if (Global.isMultiplayer)
        {
            PlayerProfile.Player_coins -= Global.coinsRequired;

            if (EventManager.UpdateUI != null)
                EventManager.UpdateUI.Invoke("UpdateCoins");
        }
      

        UIEvents.HidePanel(Panel.EndGamePanel);//it is needed to be closed, if other players are still on endgame screen and master started game already

        ShowMultiplayerMessage(false);
        this.multiplayerCards = shuffledCards;
        this.sortedPlayersIds = sortedIds;
        playButton.SetActive(false);
        UIEvents.UpdateData(Panel.GameplayPanel, OnCardsCoverScreen, "ShowCardIntro");
    }

    void OnCardsCoverScreen(object[] parameters = null)
    {
        ResetGame();
        TableController.instance.ShowSideTable();

        if (Global.isMultiplayer)
        {
            //if (Photon.Pun.PhotonNetwork.InRoom && Photon.Pun.PhotonNetwork.IsMasterClient)
            //{
            //    cardDeck.CreateInitialDeck();
            //    cardDeck.ShowHideCardContainer(false);
            //}
            List<CardData> c = cardDeck.CardsStringToList(this.multiplayerCards);
            cardDeck.CreateInitialDeck(c);
            AddRemainingPlayers();

            //    UIEvents.UpdateData(Panel.PlayersUIPanel, null, "SetPlayersData");
            Invoke("Deal", 1.5f); //will open this

            //    if(Photon.Pun.PhotonNetwork.InRoom && Photon.Pun.PhotonNetwork.IsMasterClient)
            //         Invoke("StartDealByMaster", 1.5f); //will open this


            return;
        }

        //creating this deck for practice
        cardDeck.CreateInitialDeck();
        Invoke("Deal", 1.5f); //will open this
    }

    //void StartDealByMaster(){

    //}

    void CreateBotPlayerForMultiplayer()
    {
        int bp = 0;
        for (int i = 0; i < 4; i++)
        {
            if (PlayerManager.instance.player[i].id == null)
            {
                bp++;
                PlayerManager.instance.players[i].name = $"Bot Player {bp}";
                PlayerManager.instance.players[i].isOwn = false;
                PlayerManager.instance.players[i].isMaster = false;
                PlayerManager.instance.players[i].isBot = true;
                PlayerManager.instance.players[i].id = $"BP{bp}";
                //   PlayerManager.instance.players[i].tablePosition = 0;
                //   PlayerManager.instance.players[i].photonIndex = i;
            }
        }
    }

    //Add bot players if there are less than 4 players in photon room
    void AddRemainingPlayers()
    {


        CreateBotPlayerForMultiplayer();

     //   List<string> playerIds = sortedPlayersIds.Split(',').ToList();

        //print("sortedPlayersIds : " + sortedPlayersIds);

        //for (int i = 0; i < PlayerManager.instance.player.Count; i++)
        //{
        //    print("On the other hand " + PlayerManager.instance.player[i].id);
        //}


        //PlayerManager.instance.player = PlayerManager.instance.player
        //   .OrderBy(item => sortedPlayersIds.IndexOf(item.id))
        //   .ToList();


        //for (int i = 0; i < PlayerManager.instance.player.Count; i++)
        //{
        //    print("On the other hand " + PlayerManager.instance.player[i].name);
        //}


        PlayerManager.instance.player = PlayerManager.instance.SortMultiplayerPositions();


        for (int i = 0; i < PlayerManager.instance.player.Count; i++)
        {
            print("Afterr positioning name " + PlayerManager.instance.player[i].name);
            print("Afterr positioning photon index " + PlayerManager.instance.player[i].photonIndex);
            print("Afterr positioning tablePosition " + PlayerManager.instance.player[i].tablePosition);
        }


        //for (int i = 0; i < PlayerManager.instance.player.Count; i++)
        //{
        //    print("and Here player ids: " + PlayerManager.instance.player[i].id);
        //    print("and Table Position is: " + PlayerManager.instance.player[i].tablePosition);

        //}

        //for(int r=0;r< PlayerManager.instance.players.Count; r++)
        //{
        //    print("on adding player: " + r +" : " + PlayerManager.instance.players[r].name);
        //}

        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "SetPlayersData");
    }

    Photon.Realtime.Player GetNewMasterClient() 
    {
        Photon.Realtime.Player masterPlayer = null;
        foreach (var item in PhotonNetwork.CurrentRoom.Players)
        {
            if (item.Value.IsMasterClient)
            {
                masterPlayer = item.Value;
                break;
            }
        }
        return masterPlayer;
    }

    public void ReplaceBotWithPlayer(string playerID)
    {

        //PlayerManager.instance.players[i].isMaster = PhotonNetwork.PlayerList[i].IsMasterClient;

        Photon.Realtime.Player newMasterPlayer = null;

        for (int i=0;i< PlayerManager.instance.players.Count;i++)
        {
            Player p = PlayerManager.instance.players[i];
            if (p.id == playerID)
            {
                if (p.isMaster) //if master player left the game, update the master player
                {
                    newMasterPlayer = GetNewMasterClient();
                    Debug.Log("New Master Client is: "+ newMasterPlayer.NickName);
                }
                print("Bot has Taken over player Id: " + playerID);
                MesgBar.instance.show( p.name +" Left.");

                //    p.id = $"BP{p.photonIndex}";
                p.name = $"Bot Player {i}";
                p.isBot = true;
                p.isMaster = false;
                p.isOwn = false;
            }
        }

        if (newMasterPlayer != null)
        {
            Debug.Log("Updating Master Client on client.");
            Player player = PlayerManager.instance.players.Find(x => x.id.Equals(newMasterPlayer.UserId));
            player.isMaster = true;
        }
        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "SetPlayersData");
    }

    //public List<Player> SortMultiplayerPositions()
    //{
    //    List<Player> p = PlayerManager.instance.players;
    //    int myIdIndex = p.FindIndex(x=>x.id==PlayerProfile.Player_UserID);

    //    if (myIdIndex == -1)
    //    {
    //        Debug.Log("ID not found in the list.");
    //        return PlayerManager.instance.players;
    //    }

    //    List<Player> sortedList = new List<Player>();

    //    // Add elements after your ID
    //    for (int i = myIdIndex; i < p.Count; i++)
    //    {
    //        sortedList.Add(p[i]);
    //    }

    //    // Add elements before your ID
    //    for (int i = 0; i < myIdIndex; i++)
    //    {
    //        sortedList.Add(p[i]);
    //    }

    //    return sortedList;
    //}

    public void Deal()
    {
     //   playButton.SetActive(false);

        cardDeck.DistributeCards();
    }

    public void StartBid()
    {
        cardHand.SortHandCards();
        cardHand.StartCoroutine(cardHand.ShowPlayerCards());
        //
        bidManager.StartBid(this.totalPlayers, this.currentPlayerIndex);
    }

    //Only for multiplayer
    public void GetBidFromPlayers(string playerId, int photonIndex, int selectedBid)
    {
        bidManager.OnGetPlayerBid(playerId, photonIndex, selectedBid);
    }

    void SetPlayerTurnIndication(Player p=null, bool removeFromAll=false)
    {
        foreach(Player pl in PlayerManager.instance.players)
        {
            if (removeFromAll)
                UIEvents.UpdateData(Panel.PlayersUIPanel, null, "SetTurnIndication", pl.tablePosition, false);
            else
                UIEvents.UpdateData(Panel.PlayersUIPanel, null, "SetTurnIndication", pl.tablePosition, pl.tablePosition == p.tablePosition);
        }
    }

    public void StartGame()
    {
        this.currentPlayerIndex = PlayerManager.instance.GetMasterIndex();
        StartTricks();
    }

    public void StartTricks()
    {
        this.totalPlayerPlayed = 0;
        //currentPlayerIndex = (currentPlayerIndex + 1) % 4;


        if (Global.isMultiplayer && Photon.Pun.PhotonNetwork.InRoom)
        {
            if (Photon.Pun.PhotonNetwork.IsMasterClient)
            {      
                if(TrickManager.cards.Count == 0) PhotonRPCManager.Instance.ResetTrick();
                PhotonRPCManager.Instance.SetPlayerTurn(PlayerManager.instance.player[this.currentPlayerIndex].id, PlayerManager.instance.player[this.currentPlayerIndex].photonIndex);
            }
            return;
        }

        //For Practice Mode
        PlayTurn();
    }

    public void GetPlayerTurn(string playerID,int photonIndex)
    {
        print("It is: " + playerID + " Turn");
        print("and photon index is : " + photonIndex);
        this.currentPlayerIndex = PlayerManager.instance.GetPlayerIndexByID(playerID);
        print("palyer index here: " + this.currentPlayerIndex);
        PlayTurn();
    }

    public void PlayTurn()
    {
        Player currentPlayer = PlayerManager.instance.players[this.currentPlayerIndex];
       PlayerManager.instance.SetPlayerTurn(this.currentPlayerIndex);
       SetPlayerTurnIndication(currentPlayer); 
        if (currentPlayer.isBot)
        {

            if (Global.isMultiplayer)
            {
                if (Photon.Pun.PhotonNetwork.InRoom && Photon.Pun.PhotonNetwork.IsMasterClient)
                    StartCoroutine(PlayBotTurnByMaster(currentPlayer));
            }
            else
                StartCoroutine(BotPlay(currentPlayer));
        }
        else
        {
            if (currentPlayer.isOwn)
            {
                cardHand.ActiveMainPlayerCards();
                UIEvents.UpdateData(Panel.PlayersUIPanel, null, "ShowHideYourTurnHeading", true);
            }
            else
            {
                //if we want to show anything for other player
            }
        }
    }


    IEnumerator PlayBotTurnByMaster(Player botPlayer)
    {
        yield return new WaitForSeconds(Random.Range(2, 5));
        List<Card> hand = botPlayer.hand;
        //Card playedCard = botPlayer.PlayCard(0);
        Card playedCard = botTrick.GetBestCard(botPlayer.hand);

        SendCardPlacedData(botPlayer.id, playedCard.data.shortCode);
    }

    IEnumerator BotPlay(Player botPlayer)
    {
        yield return new WaitForSeconds(Random.Range(2,5));
        List<Card> hand = botPlayer.hand;
        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "StopTimer", this.currentPlayerIndex);

        //Card playedCard = botPlayer.PlayCard(0);
        Card playedCard = botTrick.GetBestCard(botPlayer.hand);

        if (playedCard.suit == Card.Suit.Spades)
        {
            print("Spade Activated: " + playedCard.name + " By Player " + playedCard.cardOwner);
            Global.isSpadeActive = true;
        }


        print("bot has choosen: " + playedCard.name);
        playedCard.gameObject.SetActive(true);

        playedCard.SwitchSide(true);
        print("currentPlayerIndex" + this.currentPlayerIndex);

        playedCard.MoveCard(TableController.instance.GetPlayerShowCardTransform(botPlayer.tablePosition),2.5f,true,false, ()=> {
            ShowTrashTalk(playedCard);

            TrickManager.AddCard(playedCard);

            botPlayer.hand.Remove(playedCard);
            UIEvents.UpdateData(Panel.PlayersUIPanel, null, "UpdateCardCount", this.currentPlayerIndex, botPlayer.hand.Count);

            TrickManager.HighlightLowCards();

            DecideNext();

        });
    //    yield return new WaitForSeconds(1f);

    }

    public void PlaceCardOnTable(Player player, Card playedCard)
    {
        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "StopTimer", this.currentPlayerIndex);

        //cardHand.UpdateCardArrangement();
        cardHand.ActiveMainPlayerCards(false);

        if (Global.isMultiplayer)
        {
            SendCardPlacedData(player.id, playedCard.data.shortCode);
            return;
        }

        if (playedCard.suit == Card.Suit.Spades)
        {
            print("Spade Activated: " + playedCard.name + " By Player " + playedCard.cardOwner);
            Global.isSpadeActive = true;
        }

        //        cardHand.ActiveMainPlayerCards(false);

        playedCard.MoveCard(TableController.instance.GetPlayerShowCardTransform(player.tablePosition), 2.5f,true,false,()=> {
            ShowTrashTalk(playedCard);

            TrickManager.HighlightLowCards();
            cardHand.UpdateCardArrangement();
        });

        player.hand.Remove(playedCard);
        cardHand.OnUseHandCard(playedCard);
        TrickManager.AddCard(playedCard);
        DecideNext();
    }

    void ShowTrashTalk(Card card)
    {
        UIEvents.UpdateData(Panel.GameplayPanel, null, "ShowTrashTalk", card.suit.ToString(), card.data.valueName);
    }


    void SendCardPlacedData(string playerId, string cardCode)
    {
        PhotonRPCManager.Instance.PlacedCardByPlayer(playerId, cardCode);
    }

    public void OnPlacedCardByMultiplayer(string playerId, string cardCode)
    {
        //
        print("Player placed was : " + playerId);
        print("Player placed card: " + cardCode);
        Card card = cardDeck.GetCard(cardCode);
        Player currentPlayer = PlayerManager.instance.GetPlayerById(playerId);
        print("and table position of that player is: " + currentPlayer.tablePosition);
        //  UIEvents.UpdateData(Panel.PlayersUIPanel, null, "StopTimer", currentPlayer.tablePosition);
        if (card.suit == Card.Suit.Spades)
        {
            print("Spade Activated: " + card.name + " By Player " + card.cardOwner);
            Global.isSpadeActive = true;
        }

        currentPlayer.hand.Remove(card);
        cardHand.OnUseHandCard(card);
        TrickManager.AddCard(card);

        if (currentPlayer.isOwn)
        {
            card.MoveCard(TableController.instance.GetPlayerShowCardTransform(currentPlayer.tablePosition), 2.5f, true, false, () => {
                ShowTrashTalk(card);
                TrickManager.HighlightLowCards();
                cardHand.UpdateCardArrangement();
            });
        }
        else
        {
            ShowTrashTalk(card);
            card.gameObject.SetActive(true);
            card.SwitchSide(true);

            card.MoveCard(TableController.instance.GetPlayerShowCardTransform(currentPlayer.tablePosition), 2.5f, true, false, () => {
                //    TrickManager.AddCard(card);

                //    currentPlayer.hand.Remove(card);
                UIEvents.UpdateData(Panel.PlayersUIPanel, null, "UpdateCardCount", currentPlayer.tablePosition, currentPlayer.hand.Count);
                TrickManager.HighlightLowCards();
                //    DecideNext();

            });
        }

        if (Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            DecideNext();
        }
        else
        {
            this.totalPlayerPlayed++;
            print("in else total played player: " + this.totalPlayerPlayed);
            if (this.totalPlayerPlayed == this.totalPlayers)
            {
                Invoke("OnWinTrick", 1);
            }
        }
    }

    void DecideNext()
    {
        this.totalPlayerPlayed++;

        if (this.totalPlayerPlayed == this.totalPlayers)
        {
            Invoke("OnWinTrick", 1);
            return;
        }

        this.currentPlayerIndex = (this.currentPlayerIndex + 1) % this.totalPlayers;


        if (Global.isMultiplayer && Photon.Pun.PhotonNetwork.InRoom)
        {
            if (Photon.Pun.PhotonNetwork.IsMasterClient)
            {
                PhotonRPCManager.Instance.SetPlayerTurn(PlayerManager.instance.player[this.currentPlayerIndex].id, PlayerManager.instance.player[this.currentPlayerIndex].photonIndex);
            }
            return;
        }

        //Bot Player
        PlayTurn();
    }

    Player trickWinner;

    void OnWinTrick()
    {
        Player player = TrickManager.GetTrickWinner();
        this.trickWinner = player;
        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "WinnerAnimation", player.tablePosition);
        TrickManager.GiveCardsToWinner(player);
        CompleteTrick();
    }

    void CompleteTrick()
    {
        if (IsRoundOver())
        {
            RoundManager.instance.AddCurrentRoundProgress();
            SetPlayerTurnIndication(null, true);

            Invoke("OnRoundOver", 1);
        }
        else
        {
            Invoke("ResetTrick", 1);
        }

    }

    public void OnTurnTimeUp()
    {
        Player currentPlayer = PlayerManager.instance.players[this.currentPlayerIndex];

        if (currentPlayer.isOwn)
        {
            //UIEvents.UpdateData(Panel.PlayersUIPanel, null, "StopTimer", this.currentPlayerIndex);
            UIEvents.UpdateData(Panel.PlayersUIPanel, null, "ShowHideYourTurnHeading", false);
            //Card playedCard = botPlayer.PlayCard(0);
            Card card = botTrick.GetBestCard(currentPlayer.hand);
            card.PlaceCardOnTable();
        }
        if (currentPlayer.isBot && Global.isMultiplayer)
        {
            //Hunain
            Debug.Log("**********Bug Solved**********");
            PlayTurn();
        }
    }


    void ResetTrick()
    {
        Debug.Log("ResetTrick");
        TrickManager.ResetTrick();

        this.currentPlayerIndex = this.trickWinner.tablePosition;

        StartTricks();
    }

    //here we will show the animations or bonuses which players gets
    void OnRoundOver()
    {
        print("round over");

        Invoke("ShowRoundResult", 1);
        //UIEvents.ShowPanel(Panel.GameOverPanel);
    }

    void ShowRoundResult()
    {
  //    if(needNextRound) logic will be here
        StartNextRound();
        //StartCoroutine(AutomaticallyStartGame());
        UIEvents.ShowPanel(Panel.EndGamePanel);
    }

    public void StartNextRound()
    {
        //  if Need next round
        //         logic
      //  UIEvents.HidePanel(Panel.EndGamePanel);

        if (!Global.isMultiplayer)
        {
            SetPlayButton(true);
            StartNewGame();
            return;
        }

    //    TableController.instance.ShowSideTable(false);

        if (Photon.Pun.PhotonNetwork.InRoom && Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            SetPlayButton(true);
          //  ResetGame();
        }
        else
        {
            ShowMultiplayerMessage(true, "Waiting for master to start the game.");
        }
        ResetGame();
    }

    public void StartRound()
    {
        ShowMultiplayerMessage(false);
    }

    public void ShowMultiplayerMessage(bool show, string message="")
    {
        UIEvents.UpdateData(Panel.GameplayPanel, null, "ShowHideMessage", show, message);
    }

    public void OnWinningGame(Player winner)
    {
        print("1 Winner is: " + winner.name);
        if (!Global.isMultiplayer)
            return;

        int coins = (this.totalPlayers / 2) * Global.coinsRequired;
        print("2 Winner is: " + winner.name);

        if (winner.isOwn || winner.partner.isOwn)
        {
            print("3 Winner is: " + winner.name);

            PlayerProfile.Player_coins += coins;

            if (EventManager.UpdateUI != null)
                EventManager.UpdateUI.Invoke("UpdateCoins");

            string[] winners = GetWinnerIds(winner);

            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add("GameID", Global.currentGameId);


             for (int i = 0; i < winners.Length; i++)
            {
                keyValuePairs.Add($"UserIDs[{i}]", winners[i]);
            }

            keyValuePairs.Add("winCoins", coins);

            WebServiceManager.instance.APIRequest(WebServiceManager.instance.endGameFunction, Method.POST, null, keyValuePairs,

            (JObject resp, long arg2) =>
            {


            }
            , (msg) =>
            {
                print(msg);
            }, CACHEABLE.NULL, false, null);
        }
    }

    public string[] GetWinnerIds(Player winner)
    {
        List<string> ids = new List<string>();

        if (!winner.isBot)
            ids.Add(winner.id);

        if(!winner.partner.isBot)
            ids.Add(winner.partner.id);

        return ids.ToArray();
    }

    private bool IsRoundOver()
    {
        foreach (Player player in PlayerManager.instance.players)
        {
            if (player.hand.Count > 0)
            {
                return false;
            }
        }
        return true;
    }
}