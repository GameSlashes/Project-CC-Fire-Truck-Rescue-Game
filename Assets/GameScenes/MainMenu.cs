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
    public Text totalCoins;



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
