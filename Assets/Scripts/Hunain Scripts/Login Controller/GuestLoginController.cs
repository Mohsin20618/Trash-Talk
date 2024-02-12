using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TrashTalk;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GuestLoginController : MonoBehaviour
{
    [Space]
    public PlayerProfile profile;

    [Space]
    public Button loginBtn;

    [Space]
    public Texture2D GuestUserPic;
    public List<Texture2D> AvatarsList;

    public static GuestLoginController GuestLoginControllerRef;

    private void Awake()
    {
        GuestLoginControllerRef = this;
    }

    private void Start()
    {
        loginBtn.onClick.AddListener(() => login());
        if (PlayerPrefs.GetString(ConstantVariables.AuthProvider) == ConstantVariables.Guest)
        {
            Debug.Log("ByPass Guest Login");
            ByPassLogin();
        }
    }

    void loadDataFromPRefs()
    {

        PlayerProfile.Player_UserID =   PlayerPrefs.GetString(ConstantVariables.UserID);
        PlayerProfile.Player_UserName =   PlayerPrefs.GetString(ConstantVariables.UserName);
        //PlayerProfile.Player_Email = PlayerPrefs.GetString(ConstantVariables.UserEmail);
        //PlayerProfile.Player_rawImage_Texture2D = TextureConverter.Base64ToTexture2D(PlayerPrefs.GetString("Picture"));
        //PlayerProfile.authProvider =   PlayerPrefs.GetString(ConstantVariables.AuthProvider);
        //PlayerProfile.Player_coins =   PlayerPrefs.GetInt("Coins");
        //Controller.instance.Home_Screen.GetComponent<HomeScreen>().ChipsSettter();
    }

    private void ByPassLogin()
    {
        loadDataFromPRefs();

        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        keyValuePairs.Add("UserID", PlayerProfile.Player_UserID);
        keyValuePairs.Add("FullName", PlayerProfile.Player_UserName);
        //keyValuePairs.Add("Email", PlayerProfile.Player_Email);
        //keyValuePairs.Add("Password", PlayerProfile.Player_Password);
        //keyValuePairs.Add("AuthProvider", PlayerProfile.authProvider);


        WebServiceManager.instance.APIRequest(WebServiceManager.instance.signUpFunction, Method.POST, null, keyValuePairs, OnLoginSuccess, OnFail, CACHEABLE.NULL, true, null);
    }

    private void OnLoginSuccess(JObject resp, long arg2)
    {
        Debug.Log("OnLoginSuccess: " + resp.ToString());

        var playerData = PlayerData.FromJson(resp.ToString());
        PlayerProfile.UpdatePlayerData(playerData.User);
        PlayerProfile.SaveDataToPrefs();
        PlayerProfile.showPlayerDetails();
        CountDownTimer.instance.SetTimerData();
        PhotonConnectionController.Instance.ConnectingToPhoton();


        UIEvents.ShowPanel(Panel.TabPanels);
        UIEvents.ShowPanel(Panel.GameSelectPanel);
        UIEvents.HidePanel(Panel.SignupPanel);

    }

    private void OnFail(string obj)
    {
        Debug.LogError("OnFail: " + obj.ToString());
    }

    private void login()
    {
        string guestUserID = GuestLoginGenerator.GenerateUniqueUserId();
        string guestName = GuestLoginGenerator.GenerateUniqueName();
        string guestEmail = GuestLoginGenerator.GenerateUniqueEmail();
        string guestPassword = GuestLoginGenerator.GenerateRandomPassword();

        PlayerProfile.authProvider = ConstantVariables.Guest;

        // Use the generated email and password for guest login
        if (!string.IsNullOrEmpty(guestEmail) && !string.IsNullOrEmpty(guestPassword) && !string.IsNullOrEmpty(guestUserID) && !string.IsNullOrEmpty(guestName))
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add("UserID", guestUserID);
            keyValuePairs.Add("FullName", guestName);
            keyValuePairs.Add("Email", guestEmail);
            keyValuePairs.Add("Password", guestPassword);
            keyValuePairs.Add("AuthProvider", PlayerProfile.authProvider);

            WebServiceManager.instance.APIRequest(WebServiceManager.instance.signUpFunction, Method.POST, null, keyValuePairs, OnLoginSuccess, OnFail, CACHEABLE.NULL, true, null);
        }
    }


    public void ChangeScreen()
    {
        //Home_Screen.GetComponent<HomeScreen>().ChipsSettter();//.HomeScreenDataSetter();
        //Home_Screen.gameObject.SetActive(true);
    }
}


public class GuestLoginGenerator
{
    private const string namePrefix = "Guest_";
    private const string passwordSuffix = "_pass";

    static string uniqueId = Random.Range(1, 51).ToString();

    public static string GenerateUniqueEmail()
    {
        string email = namePrefix.ToLower() + uniqueId + "@trashtalk.com";
        return email;
    }

    public static string GenerateUniqueUserId()
    {
        string userID = SystemInfo.deviceUniqueIdentifier;
        return userID;
    }

    public static string GenerateUniqueName()
    {
        string userID = namePrefix + uniqueId;
        return userID;
    }

    public static string GenerateRandomPassword()
    {
        string password = namePrefix.ToLower() + uniqueId + passwordSuffix;
        return password;
    }
}
