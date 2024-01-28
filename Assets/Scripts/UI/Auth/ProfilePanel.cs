using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using TrashTalk;
using Newtonsoft.Json.Linq;

public class ProfilePanel : UIPanel
{

    public InputField displayName;
    public InputField FirstName;
    public InputField LastName;
    public InputField Cont_Num;
    public InputField email;
    public InputField password;
    public Button submitBtn;

    PlayerProfile profile;

    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public void Start()
    {
        email.interactable = false;
        password.interactable = false;
        submitBtn.onClick.AddListener(()=> UpdateProfile());
    }

    private void UpdateProfile()
    {
        if (PlayerProfile.authProvider == ConstantVariables.Apple || PlayerProfile.authProvider == ConstantVariables.Facebook)
        {
            MesgBar.instance.show("Can't Update Profile.");
            return;
        }

        
        //if (displayName.text == PlayerProfile.Player_UserName)
        //{
        //    MesgBar.instance.show("Nothing to update.");
        //    return;
        //}

        PlayerProfile.Player_UserName = displayName.text;
        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        keyValuePairs.Add("FullName", PlayerProfile.Player_UserName);

        WebServiceManager.instance.APIRequest(WebServiceManager.instance.updateProfileFunction, Method.POST, null, keyValuePairs, UpdateData, OnFail, CACHEABLE.NULL, true, null);
    }

    private void UpdateData(JObject arg1, long arg2)
    {
        PlayerPrefs.SetString("FirstName", FirstName.text);
        PlayerPrefs.SetString("LastName", LastName.text);
        PlayerPrefs.SetString("Cont_Num", Cont_Num.text);
        OnSubmitButton();
        //MesgBar.instance.show("Profile updated successfully");
    }

    private void OnFail(string obj)
    {
        MesgBar.instance.show(obj);
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

        UpdateUI();
    }

    void UpdateUI()
    {
        FirstName.text = PlayerPrefs.GetString("FirstName", "");
        LastName.text = PlayerPrefs.GetString("LastName", "");
        Cont_Num.text = PlayerPrefs.GetString("Cont_Num", "");
        displayName.text = PlayerProfile.Player_UserName;
        email.text = PlayerProfile.Player_Email;
    }

    public void OnSubmitButton()
    {
        UIEvents.ShowPanel(Panel.Popup);
        UIEvents.UpdateData(Panel.Popup, null, "SetData", "Profile updated", "", "OK");
    }


}
