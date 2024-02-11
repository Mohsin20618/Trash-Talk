using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameSelectPanel : UIPanel
{
    public GameObject spinAvailableSprite;
    public GameObject adAvailableSprite;
    public List<GameObject> BtnList;   

    private void Awake()
    {
        EventManager.OnAdorSpinAvailable += OnAdorSpinAvailable;
    }

    private void OnEnable()
    {
        //StartCoroutine(MainButtonAnimation());
    
    }
    private void OnDestroy()
    {
        EventManager.OnAdorSpinAvailable -= OnAdorSpinAvailable;
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
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
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


            AdManager.instance.ShowRewardedAd();
            adAvailableSprite.SetActive(false);
        }
        else
        {
            CountDownTimer.instance.ToggleTimerForSpinAndAds(false);
        }
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
