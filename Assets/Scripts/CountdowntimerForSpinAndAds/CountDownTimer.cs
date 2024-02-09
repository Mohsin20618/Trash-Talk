using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    public static CountDownTimer instance;
    public GameObject popUp;
    public Text DescriptionText;
    public Text timeTextForSpin;
    public Text timeTextForAds;
    public Button okButton;


    private void Awake()
    {
        instance = this;
        okButton.onClick.AddListener(() => { hide(); });
    }

    public void ToggleTimerForSpinAndAds(bool isSpin)
    {
        show();
        if (isSpin)
        {
            DescriptionText.text = "Next Spin Will Be in";
            timeTextForAds.gameObject.SetActive(false);
            timeTextForSpin.gameObject.SetActive(true);
        }
        else
        {
            DescriptionText.text = "Next Ad Will Be in";
            timeTextForAds.gameObject.SetActive(true);
            timeTextForSpin.gameObject.SetActive(false);
        }
    }
    public void show()
    {
        popUp.SetActive(true);
    }
    public void hide() 
    {
        popUp.SetActive(false);    
    }

    private void Start()
    {

        //Debug.Log("GameRulesManager.matchDetails.timeDifference: " + GameRulesManager.matchDetails.matchDate);
        double spinTime = (Time.time * 1000f)+ 120000f;
        double adsTime = (Time.time * 1000f)+ 220000f;
        StartCountdown(spinTime, adsTime);
        hide();
    }
    public void StartCountdown(double StartingTimeforSpin , double StartingTimeforAds)
    {
        double servertime = (Time.time * 1000f);
        Epoch.UpdateDiff(servertime); //Server Current Time
        //Debug.Log("Server diff " + Epoch.instance.serverTimeDiff);
        StartCoroutine(CountdownTimerforSpin(StartingTimeforSpin)); //Watch Time
        StartCoroutine(CountdownTimerforAds(StartingTimeforAds)); //Watch Time
    }
    private IEnumerator CountdownTimerforSpin(double StartingTimeforSpin)
    {
        while (true)
        {
            double timeLeft = Epoch.SecondsLeft(StartingTimeforSpin);
            if (timeLeft > 0)
            {

                int totalMinute = (int)timeLeft / 60;
                int toatalHours = totalMinute / 60;
                int days = (int)toatalHours / 24;

                int remainingMinutes = totalMinute % 60;
                int remainingHours = toatalHours % 24;

                timeTextForSpin.text = days.ToString("00") + ":" + remainingHours.ToString("00") + ":" + remainingMinutes.ToString("00") + ":" + ((int)(timeLeft % 60)).ToString("00");


                //timeText.text = "00:" + timeLeft.ToString("00");
            }
            else
            {
                Debug.Log("ended");
                Global.isSpinAvailable = true;
                EventManager.OnAdorSpinAvailable?.Invoke(true);
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }    
    private IEnumerator CountdownTimerforAds(double StartingTimeforAds)
    {
        while (true)
        {
            double timeLeft = Epoch.SecondsLeft(StartingTimeforAds);
            if (timeLeft > 0)
            {

                int totalMinute = (int)timeLeft / 60;
                int toatalHours = totalMinute / 60;
                int days = (int)toatalHours / 24;

                int remainingMinutes = totalMinute % 60;
                int remainingHours = toatalHours % 24;

                timeTextForAds.text = days.ToString("00") + ":" + remainingHours.ToString("00") + ":" + remainingMinutes.ToString("00") + ":" + ((int)(timeLeft % 60)).ToString("00");


                //timeText.text = "00:" + timeLeft.ToString("00");
            }
            else
            {
                Debug.Log("ended");
                Global.isAdAvailable =true;
                EventManager.OnAdorSpinAvailable?.Invoke(false);
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
