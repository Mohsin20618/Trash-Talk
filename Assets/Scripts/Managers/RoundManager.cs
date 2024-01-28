using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//public class RoundPlayer
//{
//    public string name;
//    public int score = 0;
// //   public int tablePosition;
//    public int bidPlaced;
//    public int bidWon;
//    public bool isBot = false;
//    public bool isOwn = false;
//    public int photonIndex = 0;
//    public string id;
////    public bool isMaster;
//    public string imageURL;

//    public RoundPlayer(string playerName, string id, string image, bool isOwn, bool isBot, int bidPlaced, int bidWon)
//    {
//        this.name = playerName;
//        this.isBot = isBot;
//        this.isOwn = isOwn;
//        this.bidPlaced = bidPlaced;
//        this.bidWon = bidWon;
//        this.id = id;
//        this.imageURL = image;
//    }
//}

public class Round
{
    public List<Player> players = new List<Player>();
    //   public List<RoundPlayer> players = new List<RoundPlayer>();

    public Round(List<Player> roundPlayers)
    {
     //   SetPartners(roundPlayers);
        //foreach(Player player in roundPlayers)
        //{
        //    RoundPlayer p = new RoundPlayer(player.name,player.id,player.imageURL,player.isOwn,player.isBot, player.bidPlaced,player.bidWon);
        //    players.Add(p);
        //}

        foreach (Player player in roundPlayers)
        {

            Player p = new Player(player.name, player.id, player.imageURL, player.isOwn, player.isMaster,player.isBot,player.tablePosition);
          
            p.partner = player.partner;

            p.teamBidPlaced = player.teamBidPlaced;
            p.teamBidWon = player.teamBidWon;

            p.roundBags = player.roundBags;
            p.score = player.score;
            p.roundTotalBags = player.roundTotalBags;
            p.roundGabPenalty = player.roundGabPenalty;
            p.roundTotalPoints = player.roundTotalPoints;
            p.roundBonus = player.roundBonus;
            p.gameWinner = player.gameWinner;

            players.Add(p);
        }
    }

    //void SetPartners(List<Player> roundPlayers)
    //{
    //    roundPlayers[0].partner = roundPlayers[2];
    //    roundPlayers[1].partner = roundPlayers[3];
    //    roundPlayers[2].partner = roundPlayers[0];
    //    roundPlayers[3].partner = roundPlayers[1];
    //}
}




