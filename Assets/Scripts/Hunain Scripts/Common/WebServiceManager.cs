//Hunain
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public enum Method { GET, POST, PUT, DELETE }
public enum CACHEABLE { NULL, USER_DATA }

public class WebServiceManager : MonoBehaviour
{
    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

    [Space]
    [Header("Game Apis")]
    public string baseURL;

    [Space]
    [SerializeField] internal string customRegistrationFunction            = "";
    [SerializeField] internal string customLoginFunction            = "";
    [SerializeField] internal string updateProfileFunction            = "";
    [SerializeField] internal string signUpFunction            = "";
    [SerializeField] internal string getProfileFunction        = "";
    [SerializeField] internal string purchaseCoinsFunction     = "";
    [SerializeField] internal string startGameFunction         = "";
    [SerializeField] internal string endGameFunction           = "";
    [SerializeField] internal string globalDatabaseUsers       = "";
    [SerializeField] internal string leaderboard               = "";
    [SerializeField] internal string GetTimerForSpinAndAd             = "";
    [SerializeField] internal string UpdateTimerForSpinOrAd             = "";
    [SerializeField] internal string getLatestPlayedMembers             = "";
    [SerializeField] internal string getFriends             = "";


    public static WebServiceManager instance;

    //Live
    //--

    public delegate void OnWebServiceResponse(JObject data, string getFunction, long code);
    public  event OnWebServiceResponse onWebServiceResponse;

    public  delegate void OnWebServiceError(string error);
    public  event OnWebServiceError onWebServiceError;

    public  delegate void OnWebServiceProgress(float value);
    public  event OnWebServiceProgress onWebServiceProgress;

    [Space]
    [Space]
    public bool debug = false;
    public bool SSLPinning = false;
    private string fileName;


