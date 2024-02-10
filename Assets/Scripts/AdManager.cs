using UnityEngine;
using UnityEngine.UI; // Import the Unity UI namespace
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    public static AdManager instance;

    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
    //private string _adUnitId = "ca-app-pub-3951308962114290/9279242535";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3951308962114290/2149097097";
#else
  private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#endif
    private RewardedAd rewardedAd;
    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        Debug.Log("Initializing.");

        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
          InitCalled();
        });

    }
    private void OnDestroy()
    {
        rewardedAd.Destroy();
    }

    private void InitCalled()
    {
        Debug.Log("SDK Initialized.");
        LoadRewardedAd();
    }


     
    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {

        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
                RegisterEventHandlers(rewardedAd);
            });
    }

    public void ShowRewardedAd()
    {

        Debug.Log("ShowRewardedAd.");

        const string rewardMsg = "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null)
        {
            if (rewardedAd.CanShowAd())
            {
                rewardedAd.Show((Reward reward) =>
                {
                    // TODO: Reward the user.
                    Debug.Log("Rewarding user");
                });
            }
            else
            {
                Debug.Log("rewardedAd.CanShowAd(): " + rewardedAd.CanShowAd());
            }
        }
    }
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
}