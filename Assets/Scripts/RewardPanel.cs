using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanel : MonoBehaviour
{
    public List<GameObject> AnimateObjects;
    public Text coinText;
    public Button tapToCollectBtn;
    private void Awake()
    {
        tapToCollectBtn.onClick.AddListener(() => { BtnCallback(); });
    }
    private void OnEnable()
    {
        StartCoroutine(MainButtonAnimation());
    }
    private void BtnCallback()
    {
        UIEvents.ShowPanel(Panel.TabPanels);
        UIEvents.HidePanel(Panel.SpinWheel);
        gameObject.SetActive(false);
    }
    public void SetCoinText(string coins)
    {
        coinText.text = coins;
        gameObject.SetActive(true);
    }
    public IEnumerator MainButtonAnimation()
    {
        foreach (var item in AnimateObjects)
        {
            item.transform.localScale = Vector3.zero;
        }
        //yield return new WaitForSeconds(1f);
        foreach (var item in AnimateObjects)
        {
            yield return new WaitForSeconds(0.1f);
            LeanTween.scale(item, Vector3.one, 0.1f).setEase(LeanTweenType.easeOutBack);
        }
    }
}