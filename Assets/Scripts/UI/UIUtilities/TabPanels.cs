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

    public RectTransform levelBar;

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
        levelBarWidth = levelBar.sizeDelta.x;
    }

    private void OnEnable()
    {
        EventManager.UpdateUI += UpdateUI;

        UpdateUI("coins");
        SetLevelUI();
        SelectPanel(3);
    }

    void OnDisable()
    {
        EventManager.UpdateUI -= UpdateUI;
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
    }


    void SetLevelUI()
    {
        float progress = PlayerProfile.GetCurrentLevelPercentage();
        levelBar.sizeDelta = new Vector2((progress * levelBarWidth), levelBar.sizeDelta.y);
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

        if (index == 4)
        {
            Hide();
            UIEvents.ShowPanel(Panel.FriendsPanel);
            UIEvents.UpdateData(Panel.FriendsPanel, null, "SelectPanel", 1);
            return;
        }

        panels.GetChild(index).gameObject.SetActive(true);
        panels.GetChild(lastActiveIndex).gameObject.SetActive(false);

        lastActiveIndex = index;
    }

}
