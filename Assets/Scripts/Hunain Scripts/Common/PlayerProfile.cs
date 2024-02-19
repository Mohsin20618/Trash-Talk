using System.Collections.Generic;
using UnityEngine;
using TrashTalk;
using System;

public class PlayerProfile : MonoBehaviour
{
    public static Texture2D Player_rawImage_Texture2D;

    public static string imageUrl;
    public static string Player_Access_Token;
    public static string Player_UserName;
    public static string Player_UserID;
    public static string Player_Email;
    public static string Player_Password = ConstantVariables.defaultPass;
    public static string authProvider;
    public static string Player_OS;
    public static long Player_coins;
    public static string GameId;
    public static string RoomId;
    public static bool isNewUser;
    public static string PlayerCountry;
    public static int gamesWon;
    public static int gamesPlayed;
    public static int level = 1;

    public List<User> globalUsers;
    public List<FriendDetail> facebookFriends;

    public static PlayerProfile instance;

    //public string   Player_Gender;
    public static List<string> PlayerPurchasedProductsIDs = new List<string>();
    public static Dictionary<string  /*productID*/, string /*Count*/> Dict_PlayerPurchasedProductsIDs_And_Count = new Dictionary<string, string>();

    private void Awake()
    {
        if (instance == null)
            instance = this;

        print("Instance name " + instance.name);
    }

    //Start is called before the first frame update
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        print("Current Level: " + GetPlayerLevel());
        print("Current Level %: " + GetCurrentLevelPercentage());

    }

    public static void showPlayerDetails()
    {
        Debug.Log("Player Image: " + Player_rawImage_Texture2D);
        Debug.Log("Player_Access_Token: " + Player_Access_Token);
        Debug.Log("Player_UserName: " + Player_UserName);
        Debug.Log("Player_UserID: " + Player_UserID);
        Debug.Log("Player_Email: " + Player_Email);
        Debug.Log("Player_authProvider: " + authProvider);
        Debug.Log("Player OperatingSystem: " + Player_OS);
        Debug.Log("Player_coins: " + Player_coins);
    }

    public void DisplayUserData()
    {
        //HomeScreen homeScreen = FindObjectOfType<HomeScreen>();

        //int chips = int.Parse(Player_chips);

        //if (Chips_Text == null)
        //{
        //    Debug.LogError("Chips_Text == null");
        //    Chips_Text = homeScreen.chipsText;
        //}
        //Chips_Text.text = Player_chips;

        ////Chips Set on Photon
        //PhotonNetwork.LocalPlayer.SetScore(chips);

        //Debug.LogError("DisplayUserData: Player_chips = " + Player_chips);
    }

    internal static void clearData()
    {
        Player_rawImage_Texture2D = null;
        Player_Access_Token = "";
        Player_UserName = "";
        Player_UserID = "";
        Player_Email = "";
        authProvider = "";
        Player_OS = "";
        Player_coins = 0;
        GameId = "";
        RoomId = "";
        PlayerCountry = "";
        gamesWon = 0;
        gamesPlayed = 0;
        isNewUser = false;
    }


    public static void UpdatePlayerData(User user)
    {
        if (user == null)
        {
            Debug.LogError("Error");
        }
        PlayerProfile.Player_UserID     = user.UserId;
        PlayerProfile.Player_UserName   = user.FullName;
        PlayerProfile.Player_Email      = user.Email;
        PlayerProfile.Player_Password   = user.Password;
        PlayerProfile.authProvider      = user.AuthProvider;
        PlayerProfile.Player_coins      = user.Coins;
        PlayerProfile.Player_Access_Token = user.AccessToken;
        PlayerProfile.gamesWon = user.winCount;
        PlayerProfile.imageUrl = user.ImagePath + "/" + user.Image;
        Debug.Log("PlayerProfile.Player_Access_Token&&&&&&&&&&&&&&&& " + PlayerProfile.Player_Access_Token);
        string url = user.ImagePath + "/" + user.Image;

        Debug.Log("user::::: " + user.ToJson());
        Debug.Log("user:::Image:: " + user.Image);
        Debug.Log("user:::ImagePath:: " + user.ImagePath);
        Debug.Log("url: = " + url);
        ImageCacheManager.instance.CheckOrDownloadImage(url,null,GetImage);//TextureConverter.Base64ToTexture2D(user.Image);
    }

    private static void GetImage(Texture2D texture)
    {
        PlayerProfile.Player_rawImage_Texture2D = texture;


        if (PlayerProfile.Player_rawImage_Texture2D == null)
        {
            Debug.Log("Image is null");
            return;
        }


        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        EventManager.UpdateProfilePic?.Invoke(sprite);
    }

    public static void SaveDataToPrefs()
    {

        PlayerPrefs.SetString(ConstantVariables.UserID, PlayerProfile.Player_UserID);
        PlayerPrefs.SetString(ConstantVariables.UserName, PlayerProfile.Player_UserName);
        PlayerPrefs.SetString(ConstantVariables.UserEmail, PlayerProfile.Player_Email);
        PlayerPrefs.SetString(ConstantVariables.UserPassword, PlayerProfile.Player_Password);
        PlayerPrefs.SetString(ConstantVariables.AuthProvider, PlayerProfile.authProvider);
        PlayerPrefs.Save();
    }

    public static int GetPlayerLevel()
    {
        int won = gamesWon;

        if(won >=0 && won <= 10)
        {
            return 1;
        }
        else if (won >= 10 && won <= 20)
        {
            return 2;
        }
        else if (won >= 20 && won <= 30)
        {
            return 3;
        }
        else if (won >= 30 && won <= 40)
        {
            return 4;
        }
        else if (won >= 40 && won <= 100)
        {
            return 5;
        }
        else if (won >= 100 && won <= 500)
        {
            return 6;
        }

        return 6;
    }


    public static float GetCurrentLevelPercentage()
    {
        int won = gamesWon;
        int level = GetPlayerLevel();
        float min = 0;
        float max = 0;

        switch (level)
        {
            case 1:
                min = 0;
                max = 10;
                break;
            case 2:
                min = 10;
                max = 20;
                break;
            case 3:
                min = 20;
                max = 30;
                break;
            case 4:
                min = 30;
                max = 40;
                break;
            case 5:
                min = 40;
                max = 100;
                break;
            case 6:
                min = 100;
                max = 500;
                break;

        }

        return ((won-min)/(max-min));
    }

}
