using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
    public enum AdsLoadingStatus{
    NotLoaded,
    Loading,
    Loaded,
    NoInventory
}
public abstract class Handler : MonoBehaviour
{
    public bool EnableTestModeAds;
    public bool DisableDebugLogs = true;

    public delegate void RewardUserDelegate();

    public delegate void AfterLoading();

    public static AdsLoadingStatus rAdStatus = AdsLoadingStatus.NotLoaded;
    public static AdsLoadingStatus riAdStatus = AdsLoadingStatus.NotLoaded;

    public static AdsLoadingStatus iAdStatus = AdsLoadingStatus.NotLoaded;

    public static AdsLoadingStatus smallBannerStatus = AdsLoadingStatus.NotLoaded;

    public static AdsLoadingStatus small2ndBannerStatus = AdsLoadingStatus.NotLoaded;

    public static AdsLoadingStatus mediumBannerStatus = AdsLoadingStatus.NotLoaded;
    public abstract bool IsInterstitialAdReady();
    public abstract void ShowInterstitialAd();
    public abstract void showWaitInterstitial();
    public abstract void LoadInterstitialAd();


    public abstract bool IsSmallFirstBannerReady();
    public abstract void Load_SmallBanner1();

    public abstract void Show_SmallBanner1();
    public abstract void Hide_SmallBanner1Event();

    //2nd Banner
    public abstract bool IsSecondBannerReady();
    public abstract void Load_SmallBanner2();

    public abstract void Show_SmallBanner2();
    public abstract void Hide_SmallBanner2Event();

    public abstract bool IsMediumBannerReady();

    public abstract void LoadMediumBanner();
    public abstract void ShowMediumBanner(AdPosition pos);
    public abstract void HideMediumBannerEvent();

    public abstract bool IsRewardedAdReady();
    public abstract void LoadRewardedVideo();

    public abstract void ShowRewardedVideo(RewardUserDelegate _delegate);

    public abstract void LoadRewardedInterstitial();

    public abstract void ShowRewardedInterstitialAd(RewardUserDelegate _delegate);
    public abstract bool IsRewardedInterstitialAdReady();

    public GameObject Loading;

    int a = 0;

    public void ShowRewardedAdsBoth(RewardUserDelegate _delegate)
    {
        if (a == 0)
        {
            ShowRewardedVideo(_delegate);
            a = 1;
        }
        else if (a == 1)
        {
            ShowRewardedInterstitialAd(_delegate);
            a = 0;
        }
    }
    public void ShowRewardVideo()
    {
        ShowRewardedAdsBoth(GiveReward);
    }

    public void GiveReward()
    {
        Debug.Log("RewardGiven");
    }
}
