using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class fakeLoadingScreen : MonoBehaviour
{
    public Image fillBar;
    
    bool oneTime;
    bool single;

    public GameObject adIsLoading;


    public void Start()
    {
        fillBar.fillAmount = 0;
       
        if (FindObjectOfType<Handler>())
        {
            FindObjectOfType<Handler>().LoadInterstitialAd();
            FindObjectOfType<Handler>().ShowMediumBanner(GoogleMobileAds.Api.AdPosition.BottomRight);
        }

        if (PlayerPrefs.GetInt("adShowMore") == 5)
        {
            adIsLoading.SetActive(true);
        }
        else
        {
            adIsLoading.SetActive(false);
        }
    }

    public void Update()
    {
        if (fillBar.fillAmount < 1)
        {
            fillBar.fillAmount += 0.15f * Time.deltaTime;
            //fillBar.fillAmount += 0.1f * Time.deltaTime;

            if (fillBar.fillAmount >= 0.85f)
            {
                if (oneTime == false)
                {
                    if (FindObjectOfType<Handler>())
                    {
                        FindObjectOfType<Handler>().HideMediumBannerEvent();
                        if (PlayerPrefs.GetInt("adShowMore") == 5)
                        {
                            FindObjectOfType<Handler>().showWaitInterstitial();
                            PlayerPrefs.SetInt("loadInterstitialAD", 5);
                            PlayerPrefs.SetInt("adShowMore", 1);
                        }

                    }

                    if (FindObjectOfType<TimerScriptAD>())
                    {
                        FindObjectOfType<TimerScriptAD>().checkInterstitial();
                    }

                    oneTime = true;
                }
            }
        }
        else
        {
            if (single == false)
            {
                SceneManager.LoadScene(PlayerPrefs.GetString("fakeScene"));
                single = true;
            }
        }
    }
}
