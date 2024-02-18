using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPanel : UIPanel
{
    public GameObject popUp;
    public GameObject ppScreen;
    public GameObject rulesScreen;
    public GameObject tcScreen;

    public Button soundButton;
    public Button musicButton;
    public Button termsOfServices;
    public Button privacyPolicy;
    public Button rulesBtn;

    public Button logout;
    public static bool insideGamePlayScreen;

    public ButtonToggle soundToggle;
    public ButtonToggle musicToggle;

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
        popUp.transform.localScale = Vector3.zero;
        LeanTween.scale(popUp, Vector3.one, .5f).setEaseOutBack();
    }

    // Start is called before the first frame update
    void Start()
    {
        //logout.onClick.AddListener(()=> LogOutCallBack());
        termsOfServices.onClick.AddListener(()=> TC_CallBack());
        privacyPolicy.onClick.AddListener(()=> HTP_CallBack());
        rulesBtn.onClick.AddListener(()=> Rules_CallBack());
        soundButton.onClick.AddListener(()=> Sound_CallBack());
        musicButton.onClick.AddListener(()=> Music_CallBack());
        
    }

    private void Sound_CallBack()
    {
        soundToggle.OnPressToggle();

        PlayerPrefs.SetInt(ConstantVariables.m_Sound, soundToggle.isOn ? 1 :0);
        Global.isSoundOn = soundToggle.isOn;
    } 
    private void Music_CallBack()
    {
        musicToggle.OnPressToggle();

        PlayerPrefs.SetInt(ConstantVariables.m_Music, musicToggle.isOn ? 1 :0);
        Global.isMusicOn = musicToggle.isOn;
    }

    private void Rules_CallBack()
    {
        rulesScreen.SetActive(true);
        //UIEvents.HidePanel(Panel.SettingPanel);
    }

    void ToggleValueChanged(Toggle m_Toggle)
    {
        if (m_Toggle.isOn) { };
    }

    private void HTP_CallBack()
    {
        ppScreen.SetActive(true);
        //UIEvents.HidePanel(Panel.SettingPanel);
    }

    private void TC_CallBack()
    {
        tcScreen.SetActive(true);
        //UIEvents.HidePanel(Panel.SettingPanel);
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
