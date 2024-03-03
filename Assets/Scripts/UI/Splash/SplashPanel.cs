using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SplashPanel : UIPanel
{
    public RectTransform progressBar;
    public List<GameObject> fillersObjects = new List<GameObject>();   
    private float maxWidth;
    private float loadingTime = 10f;

    
    private void Start()
    {
        if (GameplayPanel.forceQuit)
        {
            ChangeScreen();
        }
        else
        {
            StartCoroutine(LoadGame());
        }
    }

    private IEnumerator LoadGame()
    {
        float elapsedTime = 0f;
        float targetWidth = progressBar.GetComponentInParent<RectTransform>().rect.width;
        float initialWidth = 0f;
        int i = 0;
        while (elapsedTime < loadingTime)
        {


            //float progress = elapsedTime / loadingTime;
            //float currentWidth = Mathf.Lerp(initialWidth, targetWidth, progress);

            //progressBar.sizeDelta = new Vector2(currentWidth, progressBar.rect.height);

            if (i < fillersObjects.Count)
                fillersObjects[i].SetActive(true);
            else
                break;
            elapsedTime += Time.deltaTime;
            i++;
            yield return new WaitForSeconds(.5f);
        }

        progressBar.sizeDelta = new Vector2(targetWidth, progressBar.rect.height);

        yield return new WaitForSeconds(1f);
        ChangeScreen();
    }


    void ChangeScreen() 
    {
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
