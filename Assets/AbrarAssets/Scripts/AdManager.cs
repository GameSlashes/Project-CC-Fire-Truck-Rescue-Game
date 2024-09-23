using UnityEngine;

public class AdManager : MonoBehaviour
{
    [SerializeField]
    private bool isRightPosition;
    [SerializeField]
    private bool shouldShowInterstitialOnEnable = true;  
    [SerializeField]
    private bool shouldMediumBanner = true;

    private Handler handler;
    private TimerScriptAD timerScriptAD;

    private void Awake()
    {
        // Cache references to avoid repeated calls to FindObjectOfType
        handler = FindObjectOfType<Handler>();
        timerScriptAD = FindObjectOfType<TimerScriptAD>();
    }

    private void OnEnable()
    {
        if (shouldShowInterstitialOnEnable)
        {
            ShowInterstitialAd();
        }

        UpdateBannerVisibility();
        if (shouldMediumBanner)
        {
            ShowMediumBannerAd();
        }
    }

    public void ShowInterstitialAd()
    {
        handler?.showWaitInterstitial();
        PlayerPrefs.SetInt("loadInterstitialAD", 5);

        timerScriptAD?.checkInterstitial();
    }

    public void ShowMediumBannerAd()
    {
        if (isRightPosition)
        {
            handler?.Hide_SmallBanner2Event();
            handler?.ShowMediumBanner(GoogleMobileAds.Api.AdPosition.BottomRight);
        }
        else
        {
            handler?.Hide_SmallBanner1Event();
            handler?.ShowMediumBanner(GoogleMobileAds.Api.AdPosition.BottomLeft);
        }
    }

    public void HideMediumBannerAd()
    {
        handler?.HideMediumBannerEvent();

        if (isRightPosition)
        {
            handler?.Show_SmallBanner2();
        }
        else
        {
            handler?.Show_SmallBanner1();
        }
    }

    private void UpdateBannerVisibility()
    {
        if (isRightPosition)
        {
            handler?.Hide_SmallBanner2Event();
        }
        else
        {
            handler?.Hide_SmallBanner1Event();
        }
    }
}
