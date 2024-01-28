using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameSelectPanel : UIPanel
{
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

    void OpenTabPanel()
    {
     //   UIEvents.ShowPanel(Panel.TabPanels);
     //   Hide();


    }
}
