using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FriendsPanel : UIPanel
{
    public Transform panels;
    public Transform tabButtons;
    
    int lastActiveIndex = 0;

    private void OnEnable()
    {
      //  SelectPanel(0);
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
        Hide();
        UIEvents.ShowPanel(Panel.TabPanels);
    }

    public void SelectPanel(int index)
    {
        if (index == lastActiveIndex)
            return;

        SetButtons(index);

        panels.GetChild(index).gameObject.SetActive(true);
        panels.GetChild(lastActiveIndex).gameObject.SetActive(false);

        lastActiveIndex = index;
    }

    void SetButtons(int index)
    {
        tabButtons.GetChild(index).GetComponent<FriendsTabButton>().SetActiveInactive(true);
        tabButtons.GetChild(lastActiveIndex).GetComponent<FriendsTabButton>().SetActiveInactive(false);
    }

    public void OnLeaderboardButton()
    {
        UIEvents.ShowPanel(Panel.LeaderboardPanel);
        Hide();
    }
}
