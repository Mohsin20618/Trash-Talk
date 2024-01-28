using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class GameplayPanel : UIPanel
{
    public GameObject cardIntroPanel;
    public Text messageText;
    public Image trashTalk;
    private Action<object[]> callBack;

    public List<Sprite> jokerTrashTalks;
    public List<Sprite> queensTrashTalks;
    public List<Sprite> jacksTrashTalks;
    public List<Sprite> kingsTrashTalks;
    public List<Sprite> acesTrashTalks;

    public static bool forceQuit = false; //To Handle Player Left Sce

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

        switch (type)
        {
            case "ShowCardIntro":
                this.callBack = callBack;
                ShowCardIntro();
                break;
            case "ShowHideMessage":
                this.callBack = callBack;
                bool show = (bool)parameters[1];
                string message = (string)parameters[2];
                ShowHideMessage(show,message);
                break;
            case "ShowTrashTalk":
                string suit = (string)parameters[1];
                string card = (string)parameters[2];
                ShowTrashTalk(suit, card);
                break;

        }
    }

    void ShowHideMessage(bool show, string message="")
    {
        messageText.text = message;
        messageText.gameObject.SetActive(show);
    }

    void ShowCardIntro()
    {
        cardIntroPanel.SetActive(true);
        cardIntroPanel.GetComponentInChildren<AnimationEvents>().SetCallBack(this.callBack);
    }

    private void OnEnable()
    {
        forceQuit = false;
    }
    void ShowTrashTalk(string suit, string card)
    {
        Sprite sprite = null;
        if (card != "Ace" && card != "King" && card != "Queen" && card != "Jack" && card != "Joker")
            return;



        if(card == "Ace")
        {
            if (suit == Card.Suit.Diamonds.ToString())
                sprite = acesTrashTalks[0];
            else if (suit == Card.Suit.Hearts.ToString())
                sprite = acesTrashTalks[1];
            else if (suit == Card.Suit.Clubs.ToString())
                sprite = acesTrashTalks[2];
            else if (suit == Card.Suit.Spades.ToString())
                sprite = acesTrashTalks[3];
            else
                sprite = acesTrashTalks[4];
        }
        else if (card == "King")
        {
            if (suit == Card.Suit.Diamonds.ToString())
                sprite = kingsTrashTalks[0];
            else if (suit == Card.Suit.Hearts.ToString())
                sprite = kingsTrashTalks[1];
            else if (suit == Card.Suit.Clubs.ToString())
                sprite = kingsTrashTalks[2];
            else if (suit == Card.Suit.Spades.ToString())
                sprite = kingsTrashTalks[3];
            else
                sprite = kingsTrashTalks[4];

        }
        else if (card == "Queen")
        {
            if(suit == Card.Suit.Diamonds.ToString())
                sprite = queensTrashTalks[0];
            else if (suit == Card.Suit.Hearts.ToString())
                sprite = queensTrashTalks[1];
            else
                sprite = queensTrashTalks[Utility.GetRandom(2, queensTrashTalks.Count)];

        }
        else if (card == "Jack")
        {
            sprite = jacksTrashTalks[Utility.GetRandom(0, jacksTrashTalks.Count)];
        }
        else if (card == "Joker")
        {
            sprite = jokerTrashTalks[Utility.GetRandom(0, jokerTrashTalks.Count)];
        }

        if (sprite == null)
            return;


        trashTalk.gameObject.SetActive(false);

        trashTalk.sprite = sprite;

        trashTalk.gameObject.SetActive(true);
        Invoke("HideTrashTalk", 1);
    }

    void HideTrashTalk()
    {
        trashTalk.gameObject.SetActive(false);
    }


    public void OnHomeButton()
    {
        Time.timeScale = 0;
        UIEvents.ShowPanel(Panel.Popup);
        UIEvents.UpdateData(Panel.Popup, (obj) => {

            Time.timeScale = 1;

            if ((int)obj[0] == 2)//on yes
            {
                forceQuit = true;
                if (Global.isMultiplayer && Photon.Pun.PhotonNetwork.InRoom)
                {
                    PhotonRoomCreator.instance.LeavePhotonRoom();
                }
                SoundManager.Instance.StopBackgroundMusic();
                Hide();
                UIEvents.ShowPanel(Panel.TabPanels);
                //   UIEvents.ShowPanel(Panel.GameSelectPanel);
            }
        }, "SetData", "Are you sure you want to leave game?", "NO", "YES");

    }
}
