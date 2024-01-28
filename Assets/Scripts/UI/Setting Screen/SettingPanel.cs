using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPanel : UIPanel
{

    public Button soundButton;
    public Button termsOfServices;
    public Button privacyPolicy;
    public Button rulesBtn;

    public Button logout;
    public static bool insideGamePlayScreen;

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void UpdateData(Action<object[]> callBack, params object[] parameters)
    {

    }

    private void OnEnable()
    {
        logout.gameObject.SetActive(!insideGamePlayScreen);
    }

    // Start is called before the first frame update
    void Start()
    {
        //logout.onClick.AddListener(()=> LogOutCallBack());
        termsOfServices.onClick.AddListener(()=> TC_CallBack());
        privacyPolicy.onClick.AddListener(()=> HTP_CallBack());
        rulesBtn.onClick.AddListener(()=> Rules_CallBack());
        soundButton.onClick.AddListener(()=> Sound_CallBack());
        
    }

    private void Sound_CallBack()
    {
    }

    private void Rules_CallBack()
    {

    }

    void ToggleValueChanged(Toggle m_Toggle)
    {
        if (m_Toggle.isOn) { };
    }

    private void HTP_CallBack()
    {

    }

    private void TC_CallBack()
    {

    }

    /// <summary>
    /// Calling this from inspection
    /// </summary>
    public void LogOutCallBack()
    {
        //UIEvents.HidePanel(Panel.TabPanels);
        //UIEvents.HidePanel(Panel.GameSelectPanel);

        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
