using GoogleMobileAds.Api;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrashTalk;
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
    private void Start()
    {

        //Debug.Log("GameRulesManager.matchDetails.timeDifference: " + GameRulesManager.matchDetails.matchDate);
        //double spinTime = (Time.time * 1000f) + 120000f;
        //double adsTime = (Time.time * 1000f) + 220000f;
        //StartCountdown(spinTime, adsTime);
        //hide();
    }
    #region Set Timer of Admob and Spinwheel
    public void SetTimerData()
    {
        Debug.Log("?? SetTimerData");
        WebServiceManager.instance.APIRequest(WebServiceManager.instance.GetTimerForSpinAndAd, Method.GET, null, null, OnSuccess, OnFail, CACHEABLE.NULL, true, null);
    }
    void OnSuccess(JObject resp, long arg2)
    {
        TimerDataForSpinAndAdd timers = DeSerialize.FromJson<TimerDataForSpinAndAdd>(resp.ToString());
        Debug.Log("??timer " + timers.Data.RemainingSpinTimer.ToUnixTimeMilliseconds());
        Epoch.UpdateDiff(timers.ServerTime.ToUnixTimeMilliseconds()); //Server Current Time
        double spinTime = timers.Data.RemainingSpinTimer.ToUnixTimeMilliseconds();
        double adTime = timers.Data.RemainingAdTimer.ToUnixTimeMilliseconds();
        StartCountdown(spinTime, adTime);
    }
    void OnFail(string msg)
    {
        print("Timer Api Fail Message: " + msg);
    }
    public void StartCountdown(double StartingTimeforSpin, double StartingTimeforAds)
    {
        StartCoroutine(CountdownTimerforSpin(StartingTimeforSpin)); //Watch Time
        StartCoroutine(CountdownTimerforAds(StartingTimeforAds)); //Watch Time
    }
    #endregion

    #region Upadte Ad Timer
    public void UpdateAdTimer(string coins)
    {
        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();

        keyValuePairs.Add("timerType", ConstantVariables.Ad);
        keyValuePairs.Add("Coins", coins);
        WebServiceManager.instance.APIRequest(WebServiceManager.instance.UpdateTimerForSpinOrAd, Method.POST, null, keyValuePairs, TimerUpdateSuccessAd, OnFail, CACHEABLE.NULL, true, null);
    }
    private void TimerUpdateSuccessAd(JObject resp, long arg2)
    {
        OnRewardSuccess(500);
        TimerDataForSpinAndAdd timers = DeSerialize.FromJson<TimerDataForSpinAndAdd>(resp.ToString());
        Epoch.UpdateDiff(timers.ServerTime.ToUnixTimeMilliseconds()); //Server Current Time
        double adTime = timers.Data.RemainingAdTimer.ToUnixTimeMilliseconds();
        Global.isAdAvailable = false;
        StartCoroutine(CountdownTimerforAds(adTime));
    }
    #endregion

    #region Reward Coins
    void OnRewardSuccess(int coins)
    {
        PlayerProfile.Player_coins += coins;

        if (EventManager.UpdateUI != null)
            EventManager.UpdateUI.Invoke("UpdateCoins");
    }

    #endregion
    #region Update Spin Timer
    public void UpdateSpinTimer(string coins)
    {
        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();

        keyValuePairs.Add("timerType", ConstantVariables.Spin);
        keyValuePairs.Add("Coins", coins);
        WebServiceManager.instance.APIRequest(WebServiceManager.instance.UpdateTimerForSpinOrAd, Method.POST, null, keyValuePairs, TimerUpdateSuccessSpin, OnFail, CACHEABLE.NULL, true, null);
    }
    private void TimerUpdateSuccessSpin(JObject resp, long arg2)
    {
        OnRewardSuccess(500);
        TimerDataForSpinAndAdd timers = DeSerialize.FromJson<TimerDataForSpinAndAdd>(resp.ToString());
        Epoch.UpdateDiff(timers.ServerTime.ToUnixTimeMilliseconds()); //Server Current Time
        double spinTime = timers.Data.RemainingSpinTimer.ToUnixTimeMilliseconds();
        Global.isSpinAvailable = false;
        StartCoroutine(CountdownTimerforSpin(spinTime));
    }
    #endregion

    public void ToggleTimerForSpinAndAds(bool isSpin)
    {
        show();
        if (isSpin)
        {
            DescriptionText.text = "Spin Rewarded!\nKeep an eye on the timer for next reward. Happy gaming!";
            timeTextForAds.gameObject.SetActive(false);
            timeTextForSpin.gameObject.SetActive(true);
        }
        else
        {
            DescriptionText.text = "Ad Watched!\nKeep an eye on the timer for next spin. Happy gaming!";
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

    private IEnumerator CountdownTimerforSpin(double StartingTimeforSpin)
    {
        Debug.Log("?? spin Timer Start");
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

                timeTextForSpin.text = remainingHours.ToString("00") + ":" + remainingMinutes.ToString("00") + ":" + ((int)(timeLeft % 60)).ToString("00");


                //timeText.text = "00:" + timeLeft.ToString("00");
            }
            else
            {
                Debug.Log("ended");
                Global.isSpinAvailable = true;
                EventManager.OnAdOrSpinAvailable?.Invoke(true);
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }    
    private IEnumerator CountdownTimerforAds(double StartingTimeforAds)
    {
        Debug.Log("?? ads Timer Start");
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

                timeTextForAds.text = remainingHours.ToString("00") + ":" + remainingMinutes.ToString("00") + ":" + ((int)(timeLeft % 60)).ToString("00");


                //timeText.text = "00:" + timeLeft.ToString("00");
            }
            else
            {
                Debug.Log("ended");
                Global.isAdAvailable =true;
                EventManager.OnAdOrSpinAvailable?.Invoke(false);
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
