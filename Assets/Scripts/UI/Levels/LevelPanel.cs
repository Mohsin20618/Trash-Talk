using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelPanel : UIPanel
{
    public LevelItem[] levels;

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

    private void OnEnable()
    {
        int wonMatches = PlayerProfile.gamesWon;

        foreach (LevelItem level in levels)
        {
            level.SetData(PlayerProfile.GetPlayerLevel(),PlayerProfile.GetCurrentLevelPercentage());
        }
    }
}
