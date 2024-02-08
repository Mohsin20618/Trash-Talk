using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingSubPanel : MonoBehaviour
{
    public GameObject popUp;
    public Button closeBtn;
    private void Awake()
    {
        closeBtn.onClick.AddListener(() => { CloseBtnCallback(); });
    }
    private void OnEnable()
    {
        popUp.transform.localScale = Vector3.zero;
        LeanTween.scale(popUp, Vector3.one, .5f).setEaseOutBack();
    }
    private void CloseBtnCallback()
    {
        gameObject.SetActive(false);
        UIEvents.ShowPanel(Panel.SettingPanel);
    }

}