    [SerializeField] public PlayerProfile playerPersonalData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }


    public void APIRequest(string getFunction, Method getMethod, string rawData = null, Dictionary<string, object> getParameters = null, Action<JObject, long> OnSuccess = null, Action<string> OnFail = null, CACHEABLE cacheable = 0, bool showLoader = true, FileUplaod fileUplaod = null)
    {
        if (showLoader)
            WaitingLoader.instance.ShowHide(showLoader);

        //--
        if (debug)
            print("API Request "+ getMethod.ToString() + " " + baseURL + getFunction);

        //--
        StartCoroutine(WaitForRequest(baseURL, getFunction, getMethod, rawData, getParameters, OnSuccess, OnFail, cacheable, showLoader , fileUplaod));
    }

    IEnumerator WaitForRequest(string baseURL, string getFunction, Method getMethod, string rawData = null, Dictionary<string, object> getParameters = null, Action<JObject, long> OnSuccess = null, Action<string> OnFail = null, CACHEABLE cacheable = 0, bool showLoader = true, FileUplaod fileUplaod = null)
    {
        UnityWebRequest www;

        //--
        if (getMethod == Method.POST)
        {
            WWWForm form = new WWWForm();

            //--
            if (fileUplaod != null)
            {
                print(fileUplaod.key);
                print(fileUplaod.data);
                print(fileUplaod.name);
                print(fileUplaod.mimeType);

                form.AddBinaryData(fileUplaod.key, fileUplaod.data);

            }

            //--
            if (getParameters != null)
            {
                foreach (var item in getParameters)
                {
                    if (debug)
                        print(item.Key + " = " + item.Value.ToString());
                    //--
                    form.AddField(item.Key, item.Value.ToString());
                }
            }

            if (rawData != null)
            {
                if(debug)
                    print(rawData);

                www = UnityWebRequest.Put(baseURL + getFunction, rawData);
                www.method = "POST";
            }
            else
            {
                www = UnityWebRequest.Post(baseURL + getFunction, form);
            }
            //--

        }
        else if (getMethod == Method.DELETE)
        {
            www = UnityWebRequest.Delete(baseURL + getFunction);
        }
        else if (getMethod == Method.PUT)
        {
            if(debug)
                print("rawData = " + rawData);
            www = UnityWebRequest.Put((baseURL + getFunction), rawData);
        }
        else
        {
            string param = "";
            bool isAndSignConcatenate = false;

            if (getParameters != null)
            {
                foreach (var item in getParameters)
                {
                    param += ((isAndSignConcatenate == false ? "?" : "&") + item.Key + "=" + item.Value);

                    if (!isAndSignConcatenate)
                        isAndSignConcatenate = true;
                }
            }

            //--
            if (debug)
            {
                print("DEBUG: "+ param);
            }

            //--
            www = UnityWebRequest.Get(baseURL + getFunction + param);
        }

        //--
        if (getMethod == Method.PUT)
            www.SetRequestHeader("Content-Type", "application/json");
        else
        {
            if (rawData != null)
            {
                www.SetRequestHeader("Content-Type", "application/json");
            }
            else if (fileUplaod == null)
            {
                www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            }
            else
            {
                print("do nothing file uploading. .. ");
            }
        }


        //--
        if (debug)
        {
            print("accessToken is = " + PlayerProfile.Player_Access_Token);
        }

        if (PlayerProfile.Player_Access_Token != null)
        {
            www.SetRequestHeader("Authorization", "Bearer " + PlayerProfile.Player_Access_Token);
        }        
        
        if (SSLPinning)
        {
            www.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
        }

        yield return www.SendWebRequest();

        WaitingLoader.instance.ShowHide();

        //-- check session
        if (debug)
        {
            print("check session" + www.responseCode);
        }

        if (www.responseCode == 401)
        {
            //Ui_Controller.Instance.Logout();
            yield return null;
        }


        //--
        //if (cacheable != CACHEABLE.NULL && ResponseStatus.Check(www.responseCode))
        //{
        //    TextFileManager.instance.SaveTextFile(www.downloadHandler.text, cacheable.ToString());
        //}

        //--
        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            OnFail?.Invoke("Connection Error");
            RaiseOnWebServiceError(www.error, getFunction);
        }
        else
        {
            if (debug)
            {
                print(www.downloadHandler.text);
            }

            //--
            JObject data = new JObject();
            //try
            //{
                data = JObject.Parse(www.downloadHandler.text, jsonLoadSettings);
                //--
                //RaiseOnWebServiceResponse(data, getFunction, www.responseCode);
                //--
                OnSuccess?.Invoke(data, www.responseCode);
            //}
            //catch (Exception ex)
            //{
            //    if (debug)
            //        Debug.LogError("Exception Error" + ex.Message);

            //    OnFail?.Invoke("Exception Error" + ex.Message);
            //}
        }

        www.Dispose();
    }

    public static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore
    };


    public static JsonLoadSettings jsonLoadSettings = new JsonLoadSettings
    {
        //DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Ignore,
        LineInfoHandling = LineInfoHandling.Ignore,
        CommentHandling = CommentHandling.Ignore
    };


    private void RaiseOnWebServiceResponse(JObject data, string getFunction, long code)
    {
        WaitingLoader.instance.ShowHide();

        if(onWebServiceResponse!= null)
            onWebServiceResponse(data, getFunction, code);
    }


    private void RaiseOnWebServiceError(string error, string getFunction)
    {
        WaitingLoader.instance.ShowHide();

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            print("RaiseOnWebServiceError >- from the response of fucntion named as " + getFunction + "() and error is NotReachable");
            if(ErrorPopup.instance != null) ErrorPopup.instance.ShowMessage("There might be a network issue on your end, please try again.");
            return;
        }

        if (error != "")
        {
            if (error == "Error")
            {
                print("RaiseOnWebServiceError >- from the response of fucntion named as " + getFunction + "() and error is Check Internet");
                if (ErrorPopup.instance != null) ErrorPopup.instance.ShowMessage(error);
            }
            else
            {
                print("RaiseOnWebServiceError >- from the response of fucntion named as " + getFunction + "() and error is " + error);
                if (ErrorPopup.instance != null) ErrorPopup.instance.ShowMessage("There might be a network issue on your end, please try again.");
            }
        }
    }

    public void ReloadCallBack()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public string GetAPIName(string apiFunc)
    {
        string[] splitArr;

        splitArr = apiFunc.Split(Char.Parse("/"));
        return splitArr[splitArr.Length - 1];
    }

    public void DownloadTexture(string getFunction, Method getMethod, string rawData = null, Dictionary<string, object> getParameters = null, Action<Texture, long> OnSuccess = null, Action<string> OnFail = null, CACHEABLE cacheable = 0, bool showLoader = true, FileUplaod fileUplaod = null)
    {
        if (showLoader)
            WaitingLoader.instance.ShowHide(showLoader);

        //--
        if (debug)
            print("API Request " + getFunction);

        //--
        StartCoroutine(_DownloadTexture(getFunction , getMethod, rawData, getParameters, OnSuccess, OnFail, cacheable, showLoader, fileUplaod));
    }

    IEnumerator _DownloadTexture(string baseURL, Method getMethod, string rawData, Dictionary<string, object> getParameters, Action<Texture, long> onSuccess, Action<string> onFail, CACHEABLE cacheable, bool showLoader, FileUplaod fileUplaod)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(baseURL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            onFail?.Invoke(www.error);
        }
        else
        {
            Texture tempTexture;


            tempTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            onSuccess?.Invoke(tempTexture, www.responseCode);
        }
    }


    public void DownloadAndSetTexture(string url, bool showLoader = true, Image image = null)
    {
        if (showLoader)
            WaitingLoader.instance.ShowHide(showLoader);

        StartCoroutine(_DownloadAndSetTexture(url, showLoader, image));
    }

    IEnumerator _DownloadAndSetTexture(string url, bool showLoader = true, Image image = null)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Texture tempTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            if (image != null) image.sprite = Sprite.Create((Texture2D)tempTexture, new Rect(0.0f, 0.0f, tempTexture.width, tempTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

        }
        if (showLoader)
            WaitingLoader.instance.ShowHide(false);
    }


}

