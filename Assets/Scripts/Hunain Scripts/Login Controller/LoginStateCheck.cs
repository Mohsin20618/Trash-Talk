using UnityEngine;
using UnityEngine.UI;

public class LoginStateCheck : MonoBehaviour
{

    public Button AppleLoginBtn;

    void Start()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            PlayerProfile.Player_OS = "Android";
            AppleLoginBtn.gameObject.SetActive(false);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            PlayerProfile.Player_OS = "iOS";
            AppleLoginBtn.gameObject.SetActive(true);
        }
        else
        {
            PlayerProfile.Player_OS = "Editor";
            AppleLoginBtn.gameObject.SetActive(false);
        }

        //if (PlayerPrefs.HasKey(ConstantVariables.AuthProvider))
        //{
        //    Debug.Log("%%%%%%%%%%%" + gameObject.name);
        //    UIEvents.ShowPanel(Panel.TabPanels);
        //    UIEvents.HidePanel(Panel.SignupPanel);
        //}
    }
}