using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public static Controller instance;

    //screens
    public GameObject loginScreenParent;
    public GameObject loginScreen;
    public GameObject registerScreen;
    public GameObject buyMoreChips;
    public LoadingScreenTimeout loadingScreen;
    public PhotonRoomCreator PhotonRoomCreatorRef;

    public void disableLoadingScreen()
    {
       if(loadingScreen.gameObject.activeSelf) loadingScreen.gameObject.SetActive(false);
    }
     
    public void enableDisableHome_Screen(bool status)
    {
        //Home_Screen.gameObject.SetActive(status);
    }

    public void enableDisableLoginScreen(bool status)
    {
        loginScreen.gameObject.SetActive(status);
    }

    public void enableDisableLoginScreenParent(bool status)
    {
        loginScreenParent.gameObject.SetActive(status);
    }

    public void enableDisableRegisterScreen(bool status)
    {
        registerScreen.gameObject.SetActive(status);
    }



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
