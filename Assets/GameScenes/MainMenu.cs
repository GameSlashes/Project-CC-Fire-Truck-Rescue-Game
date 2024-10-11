using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Selection")]
    public Scenes NextScene;

    [Header("UI Panels")]
    public GameObject SettingDialogue;
    public GameObject ExitDialogue;
    //public GameObject Shop;
    public GameObject spinWheeler;
    public GameObject dailyReward;
    public GameObject mainMenu;
    public Text totalCoins;
    public GameObject playerProfiler;
    public GameObject mainScene;



    void Start()
    {
        Time.timeScale = 1;
        InitializeUI();
        if (FindObjectOfType<Handler>())
        {
            FindObjectOfType<Handler>().Show_SmallBanner1();
            FindObjectOfType<Handler>().Show_SmallBanner2();
        }
        Firebase.Analytics.FirebaseAnalytics.LogEvent("MainMenu_Open");
    }
    public void ToggleSpinWheeler()
    {
        if (spinWheeler.activeSelf)
        {
            if (FindObjectOfType<Handler>())
            {
                FindObjectOfType<Handler>().showWaitInterstitial();
                PlayerPrefs.SetInt("loadInterstitialAD", 5);
                PlayerPrefs.SetInt("adShowMore", 1);
            }
            spinWheeler.SetActive(false);
            mainMenu.SetActive(true);
            Firebase.Analytics.FirebaseAnalytics.LogEvent("SpinWheeler_Close");
        }
        else
        {
            spinWheeler.SetActive(true);
            mainMenu.SetActive(false);
            Firebase.Analytics.FirebaseAnalytics.LogEvent("SpinWheeler_Open");
        }
    }
    public void DailyRewardOpen()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("DailyReward_Open");
        Canvas dailyRewardCanvas = dailyReward.GetComponent<Canvas>();
        dailyRewardCanvas.enabled = true;
        mainScene.SetActive(false);
    }   
    public void ProfilerOpen()
    {

        if (!PlayerPrefs.HasKey("HasOpenedPlayerProfiler"))
        {
            if (FindObjectOfType<Handler>())
            {
                FindObjectOfType<Handler>().Hide_SmallBanner1Event();
                FindObjectOfType<Handler>().ShowMediumBanner(GoogleMobileAds.Api.AdPosition.BottomLeft);
            }
            playerProfiler.SetActive(true);
            Firebase.Analytics.FirebaseAnalytics.LogEvent("Profiler_Open");
        }
        else
        {
            playerProfiler.SetActive(false);
        }

    }  
    public void ProfilerClose()
    {
        if (FindObjectOfType<Handler>())
        {
            FindObjectOfType<Handler>().Show_SmallBanner1();
            FindObjectOfType<Handler>().HideMediumBannerEvent();
        }

        playerProfiler.SetActive(false);
        Firebase.Analytics.FirebaseAnalytics.LogEvent("Profiler_Close");
    }   
    public void DailyRewardClose()
    {
        Canvas dailyRewardCanvas = dailyReward.GetComponent<Canvas>();
        dailyRewardCanvas.enabled = false;
        mainScene.SetActive(true);
        ProfilerOpen();
        Firebase.Analytics.FirebaseAnalytics.LogEvent("DailyReward_Close");
    }


    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                ExitDialogue.SetActive(true);
            }
        }
    }

    void InitializeUI()
    {
        SettingDialogue.SetActive(false);
        ExitDialogue.SetActive(false);
        //Shop.SetActive(false);
    }

    public void PlayBtn()
    {
        playBtnSound();
        PlayerPrefs.SetString("fakeScene", "PlayerSelection");
        SceneManager.LoadScene("FakeLoading");
    }

    public void RemoveAds()
    {
        playBtnSound();
        SaveData.instance.checkAd = 1;
    }

    public void openLink(string link)
    {
        playBtnSound();
        Application.OpenURL(link);  
    }

    public void DialogueOpen(GameObject dialogue)
    {
        playBtnSound();
        dialogue.SetActive(true);
        Firebase.Analytics.FirebaseAnalytics.LogEvent("SettingsPanel_Open");
    }

    public void Exit()
    {
        playBtnSound();
        Application.Quit();
    }

    public void UnlockEverything()
    {
        SaveData.instance.checkAd = 1;
    }

    public void playBtnSound()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound);
        }
    }


}
