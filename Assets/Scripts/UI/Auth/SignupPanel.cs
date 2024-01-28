using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SignupPanel : UIPanel
{
    public void ConnectWithFacebook()
    {
        //print("ConnectWithFacebook");
    }

    public void PlayAsGuest()
    {
        //print("Play As Guest");
        //UIEvents.ShowPanel(Panel.TabPanels);
        //Hide();
    }

    public void ConnectWithApple()
    {
        //print("ConnectWithApple");
    }

    public void CustomSignUp()
    {
        print("CustomSignUp");
        UIEvents.ShowPanel(Panel.RegistrationPanel);
        Hide();
    }

    public void CustomLogin()
    {
        print("CustomLogin");
        UIEvents.ShowPanel(Panel.CustomLoginPanel);
        Hide();
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
}
