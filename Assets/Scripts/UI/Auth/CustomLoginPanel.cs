using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLoginPanel : UIPanel
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
        //throw new NotImplementedException();
    }
}
