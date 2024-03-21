using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TrashTalk;

public class TabPanels : UIPanel
{
    public Transform buttons;
    public Transform panels;

    public Text coinsText;

    //public RectTransform levelBar;
    public Image levelBarProgreess;
    public Text ProgreesCount;
    public Text ProgreesTotal;
    public Text levelText;

    public Text playerName;
    public Image profileThumb;
    public GameObject profileImageLoader;
    //   private string profileImageURL = "https://i.pravatar.cc/300";

    public delegate void ClickAction();
    public static event ClickAction OnClicked;

    int lastActiveIndex = 0;
    float levelBarWidth;

    private void Awake()
    {
        //levelBarWidth = levelBar.sizeDelta.x;
    }

    private void OnEnable()
    {
        EventManager.UpdateUI += UpdateUI;
        EventManager.UpdateProfilePic += UpdateProfilePic;

        UpdateUI("coins");
        //SelectPanel(3);
    }

    void OnDisable()
    {
        EventManager.UpdateUI -= UpdateUI;
        EventManager.UpdateProfilePic -= UpdateProfilePic;
    }

    private void UpdateProfilePic(Sprite sprite)
    {
        Debug.Log("Thumb Update");
        profileThumb.sprite = sprite;
    }

    //void UpdateUI(string type)
    //{
    //    switch (type)
    //    {
    //        case "UpdateCoins":
    //            UpdateCoinsUI();
    //            break;
    //    }
    //}

    private void Start()
    {
        playerName.text = PlayerProfile.Player_UserName;

        if (PlayerProfile.imageUrl != null && PlayerProfile.imageUrl != "")
        {
            ImageCacheManager.instance.CheckOrDownloadImage(PlayerProfile.imageUrl, this.profileThumb, DownloadCallBack);
        }
        else
            profileImageLoader.SetActive(false);
    }

    void DownloadCallBack(Texture2D texture2D)
    {
        profileImageLoader.SetActive(false);
    }




    public void UpdateUI(string type)
    {
        coinsText.text = PlayerProfile.Player_coins.ToString();
        playerName.text = PlayerProfile.Player_UserName;
        SetLevelUI();
    }


    void SetLevelUI()
    {
        List<int>  Leveldata = Utility.CalculateUserLevel(PlayerProfile.gamesWon);


        Debug.Log("Level " + Leveldata[0]);
        Debug.Log("winnings " + Leveldata[1]);
        Debug.Log("WinningsThreshhold " + Leveldata[2]);

        PlayerProfile.level = Leveldata[0];
        levelText.text = Leveldata[0].ToString();

        ProgreesCount.text = (Leveldata[1].ToString());
        ProgreesTotal.text = "/" + (Leveldata[2].ToString());

        float fillAmount = (float) Leveldata[1] / (float) Leveldata[2];
        Debug.Log("fillamount " + fillAmount);
        levelBarProgreess.fillAmount = fillAmount;  
        //Vector2 newSizeDelta = new Vector2(fillAmount * levelBar.sizeDelta.x, levelBar.sizeDelta.y);
        //levelBar.sizeDelta = new Vector2(((progress/100) * levelBarWidth), levelBar.sizeDelta.y);
        //levelBar.sizeDelta = newSizeDelta;
    }

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
            case "SelectPanel":
                int panelIndex = (int)parameters[1];
                SelectPanel(panelIndex);
                break;
        }

     }

    public void ToggleTabBar(bool show)
    {
        buttons.gameObject.SetActive(show);
    }

    public void OnBackButton()
    {
        UIEvents.ShowPanel(Panel.SignupPanel);
        Hide();
    }

    public void SelectPanel(int index)
    {
        if (index == lastActiveIndex)
            return;

        //if (index == 3)
        //{
        //    UIEvents.ShowPanel(Panel.GameSelectPanel);
        //    Hide();
        //    return;
        //}

        //ToggleTabBar(true);
        //ToggleTabBar(index == 3);

        if (index == 4)
            {
            Hide();
            UIEvents.ShowPanel(Panel.FriendsPanel);
            //UIEvents.UpdateData(Panel.FriendsPanel, null, "SelectPanel", 1);
            return;
        }

        panels.GetChild(index).gameObject.SetActive(true);
        panels.GetChild(lastActiveIndex).gameObject.SetActive(false);

        lastActiveIndex = index;
    }

}