public static class ResponseStatus
{
    public static bool Check(long code)
    {
        if (code == 200 || code == 201)
            return true;
        else
            return false;
    }
}

public class FileUplaod
{
    public string key;
    public string name;
    public byte[] data;
    public string mimeType;

}

class AcceptAllCertificatesSignedWithASpecificKeyPublicKey : CertificateHandler
{
    private static string PUB_KEY = "3082010A0282010100D0BBFCCBCEB22774125D26C5BE0829B8F95F53CE5FD0B83FC6563024C4E13CC79DB69463BF3E1AAEC1286EB0D1EE0A6464AF58D92E91A7E76DECDCA4D2F84D4272FF85E8B8A88BE2642F846802BCF2FD14103BEB504C2B07FFDDA915CB4632AAE2720A66DA35260CC9B5381F99D9B87A485C299D802B1E4855B6F63EBEBB286E38C73224E9718D5B834FF9FBF5018605D841DAA228C7C3E8FFAA25B0730C33F7EDB2537A6C59093D69B5B98E31C5E78CCDCAD814A19D67B2FD2BAC53F0F57772F5A51602D960BFF38726C5D274C3D73A4192E26BFFA651D94419A5E0E8BD7F702C6B9DF38F21EE82D375E1A7787E49D35D883CA486C6B7FB03CEE5BA32E9E8D10203010001";

    protected override bool ValidateCertificate(byte[] certificateData)
    {
        X509Certificate2 certificate = new X509Certificate2(certificateData);
        
        string pk = certificate.GetPublicKeyString();

        Debug.Log("PUB_KEY is = " + pk);

        //--
        if (pk.Equals(PUB_KEY))
            return true;
        //--
        return false;
    }

}
