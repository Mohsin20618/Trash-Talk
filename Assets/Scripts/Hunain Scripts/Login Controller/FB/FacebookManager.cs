using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using TrashTalk;

public class FacebookManager : MonoBehaviour
{
    [Header("FacebookScreen UI Elements")]
    public Button FbLoginBtn;

    public static List<FriendDetail> friendList = new List<FriendDetail>();
    List<string> friendsID = new List<string>();
    List<string> friendsname = new List<string>();

    [Space]
    [Header("Login Screen")]
    public GameObject LoginScreen;

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            Debug.Log("Initializing Facebook.");
            FB.Init(InitCallback);

            //FB.Init(() =>
            //{
            //    if (FB.IsInitialized)
            //    {
            //        FB.ActivateApp();
            //        Debug.Log("Activating app fb init if");
            //    }

            //    else
            //        Debug.LogError("Couldn't initialize app");
            //    if (FB.IsLoggedIn)
            //    {
            //        Debug.Log("Successfull Login");
            //        AfterSuccessfullLogin();
            //    }
            //},
            //isGameShown =>
            //{
            //    if (!isGameShown)
            //        Time.timeScale = 0;
            //    else
            //        Time.timeScale = 1;

            //});
        }
        else
        {
            Debug.Log("====>Already Initialized.");

            FB.ActivateApp();
            //Debug.Log("else Facebook Login");
            //AfterSuccessfullLogin();
        }

        // Check if the user is already logged in
        if (FB.IsLoggedIn)
        {
            // Continue with your app's logic after silent login
            Debug.Log("====>Already Logged in.");
            AfterSuccessfullLogin();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal that Facebook SDK is initialized
            FB.ActivateApp();
        }
        else
        {
            Debug.LogError("====>Failed to initialize the Facebook SDK");
        }
    }

    private IEnumerator EnableLoginScreen()
    {
        yield return new WaitForSeconds(0.5f);
        LoginScreen.SetActive(true);
    }

    private void Start()
    {
        FbLoginBtn.onClick.AddListener(() => FacebookLogin());
    }

    #region Login/ Logout

    public void FacebookLogin()
    {
        Debug.Log("====>FacebookLogin()");
        if (!FB.IsInitialized)
        {
            Debug.Log("Initializing Facebook.");
            FB.Init(InitCallback);
        }

        if (!FB.IsLoggedIn)
        {
            Debug.Log("Facebook Login");
            var permission = new List<string>() { "public_profile", "user_friends", "email" };
            FB.LogInWithReadPermissions(permission, AuthCallback);
            Debug.Log(permission);
        }
        else
        {
            Debug.LogError("Facebook is already logged in.");
        }
        //AfterSuccessfullLogin();
    }
    public void FacebookLogOut()
    {
        Debug.Log("FacebookLogOut.");
        FB.LogOut();
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // Continue with your app's logic after successful login
            AfterSuccessfullLogin();
        }
        else
        {
            Debug.Log("Facebook login failed.");
        }
    }

    #endregion

    #region if Fb is Logged in Anywhere/Any app
    public void AfterSuccessfullLogin()
    {
        // generate token and facebook id
        if (FB.IsLoggedIn)
        {
            Debug.Log("Successful Login After ");
            var atoken = AccessToken.CurrentAccessToken;
            string facebookID = AccessToken.CurrentAccessToken.UserId;

            // Print current access token's granted permissions
            //foreach (string perm in atoken.Permissions)
            //{
            //    Debug.LogError("perm: " + perm);
            //}
            //tokentext.text = atoken;
            StartCoroutine(UpdateData());
        }
    }
    /// <summary>
    /// Hits 2 different APIs
    /// one for profile picture
    /// one for name id email
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateData()
    {
        if (FB.IsLoggedIn)
        {
            string query = "/me/friends";
            FB.API("/me?fields=id,name,email", HttpMethod.GET, GetData);
            FB.API("me/picture?type=square&height=350&width=350", HttpMethod.GET, GetPicture);
            FB.API(query, HttpMethod.GET, GetFriendsData);

            yield return null;
        }
        else
            Debug.Log("Failed to Get Data");
    }

    /// <summary>
    /// gets API response for name email and userid
    /// </summary>
    /// <param name="result"></param>
    private void GetData(IGraphResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
            return;
        }
        string Player_UserName;
        string Player_UserID;
        string Player_Email;
        Debug.Log(result);
        if (result.ResultDictionary.TryGetValue("id", out Player_UserID))
        {
            PlayerProfile.Player_UserID = Player_UserID;
            Debug.Log(Player_UserID);
        }

        if (result.ResultDictionary.TryGetValue("name", out Player_UserName))
        {
            PlayerProfile.Player_UserName = Player_UserName;
            Debug.Log(Player_UserName);
        }

        if (result.ResultDictionary.TryGetValue("email", out Player_Email))
        {
            PlayerProfile.Player_Email = Player_Email;
            Debug.Log(Player_Email);
        }
        else
        {
            PlayerProfile.Player_Email = GuestLoginGenerator.GenerateUniqueEmail();
        }

        UIEvents.ShowPanel(Panel.TabPanels);
        UIEvents.HidePanel(Panel.SignupPanel);

    }


    /// <summary>
    /// gets API response for profile picture
    ///  without downloading
    /// </summary>
    /// <param name="result"></param>
    public void GetPicture(IGraphResult result)
    {
        Debug.Log(result);
        if (result.Error == null && result.Texture != null)
        {
            PlayerProfile.Player_rawImage_Texture2D = result.Texture;
        }
        else
        {
            Debug.Log(result.Error);
        }

        PlayerProfile.authProvider = ConstantVariables.Facebook;
        AttemptLogin();

    }

    private void AttemptLogin()
    {


        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        keyValuePairs.Add("UserID", PlayerProfile.Player_UserID);
        keyValuePairs.Add("FullName", PlayerProfile.Player_UserName);
        keyValuePairs.Add("Email", PlayerProfile.Player_Email);
        keyValuePairs.Add("Password", PlayerProfile.Player_Password);
        keyValuePairs.Add("AuthProvider", PlayerProfile.authProvider);
        //keyValuePairs.Add("image", PlayerProfile.imageUrl);

        WebServiceManager.instance.APIRequest(WebServiceManager.instance.signUpFunction, Method.POST, null, keyValuePairs, OnLoginSuccess, OnFail, CACHEABLE.NULL, true, null);
    }

    private void OnFail(string obj)
    {
        Debug.LogError("OnFail: " + obj.ToString());
    }

    private void OnLoginSuccess(JObject jObject, long arg2)
    {
        Debug.Log("OnLoginSuccess: " + jObject.ToString());

        var playerData = PlayerData.FromJson(jObject.ToString());
        PlayerProfile.UpdatePlayerData(playerData.User);
        CountDownTimer.instance.SetTimerData();
        PlayerProfile.SaveDataToPrefs();
        PlayerProfile.showPlayerDetails();
        PhotonConnectionController.Instance.ConnectingToPhoton();
        if (!PlayerPrefs.HasKey(Global.fbLoginRewardKey))
        {
            PurchaseThroughFacebookLogin(100,0,"facebookLogin");
        }

    }

    void PurchaseThroughFacebookLogin(int totalCoins, int price, string productID)
    {
        //InappManager.instance.PurchaseItem(productID, (payload, signature) =>
        //{
        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        keyValuePairs.Add("UserID", PlayerProfile.Player_UserID);
        keyValuePairs.Add("ProductID", productID);
        keyValuePairs.Add("Price", price);
        keyValuePairs.Add("PurchaseCoins", totalCoins);

        WebServiceManager.instance.APIRequest(WebServiceManager.instance.purchaseCoinsFunction, Method.POST, null, keyValuePairs, OnPurchaseSuccess, OnFail, CACHEABLE.NULL, true, null);

        //});
    }

    void OnPurchaseSuccess(JObject resp, long arg2)
    {
        PlayerPrefs.SetInt(Global.fbLoginRewardKey, 1);
       
        PlayerProfile.Player_coins += 100;

        if (EventManager.UpdateUI != null)
            EventManager.UpdateUI.Invoke("UpdateCoins");

        UIEvents.ShowPanel(Panel.Popup);
        UIEvents.UpdateData(Panel.Popup, null, "SetData", "Facebook Login Reward.", "", "OK");

    }

    /// <summary>
    /// gets API response for friends playing this game
    /// </summary>
    /// <param name="result"></param>
    void GetFriendsData(IGraphResult result)
    {
        friendList.Clear();
        friendsID.Clear();
        friendsname.Clear();

        var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
        var friendsList = (List<object>)dictionary["data"];
        foreach (var dict in friendsList)
        {
            string friendName = (string)((Dictionary<string, object>)dict)["name"];
            string friendId = (string)((Dictionary<string, object>)dict)["id"];

            friendsID.Add(friendId);
            friendsname.Add(friendName);
            FriendDetail friendDetail = new FriendDetail();
            friendDetail.friendUserID = friendId;
            friendList.Add(friendDetail);
        }

        DownloadFriendPicture();
    }

        
    void DownloadFriendPicture()
    {

        foreach (var item in friendsID)
        {

            FB.API("/" + item + "/picture?redirect=false&width=100&height=100", HttpMethod.GET, result =>
            {
                if (friendList.Find(x => x.friendUserID.Equals(item)) != null)
                {
                    IDictionary data = result.ResultDictionary["data"] as IDictionary;
                    string photoURL = data["url"] as string;

                    StartCoroutine(DownloadFriendPicture_FromURL(photoURL, item));

                }
            });

        }

        //NextPanel.SetActive(true);
    }


    IEnumerator DownloadFriendPicture_FromURL(string friendPic_url, string friendID/*user id */)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(friendPic_url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                int index = friendsID.IndexOf(friendID);

                Texture friendpicture = DownloadHandlerTexture.GetContent(uwr);

                var friendDetail = friendList.Find(x => x.friendUserID.Equals(friendID));
                if (friendDetail != null)
                {
                    friendDetail.friendPic = (Texture2D)friendpicture;
                    friendDetail.friendName = friendsname[index];
                    friendDetail.friendPicURl = friendPic_url;
                }

                PlayerProfile.instance.facebookFriends.Add(friendDetail);


                //GameObject Friend = Instantiate(FriendRows, Vector3.zero, Quaternion.identity, FriendRowsParent);
                //FriendRow FriendInfo = Friend.GetComponent<FriendRow>();
                //FriendInfo.userName = friendsDic[friendID].friendName;
                //FriendInfo.UserID = friendID;
                //FriendInfo.rawImage.texture = friendpicture;
                //FriendInfo.Setter();

                /////
                ///// Show Dictionary as a gameobject
                /////

                //GameObject key_Obj = new GameObject();
                //GameObject name_Obj = new GameObject();
                //GameObject texture_Obj = new GameObject();
                //key_Obj.name = friendID;

                //name_Obj.transform.SetParent(key_Obj.transform);
                //name_Obj.name = friendsDic[friendID].friendName;

                //texture_Obj.transform.SetParent(name_Obj.transform);
                //texture_Obj.name = "Profile Pic";
                //texture_Obj.AddComponent<RawImage>();
                //texture_Obj.GetComponent<RawImage>().texture = friendDetail.friendPic;

            }

        }
        #endregion

    }

    private void SetImage(FriendDetail friendDetail)
    {

    }
}
public class FriendDetail
{
    public string friendUserID;
    public string friendName;
    public string friendPicURl;
    public Texture2D friendPic;
}
