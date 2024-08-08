using UnityEngine;

public class showInterstitial : MonoBehaviour
{
    public bool right;

    public void OnEnable()
    {
        if (FindObjectOfType<Handler>())
        {
            FindObjectOfType<Handler>().showWaitInterstitial();
            PlayerPrefs.SetInt("loadInterstitialAD", 5);
        }

        if (FindObjectOfType<TimerScriptAD>())
            FindObjectOfType<TimerScriptAD>().checkInterstitial();
    }

    public void requestInterstitial()
    {
        //if (FindObjectOfType<Handler>())
        //{
        //    FindObjectOfType<Handler>().LoadInterstitialAd(); 
        //}
    }

    public void ShowMedioumBannerad()
    {
        if (right)
        {
            if (FindObjectOfType<Handler>())
            {
                FindObjectOfType<Handler>().ShowMediumBanner(GoogleMobileAds.Api.AdPosition.BottomRight);
            }
        }
        else
        {
            if (FindObjectOfType<Handler>())
            {
                FindObjectOfType<Handler>().ShowMediumBanner(GoogleMobileAds.Api.AdPosition.BottomLeft);
            }
        }

    }
    public void HideMedioumBannerad()
    {
        if (FindObjectOfType<Handler>())
        {
            FindObjectOfType<Handler>().HideMediumBannerEvent(); 
        }
    }

}
