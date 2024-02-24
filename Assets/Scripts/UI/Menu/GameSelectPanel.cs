using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine.UI;

public class GameSelectPanel : UIPanel
{
    public RewardPanel rewardPanel;
    public GameObject spinAvailableSprite;
    public GameObject adAvailableSprite;
    public GameObject downPanel;
    public GameObject gameModePanel;
    public GameObject roomSelectionPanel;
    public GameObject joinRoomPopup;
    public List<GameObject> BtnList;
    public List<RectTransform> roomSelectionCard;

    private void Awake()
    {
        EventManager.OnAdOrSpinAvailable += OnAdorSpinAvailable;
        OnPanelCentered(2,2);
    }

    private void OnEnable()
    {
        //StartCoroutine(MainButtonAnimation());
        scriptWithUnityEvent.OnPanelCentered.AddListener(OnPanelCentered);

    }

    // Reference to the script containing the UnityEvent
    public SimpleScrollSnap scriptWithUnityEvent;


    private void OnDisable()
    {
        // Unregister the listener function from the UnityEvent
        scriptWithUnityEvent.OnPanelCentered.RemoveListener(OnPanelCentered);
    }

    // Listener function that matches the signature of the UnityEvent
    private void OnPanelCentered(int value1, int value2)
    {
        Debug.Log("Panel Centered Event Raised: " + value1 + ", " + value2);

        // Handle the event here

        ScaleTheCard(value1);


    }

    void ScaleTheCard(int value = -1) 
    {
        foreach (var item in roomSelectionCard)
        {
            item.LeanScale(Vector3.one,0);

            item.GetComponent<Button>().interactable = false;
        }
        if (value != -1)
        {
            roomSelectionCard[value].LeanScale(Vector3.one * 1.2f, 0.2f);
            roomSelectionCard[value].GetComponent<Button>().interactable = true;
        }
    }
    private void OnDestroy()
    {
        EventManager.OnAdOrSpinAvailable -= OnAdorSpinAvailable;
    }
    private void OnAdorSpinAvailable(bool isSpin)
    {
      if (isSpin)
            spinAvailableSprite.SetActive(true);
      else
            adAvailableSprite.SetActive(true);
    }
    public override void Show()
    {
        Debug.Log("Show.............", this);
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        Debug.Log("Hide.............", this);
        gameObject.SetActive(false);
    }

    public override void UpdateData(Action<object[]> callBack, params object[] parameters)
    {
    }

    public void OnPractice()
    {
        //    OpenTabPanel();
        Global.isMultiplayer = false;
        UIEvents.HidePanel(Panel.TabPanels);
        UIEvents.ShowPanel(Panel.GameplayPanel);

        //Hunain
        VoiceManager.instance.EnableDisableVoiceManager();
        ChatHandler.instance.EnableDisableChatManager(false);
    }
    public void OnQuickMatchButtonPressed()
    {
        //    OpenTabPanel();
        Global.isMultiplayer = true;
        Global.isPrivate = false;
        //UIEvents.HidePanel(Panel.TabPanels);
        //UIEvents.ShowPanel(Panel.GameplayPanel);

        ToggleRoomSelection(true);

        //Hunain
        //VoiceManager.instance.EnableDisableVoiceManager();
        //ChatHandler.instance.EnableDisableChatManager(false);
    }
    public void OnCreateRoomButtonPressed()
    {
        //    OpenTabPanel();
        Global.isMultiplayer = true;
        Global.isPrivate = true;

        ToggleRoomSelection(true);
    }
    public void OnJoinRoomButtonPressed()
    {
        joinRoomPopup.SetActive(true);
    }
    public void JoinRoomCloseButtonPressed()
    {
        joinRoomPopup.SetActive(false);
    }
    public void ToggleRoomSelection(bool toggle)
    {
        downPanel.SetActive(!toggle);
        gameModePanel.SetActive(!toggle);
        roomSelectionPanel.SetActive(toggle);
    }
    public void OnMultiplayer()
    {
        //   OpenTabPanel();
        UIEvents.UpdateData(Panel.TabPanels, null, "SelectPanel", 4);
    }
    public void ShowSpinWheel()
    {
        if (Global.isSpinAvailable)
        {
            UIEvents.HidePanel(Panel.TabPanels);
            UIEvents.ShowPanel(Panel.SpinWheel);
            spinAvailableSprite.SetActive(false);
        }
        else
        {
            CountDownTimer.instance.ToggleTimerForSpinAndAds(true);
        }
    }
    public void ShowAds()
    {
        if (Global.isAdAvailable)
        {


            AdManager.instance.ShowRewardedAd(RewardRecieved);
            adAvailableSprite.SetActive(false);
        }
        else
        {
            CountDownTimer.instance.ToggleTimerForSpinAndAds(false);
        }
    }
    public void RewardRecieved() 
    {
        CountDownTimer.instance.UpdateAdTimer("500");
        rewardPanel.SetCoinText("500");
    }
    public void ShowLeaderBoard()
    {
        UIEvents.ShowPanel(Panel.LeaderboardPanel);
    }
    public void ShareonNativeApp()
    {
        new NativeShare().SetUrl("Will be store link").Share();
    }
    void OpenTabPanel()
    {
        //   UIEvents.ShowPanel(Panel.TabPanels);
        //   Hide();


    }
    public IEnumerator MainButtonAnimation()
    {
        foreach (var item in BtnList)
        {
            item.transform.localScale = Vector3.zero;
        }
        yield return new WaitForSeconds(1f);
        foreach (var item in BtnList)
        {
            yield return new WaitForSeconds(0.2f);
            LeanTween.scale(item, Vector3.one, 0.1f).setEase(LeanTweenType.easeInOutQuad);
        }
    }
}
