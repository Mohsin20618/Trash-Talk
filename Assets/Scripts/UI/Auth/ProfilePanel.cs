using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using TrashTalk;
using Newtonsoft.Json.Linq;
using System.Globalization;

public class ProfilePanel : UIPanel
{

    public InputField displayName;
    public InputField FirstName;
    public InputField LastName;
    public InputField Cont_Num;
    public InputField email;
    public InputField password;
    public Button submitBtn;

    public Text playerName;
    public Text country;
    public Text coins;
    public Text playedCount;
    public Text winCount;
    public Text levelCount;

    public Button pictureBtn;
    public Image flagImage;
    public GameObject avatarPopup;
    public GameObject profilePopup;

    public List<Countries> countries = new();

    PlayerProfile profile;

    public Texture2D oldTexture;

    public override void Show()
    {
        gameObject.SetActive(true);
        avatarPopup.SetActive(false);
    }

    public void Start()
    {
        email.interactable = false;
        password.interactable = false;
        submitBtn.onClick.AddListener(()=> UpdateProfile());
        pictureBtn.onClick.AddListener(()=> ShowAvatarPopup());
    }
    void OnEnable()
    {
        oldTexture = PlayerProfile.Player_rawImage_Texture2D;
        UpdateUI();
        EventManager.UpdateProfilePic += UpdateProfilePic;

    }


    private void UpdateProfilePic(Sprite sprite)
    {
        Debug.Log("Thumb Update");
        PlayerProfile.Player_rawImage_Texture2D = sprite.texture;
        pictureBtn.GetComponent<Image>().sprite = sprite;
    }

    void OnDisable()
    {
        UpdateProfile();
        EventManager.UpdateProfilePic -= UpdateProfilePic;
    }
    private void ShowAvatarPopup()
    {
        avatarPopup.SetActive(true);
        profilePopup.SetActive(false);
    }

    public void BackFromAvatarPopup()
    {
        avatarPopup.SetActive(false);
        profilePopup.SetActive(true);
    }

    private void UpdateProfile()
    {
        if (PlayerProfile.authProvider == ConstantVariables.Apple || PlayerProfile.authProvider == ConstantVariables.Facebook)
        {
            MesgBar.instance.show("Can't Update Profile.");
            return;
        }

        if (PlayerProfile.Player_rawImage_Texture2D == oldTexture)
        {
            Debug.Log("Nothing to update.");
            return;
        }

        FileUplaod fileUplaod = new FileUplaod();
        fileUplaod.data = TextureConverter.Texture2DToBytes((Texture2D)pictureBtn.GetComponent<Image>().mainTexture);
        fileUplaod.name = "profilePic";
        fileUplaod.key = "Image";

        PlayerProfile.Player_UserName = displayName.text;
        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        keyValuePairs.Add("FullName", PlayerProfile.Player_UserName);
        
        WebServiceManager.instance.APIRequest(WebServiceManager.instance.updateProfileFunction, Method.POST, null, keyValuePairs, UpdateData, OnFail, CACHEABLE.NULL, true, fileUplaod);
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
    void UpdateUI()
    {
        FirstName.text = PlayerPrefs.GetString("FirstName", "");
        LastName.text = PlayerPrefs.GetString("LastName", "");
        Cont_Num.text = PlayerPrefs.GetString("Cont_Num", "");
        displayName.text = PlayerProfile.Player_UserName;
        email.text = PlayerProfile.Player_Email;

        Sprite sprite = Sprite.Create(PlayerProfile.Player_rawImage_Texture2D, new Rect(0, 0, PlayerProfile.Player_rawImage_Texture2D.width, PlayerProfile.Player_rawImage_Texture2D.height), Vector2.zero);

        pictureBtn.GetComponent<Image>().sprite = sprite;

        Sprite flagSprite = GetFlag();
        flagImage.sprite =  flagSprite != null ? flagSprite : countries[0].countryFlag;


        playerName.text = PlayerProfile.Player_UserName;
        country.text = PlayerProfile.PlayerCountry;
        coins.text = PlayerProfile.Player_coins.ToString();
        playedCount.text = PlayerProfile.gamesPlayed.ToString();
        winCount.text = PlayerProfile.gamesWon.ToString();
        levelCount.text = PlayerProfile.level.ToString();
    }

    Sprite GetFlag()
    {
        foreach (var item in countries)
        {
            if (item.countryCode.ToLower().Trim() == CountryCode.code.ToLower().Trim())
            {
                Debug.Log("ss ");
                return item.countryFlag;
            }
        }

        return null;
    }

    public void OnSubmitButton()
    {
        UIEvents.ShowPanel(Panel.Popup);
        UIEvents.UpdateData(Panel.Popup, null, "SetData", "Profile updated", "", "OK");

        Dictionary<string, object> keyValuePairs2 = new Dictionary<string, object>();
        keyValuePairs2.Add("UserID", PlayerProfile.Player_UserID);
        WebServiceManager.instance.APIRequest(WebServiceManager.instance.getProfileFunction, Method.POST, null, keyValuePairs2, OnProfileSuccess, OnFail, CACHEABLE.NULL, true, null);
    }

    private void OnProfileSuccess(JObject resp, long arg2)
    {
        Debug.Log("OnProfileSuccess: " + resp.ToString());

        var playerData = PlayerData.FromJson(resp.ToString());
        PlayerProfile.UpdatePlayerData(playerData.User);
    }


}

[Serializable]
public class Countries
{
    public string countryCode;
    public Sprite countryFlag;
}