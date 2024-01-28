using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TrashTalk;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationScreen : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public InputField emailInput;
    public Button registerButton;

    [Header("Login Assets")]
    public InputField emailInputLogin;
    public InputField passwordInputLogin;
    public Button loginButton;

    private void Start()
    {
        // Attach a listener to the registerButton
        registerButton.onClick.AddListener(()=> signup());
        loginButton.onClick.AddListener(()=> login());

        if (PlayerPrefs.GetString(ConstantVariables.AuthProvider) == ConstantVariables.Custom)
        {
            Debug.Log("ByPass Login");
            ByPassLogin();
        }
    }

    void loadDataFromPRefs()
    {

        PlayerProfile.Player_UserID = PlayerPrefs.GetString(ConstantVariables.UserID);
        PlayerProfile.Player_UserName = PlayerPrefs.GetString(ConstantVariables.UserName);
        PlayerProfile.Player_Email = PlayerPrefs.GetString(ConstantVariables.UserEmail);
        PlayerProfile.Player_Password = PlayerPrefs.GetString(ConstantVariables.UserPassword);
    }

    private void ByPassLogin()
    {
        loadDataFromPRefs();

        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();

        string email = PlayerProfile.Player_Email;
        string password = PlayerProfile.Player_Password;
        keyValuePairs.Add("Email", email);
        keyValuePairs.Add("Password", password);

        //Debug.LogError("&&&&&&&&&&: "+ email);
        //Debug.LogError("&&&&&&&&&&: "+ password);

        WebServiceManager.instance.APIRequest(WebServiceManager.instance.customLoginFunction, Method.POST, null, keyValuePairs, OnLoginSuccess, OnFail, CACHEABLE.NULL, true, null);
    }

    private void OnLoginSuccess(JObject resp, long arg2)
    {
        if (arg2 == 200 || arg2 == 201)
        {
            Debug.Log("OnLoginSuccess: " + resp.ToString());

            var playerData = DeSerialize.FromJson<PlayerDataForCustom>(resp.ToString());
            Debug.Log("playerData: " + Serialize.ToJson(playerData.User));
            PlayerProfile.UpdatePlayerData(playerData.User);
            PlayerProfile.SaveDataToPrefs();
            PlayerProfile.showPlayerDetails();


            PhotonConnectionController.Instance.ConnectingToPhoton();

            UIEvents.ShowPanel(Panel.TabPanels);
            UIEvents.HidePanel(Panel.CustomLoginPanel);
            UIEvents.HidePanel(Panel.SignupPanel);
        }
        else
        {
            MesgBar.instance.show("No user found.");
        }
    }

    private void OnSignUpSuccess(JObject resp, long arg2)
    {
        if (arg2 == 200 || arg2 == 201)
        {
            Debug.Log("On SignUp Success: " + resp.ToString());

            MesgBar.instance.show("Account Created Successfully.", false);
            UIEvents.ShowPanel(Panel.CustomLoginPanel);
            UIEvents.HidePanel(Panel.RegistrationPanel);
        }
        else
        {
            MesgBar.instance.show("Sign Up Fail.");
        }
    }

    private void OnFail(string obj)
    {
        Debug.LogError("OnFail: " + obj.ToString());
        MesgBar.instance.show("Error: " + obj, true);

    }

    private void login()
    {
        //string guestUserID = GuestLoginGenerator.GenerateUniqueUserId();
        //string username = usernameInput.text;
        string password = passwordInputLogin.text.Trim();
        string email = emailInputLogin.text.Trim();



        // Perform basic validation (you should implement more robust validation)
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
        {
            MesgBar.instance.show("All fields are required.", true);
            return;
        }


        // Email Validation
        if (!CheckValidation.EmailValidaton(email))
        {
            MesgBar.instance.show("Invalid Email.", true);
            return;
        }


        PlayerProfile.authProvider = ConstantVariables.Custom;

        // Use the generated email and password for guest login
        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            //keyValuePairs.Add("UserID", guestUserID);
            //keyValuePairs.Add("FullName", username);
            keyValuePairs.Add("Email", email);
            keyValuePairs.Add("Password", password);
            //keyValuePairs.Add("AuthProvider", PlayerProfile.authProvider);

            WebServiceManager.instance.APIRequest(WebServiceManager.instance.customLoginFunction, Method.POST, null, keyValuePairs, OnLoginSuccess, OnFail, CACHEABLE.NULL, true, null);
        }
    }

    private void signup()
    {
        //string guestUserID = GuestLoginGenerator.GenerateUniqueUserId();
        string username = usernameInput.text;
        string password = passwordInput.text;
        string email = emailInput.text;



        // Perform basic validation (you should implement more robust validation)
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email)|| string.IsNullOrEmpty(username))
        {
            MesgBar.instance.show("Please fill in all fields.", true);
            return;
        }


        // Email Validation
        if (!CheckValidation.EmailValidaton(email))
        {
            MesgBar.instance.show("Invalid Email.", true);
            return;
        }


        

        PlayerProfile.authProvider = ConstantVariables.Custom;

        // Use the generated email and password for guest login
        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(username))
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            //keyValuePairs.Add("UserID", guestUserID);
            keyValuePairs.Add("FullName", username);
            keyValuePairs.Add("Email", email);
            keyValuePairs.Add("Password", password);
            keyValuePairs.Add("AuthProvider", PlayerProfile.authProvider);
            //keyValuePairs.Add("AuthProvider", PlayerProfile.authProvider);

            WebServiceManager.instance.APIRequest(WebServiceManager.instance.customRegistrationFunction, Method.POST, null, keyValuePairs, OnSignUpSuccess, OnFail, CACHEABLE.NULL, true, null);
        }
    }


    public void BackFromRegistration()
    {
        UIEvents.ShowPanel(Panel.SignupPanel);
        UIEvents.HidePanel(Panel.RegistrationPanel);
        UIEvents.HidePanel(Panel.CustomLoginPanel);
    }
}