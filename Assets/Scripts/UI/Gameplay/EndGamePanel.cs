using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class EndGamePanel : UIPanel
{
    public  Button nextButton;
    public Button previousButton;
    public Sprite[] botImages;

    public Text roundText;

    public Text myBidText;
    public Text roundBagsText;
    public Text myBounusText;
    public Text myRoundPointsText;
    public Text myTotalBags;
    public Text myGabPenalty;
    public Text myTotalPoints;

    public Text opponentBidText;
    public Text opponentroundBagsText;
    public Text opponentBounusText;
    public Text opponentRoundPointsText;
    public Text opponentTotalBags;
    public Text opponentGabPenalty;
    public Text opponentTotalPoints;

    public EndGamePlayerUI myProfile;
    public EndGamePlayerUI myPartnerProfile;
    public EndGamePlayerUI opponentProfile1;
    public EndGamePlayerUI opponentProfile2;

    public GameObject closeButton;
    public GameObject homeButton;

    int currentRoundShowing = 1;
    List<Round> rounds;

    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void UpdateData(System.Action<object[]> callBack, params object[] parameters)
    {

    }

    private void OnEnable()
    {
        rounds = RoundManager.instance.rounds;
        //for(int i = 0; i < r.Count; i++)
        //{
        //    print("Round:::::  " + (i+1));

        //    for(int j = 0; j < r[i].players.Count; j++)
        //    {
        //        print("Round Player:::::: " + r[i].players[j].bidPlaced);
        //    }
        //}

        currentRoundShowing = rounds.Count;

        SetUI();
    }

    public void OnNextButton()
    {
        currentRoundShowing++;
        SetUI();
    }

    public void OnPreviousButton()
    {
        currentRoundShowing--;
        SetUI();
    }

    void SetUI()
    { 
        nextButton.interactable = rounds.Count > currentRoundShowing;
        previousButton.interactable = currentRoundShowing > 1;

        Utility.DisableButtonChilds(nextButton);
        Utility.DisableButtonChilds(previousButton);

        Round currentRound = rounds[this.currentRoundShowing-1];
        Player myMainPlayer = currentRound.players[0];
        Player opponentMainPlayer = currentRound.players[1];

        Player winner = myMainPlayer.gameWinner;

        if (winner == null)
        {
            closeButton.SetActive(true);
            homeButton.SetActive(false);
        }
        else
        {
            closeButton.SetActive(false);
            homeButton.SetActive(true);
        }

        //if (Global.isMultiplayer)
        //{
        //    closeButton.GetComponent<Button>().interactable = PhotonNetwork.LocalPlayer.IsMasterClient;
        //}

        
        print("appp: " + opponentMainPlayer.name);
        print("appp2: " + opponentMainPlayer.partner.name);

        roundText.text = $"ROUND {this.currentRoundShowing}";

        myBidText.text = $"{myMainPlayer.teamBidWon}/{myMainPlayer.teamBidPlaced}";
        roundBagsText.text = $"{myMainPlayer.roundBags}";
        myBounusText.text = $"{myMainPlayer.roundBonus}";
        myRoundPointsText.text = $"{myMainPlayer.score}";
        myTotalBags.text = $"{myMainPlayer.roundTotalBags}";
        myGabPenalty.text = $"{myMainPlayer.roundGabPenalty}";
        myTotalPoints.text = $"{myMainPlayer.roundTotalPoints}";

        opponentBidText.text = $"{opponentMainPlayer.teamBidWon}/{opponentMainPlayer.teamBidPlaced}";
        opponentroundBagsText.text = $"{opponentMainPlayer.roundBags}";
        opponentBounusText.text = $"{opponentMainPlayer.roundBonus}";
        opponentRoundPointsText.text = $"{opponentMainPlayer.score}";
        opponentTotalBags.text = $"{opponentMainPlayer.roundTotalBags}";
        opponentGabPenalty.text = $"{opponentMainPlayer.roundGabPenalty}";
        opponentTotalPoints.text = $"{opponentMainPlayer.roundTotalPoints}";

        myProfile.SetUI(myMainPlayer.name, null, 0, myMainPlayer.imageURL, (winner!=null && (winner.id==myMainPlayer.id || winner.id == myMainPlayer.partner.id)));
        myPartnerProfile.SetUI(myMainPlayer.partner.name, myMainPlayer.partner.isBot ? botImages[2] : null, 0, myMainPlayer.partner.imageURL, (winner != null && (winner.id == myMainPlayer.id || winner.id == myMainPlayer.partner.id)));
        opponentProfile1.SetUI(opponentMainPlayer.name, opponentMainPlayer.isBot ? botImages[1] : null, 0, opponentMainPlayer.imageURL, (winner != null && (winner.id == opponentMainPlayer.id || winner.id == opponentMainPlayer.partner.id)));
        opponentProfile2.SetUI(opponentMainPlayer.partner.name, opponentMainPlayer.partner.isBot ? botImages[3] : null, 0, opponentMainPlayer.partner.imageURL, (winner != null && (winner.id == opponentMainPlayer.id || winner.id == opponentMainPlayer.partner.id)));
    }

    //Player Winner()
    //{
    //    Player player = null;
    //    List<Player> players = rounds[rounds.Count - 1].players;

    //    foreach (Player p in players)
    //    {

    //    }


    //    closeButton.SetActive(player==null);
    //    homeButton.SetActive(player != null);

    //    return null;
    //}

    public void OnCloseButton()
    {
    //    GameplayManager.instance.StartNextRound();
        Hide();
    }

    public void OnHomeButton()
    {
        ////Close complete game
        //SoundManager.Instance.StopBackgroundMusic();
        //UIEvents.ShowPanel(Panel.TabPanels);
        //UIEvents.HidePanel(Panel.GameplayPanel);
        //UIEvents.HidePanel(Panel.EndGamePanel);
        //Hide();
        if (Global.isMultiplayer && Photon.Pun.PhotonNetwork.InRoom)
        {
            PhotonRoomCreator.instance.LeavePhotonRoom();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