public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;
    public List<Round> rounds;
    public int currentRound = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        rounds = new List<Round>();
    }

    public void AddCurrentRoundProgress()
    {

        SetPartners();

        SetRoundScores();


        Round round = new Round(PlayerManager.instance.player);
        rounds.Add(round);

        
    }

    void SetPartners()
    {
        List<Player> roundPlayers = PlayerManager.instance.player;


        roundPlayers[0].partner = roundPlayers[2];
        roundPlayers[1].partner = roundPlayers[3];
        roundPlayers[2].partner = roundPlayers[0];
        roundPlayers[3].partner = roundPlayers[1];
    }

    void SetRoundScores()
    {
        List<Player> players = PlayerManager.instance.player;

        foreach (Player player in players)
        {
            int teamBidWon = player.bidWon + player.partner.bidWon;
            int teamBidPlaced = player.bidPlaced + player.partner.bidPlaced;

            player.teamBidWon = teamBidWon;
            player.teamBidPlaced = teamBidPlaced;

            int roundBag = teamBidWon > teamBidPlaced ? teamBidWon - teamBidPlaced : 0;

            player.roundBags = roundBag;

            int myScore = player.teamBidWon == player.teamBidPlaced ? player.teamBidWon * 10 : player.teamBidWon > player.teamBidPlaced ? (player.teamBidPlaced * 10) + roundBag : 0;
            //   int partnerScore = player.partner.teamBidWon == player.partner.teamBidPlaced ? player.partner.teamBidWon * 10 : player.partner.teamBidWon > player.partner.teamBidPlaced ? (player.partner.teamBidPlaced * 10) + roundBag : 0;

            //   player.score = myScore + partnerScore;

            player.roundTotalBags += roundBag;

            player.roundBonus = GetPlayerBonus(player) + GetPlayerBonus(player.partner);

            player.score = myScore + player.roundBonus;


            //int penalties = 0;


            //if (player.roundTotalBags >= 10)
            //{
            //    int setsOf10 = player.roundTotalBags / 10; // Calculate how many sets of 10 bags
            //    int penalty = setsOf10 * 100; // Calculate the penalty points

            //    // Apply the penalty
            //    penalties += penalty;

            //    // Reduce the bags by the bags covered by the penalty
            //  //  bags %= 10;
            //}

            //player.roundGabPenalty = penalties;
            //player.roundTotalPoints += (player.roundTotalPoints + player.score)- penalties;
            player.roundTotalPoints = (player.roundTotalPoints + player.score);
    //        SetTotalScoreToAllRounds(player);

        }

        Player winner = null;

        foreach (Player player in players)
        {
            player.gameWinner = Winner();
            winner = player.gameWinner;

            SetTotalScoreToAllRounds(player);
        }

        if(winner != null)
        {
            GameplayManager.instance.OnWinningGame(winner);
        }
    }

    void SetTotalScoreToAllRounds(Player player)
    {
        foreach(Round round in this.rounds)
        {
            foreach(Player p in round.players)
            {
                if(p.id == player.id)
                {
                    p.roundTotalBags = player.roundTotalBags;
                    p.roundTotalPoints = player.roundTotalPoints;
                    p.roundGabPenalty = player.roundGabPenalty;
                    p.gameWinner = player.gameWinner;
                }
            }
        }
    }

    int GetPlayerBonus(Player player)
    {
        int bonus = 0;
        //Check for Boston
        if(player.bidPlaced == Global.minBostonTricks)
        {
            if(player.bidWon < player.bidPlaced)
            {
                bonus = -Global.bostonPoints;
            }
            else if(player.bidWon == Global.maxTricks)
            {
                player.bostonWon = true;
                UIEvents.UpdateData(Panel.PlayersUIPanel, null, "ShowBunus", player.tablePosition, "Boston");
            }
            else
            {
                bonus = Global.bostonPoints;
                UIEvents.UpdateData(Panel.PlayersUIPanel, null, "ShowBunus", player.tablePosition, "10For200");
            }
        }

        if (player.bidPlaced == 0 && player.partner.bidPlaced == 0)  //Check double Nil
        {
            if (player.bidWon == 0 && player.partner.bidWon == 0)
            {
                bonus = Global.nilPoints;
                UIEvents.UpdateData(Panel.PlayersUIPanel, null, "ShowBunus", player.tablePosition, "DoubleNil");
            }
            else
            {
                bonus = -Global.nilPoints;
            }
        }
        else if (player.bidPlaced == 0)//Check Nil
        {
            if (player.bidWon == 0)
            {
                bonus = Global.nilPoints;
                UIEvents.UpdateData(Panel.PlayersUIPanel, null, "ShowBunus", player.tablePosition, "Nil");
            }
            else
            {
                bonus = -Global.nilPoints;
            }
        }

        return bonus;
    }

    Player Winner()
    {
        List<Player> players = PlayerManager.instance.player;
        Player player = null;

        foreach (Player p in players)
        {
            if (p.bostonWon)
            {
                player = p;
                break;
            }
            if (p.roundTotalPoints >= Global.scoreToWin)
            {
                if (player != null && player.roundTotalPoints > p.roundTotalPoints)
                {
                }
                else
                {
                    player = p;
                }
            }
        }

        //Bellow code is for testing purpose only and will be removed
        // foreach (Player p in players)
        // {
        //     if (p.bostonWon)
        //     {
        //         player = p;
        //         break;
        //     }
        ////     if (p.roundTotalPoints >= Global.scoreToWin)
        ////     {
        //         if (player != null && player.roundTotalPoints > p.roundTotalPoints)
        //         {
        //         }
        //         else
        //         {
        //             player = p;
        //         }
        //  //   }
        // }

        return player;
    }

    public void ClearAllRounds()
    {
        rounds.Clear();
    }
}
