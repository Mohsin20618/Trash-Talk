using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class Popup :UIPanel
{
    public GameObject button1;
    public GameObject button2;
    public GameObject closeButton;

    public Text description;
    public Text button1Text;
    public Text button2Text;


    Action<object[]> callBack;
    object[] callBackData;

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
            case "SetData":
                string description = (string)parameters[1];
                this.callBack = callBack;
                SetData(description, (string)parameters[2], (string)parameters[3]);
                break;
        }
    }

    void SetData(string description,string button1Text= "CANCEL", string button2Text = "OK")
    {
        this.description.text = description;

        this.button1Text.text = button1Text;
        this.button2Text.text = button2Text;

        this.button1.SetActive(button1Text != "");
        this.button2.SetActive(button2Text != "");
        this.closeButton.SetActive(button1Text != "");
    }

    public void OnButton1()
    {
        if (this.callBack != null)
        {
            this.callBackData = new object[] { 1 };
            this.callBack(callBackData);
        }

        Hide();
    }

    public void OnButton2()
    {
        if (this.callBack != null)
        {
            this.callBackData = new object[] { 2 };
            this.callBack(callBackData);
        }
        Hide();

    }

}
