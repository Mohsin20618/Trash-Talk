using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralPopupMessageScript : MonoBehaviour
{
    public Text PopupText;

    public static GeneralPopupMessageScript generalPopupMessageScript_Ref;

    private void Start()
    {
        generalPopupMessageScript_Ref = this;
    }

    public void ClosePopupBtn()
    {
        Debug.Log("Close button pressed");
        LeanTween.scale(gameObject, new Vector3(0f, 0f, 0), 0.5f);
    }

    

    public void OpenPopupPanel(GameObject panel)
    {
        LeanTween.scale(panel, new Vector3(1.1f, 1.1f, 1), 0.3f);
        LeanTween.scale(panel, new Vector3(1f, 1f, 1), 0.2f).setEase(LeanTweenType.easeInQuad).setDelay(0.3f);
    }



    public void ClosePopupPanel(GameObject panel)
    {
        Debug.Log("Close button pressed");
        LeanTween.scale(panel, new Vector3(0f, 0f, 0), 0.3f);
    }


    public void OpenPopupPanel(string PopupMessage, bool isWarning)
    {
        Color setColor;
        if (isWarning)
            setColor = new Color(1, 0.08962262f, 0.08962262f, 1);
        else
            setColor = Color.white;

        PopupText.text = PopupMessage;
        PopupText.color = setColor;
        LeanTween.scale(gameObject, new Vector3(1.3f, 1.3f, 0), 0.5f);
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 0), 0.3f).setEase(LeanTweenType.easeInQuad).setDelay(0.5f);
    }

    private void Awake()
    {
        //OpenPopupPanel("You Won", true);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
