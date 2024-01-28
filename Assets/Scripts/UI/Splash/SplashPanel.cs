using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SplashPanel : UIPanel
{
    public RectTransform progressBar;
    private float maxWidth;
    private float loadingTime = 3f;

    private void Start()
    {
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        float elapsedTime = 0f;
        float targetWidth = progressBar.GetComponentInParent<RectTransform>().rect.width;
        float initialWidth = 0f;

        while (elapsedTime < loadingTime)
        {
            float progress = elapsedTime / loadingTime;
            float currentWidth = Mathf.Lerp(initialWidth, targetWidth, progress);

            progressBar.sizeDelta = new Vector2(currentWidth, progressBar.rect.height);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        progressBar.sizeDelta = new Vector2(targetWidth, progressBar.rect.height);

        yield return new WaitForSeconds(1f);
        if (PlayerPrefs.HasKey(ConstantVariables.AuthProvider) && (PlayerPrefs.GetString(ConstantVariables.AuthProvider).Equals(ConstantVariables.Guest) || PlayerPrefs.GetString(ConstantVariables.AuthProvider).Equals(ConstantVariables.Custom)))
        {
            Debug.Log("%%%%%%%%%%%" + gameObject.name);

            UIEvents.ShowPanel(Panel.TabPanels);
        }
        else
        {
            UIEvents.ShowPanel(Panel.SignupPanel);
        }
        Hide();
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void UpdateData(Action<object[]> callBack, params object[] parameters)
    {
    }
}
