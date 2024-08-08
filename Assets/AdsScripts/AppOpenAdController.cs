using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;


public class AppOpenAdController: MonoBehaviour
{
    // These ad units are configured to always serve test ads.

    public string _adUnitId = "ca-app-pub-3940256099942544/9257395921";


    private AppOpenAd appOpenAd;
    private DateTime _expireTime;

    public Handler handler;

    public static AppOpenAdController Instance;
    public bool IsAdAvailable
    {
        get
        {
            return appOpenAd != null
                   && appOpenAd.CanShowAd()
                   && DateTime.Now < _expireTime;
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    [HideInInspector]
    public bool AdShowing = false;
    public void LoadAppOpenAd()
    {
        if (!PreferenceManager.GetAdsStatus())
        {
            return;
        }
        // Clean up the old ad before loading a new one.
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }

        Debug.Log("Loading the app open ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        AppOpenAd.Load(_adUnitId, adRequest,
            (AppOpenAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("app open ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("App open ad loaded with response : "
                          + ad.GetResponseInfo());

                // App open ads can be preloaded for up to 4 hours.
                _expireTime = DateTime.Now + TimeSpan.FromHours(4);
                appOpenAd = ad;
                RegisterEventHandlers(ad);
            });
    }

    /// <summary>
    /// Shows the app open ad.
    /// </summary>
    public void ShowAppOpenAd()
    {
        if (!PreferenceManager.GetAdsStatus())
        {
            return;
        }
        if (appOpenAd != null && appOpenAd.CanShowAd())
        {
            handler.Hide_SmallBanner1Event();
            handler.Hide_SmallBanner2Event();
            Debug.Log("Showing app open ad.");
            appOpenAd.Show();
        }
        else
        {
            Debug.LogError("App open ad is not ready yet.");
        }
    }



    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            if (!AdShowing)
            {
                AdShowing = false;
                if (IsAdAvailable)
                {
                    ShowAppOpenAd();
                }

            }
            else
            {
                AdShowing = false;
            }
        }
    }
    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("App open ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("App open ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            LoadAppOpenAd();
            handler.Show_SmallBanner1();
            handler.Show_SmallBanner2();
            Debug.Log("App open ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);
            LoadAppOpenAd();
        };
    }
}
