//using System;
//using System.Collections;
//using System.IO;
//using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CompleteProfile : MonoBehaviour
{
    [Space]
    [Header("User Complete Profile UI Objects")]
    public RawImage     profilepic_RawImage;
    public InputField   userName_inputField;
    public InputField   userEmail_inputField;
    public Button       finish_Button;

    [Space]
    [Header("User Stored Profile Data")]
    public PlayerProfile playerProfile_ref;

    public Text MessageText;

    //[Space]
    //[Header("Next Screen")]
    //public GameObject nextScreen;
    string base64ImageString;

    private void Awake()
    {
        if (!playerProfile_ref)
        {
            playerProfile_ref = FindObjectOfType<PlayerProfile>();
        }
    }

    private void OnEnable()
    {
        FetchandShowUserStoreData();
    }
    // Start is called before the first frame update
    void Start()
    {
        finish_Button.onClick.AddListener(()=> UpdatePlayerProfileData());
    }

    void FetchandShowUserStoreData()
    {
        var userName_Old = PlayerProfile.Player_UserName;
        var userEmail_Old = PlayerProfile.Player_Email;
        Texture2D texture2D = PlayerProfile.Player_rawImage_Texture2D;

        if (userEmail_Old != null && userName_Old != null)
        {
            userName_inputField.text = userName_Old;
            userEmail_inputField.text = userEmail_Old;
            if(texture2D!=null) profilepic_RawImage.texture = texture2D;
        }
        else
            Debug.LogError("No data present in Player Profile Class");
    }


    void UpdatePlayerProfileData()
    {
        if (userName_inputField.text == PlayerProfile.Player_UserName
            && PlayerProfile.Player_Email == userEmail_inputField.text
            && PlayerProfile.Player_rawImage_Texture2D == (Texture2D)profilepic_RawImage.texture)
        {
            ShowSuccessMessage("Nothing to Update.");
            return;
        }

        finish_Button.interactable = false;
        string userName     = PlayerProfile.Player_UserName           =    userName_inputField.text;
        string email        = PlayerProfile.Player_Email              =    userEmail_inputField.text;
        Texture2D texture2D = PlayerProfile.Player_rawImage_Texture2D =    (Texture2D)profilepic_RawImage.texture;

        PlayerPrefs.SetString(ConstantVariables.UserEmail, email);
        PlayerPrefs.SetString(ConstantVariables.UserName, userName);
        PlayerPrefs.Save();

        base64ImageString = TextureConverter.Texture2DToBase64(texture2D);
        SaveBase64Image(base64ImageString);
        //apiController.PlayerRegistrationApiMethod(PlayerProfile.Player_UserName, PlayerProfile.Player_UserID, PlayerProfile.Player_Email, "Apple", base64ImageString);
        ShowSuccessMessage("Data Updated Successfully");
    }

    public void SaveBase64Image(string base64String)
    {
        PlayerPrefs.SetString("Picture", base64String);
        PlayerPrefs.Save();
    }

    private void ShowSuccessMessage(string msg)
    {
        MessageText.text = msg;
        finish_Button.interactable = true;

        Invoke("HideText", 1f);
    }


    private void HideText()
    {
        MessageText.text = "";
    }


}
