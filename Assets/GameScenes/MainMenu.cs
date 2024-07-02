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
    public GameObject Shop;
    public Text totalCoins;

    [Header("Settings")]
    public GameObject soundOnBtn;
    public GameObject soundOffBtn;


    void Start()
    {
        Time.timeScale = 1;
        InitializeUI();
        checkSound();
        SoundManager.instance.playMainMenuSound();
    }

    public void showAds()
    {
        SaveData.instance.showAd();
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
        Shop.SetActive(false);
    }

    public void PlayBtn()
    {
        playBtnSound();
        showAds();
        SceneManager.LoadScene(NextScene.ToString());
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
        if (SoundManager.instance)
            SoundManager.instance.onButtonClickSound(SoundManager.instance.buttonMainSound);
    }


    public void soundOn()
    {
        AudioListener.volume = 1;
        soundOffBtn.SetActive(true);
        soundOnBtn.SetActive(false);
        PlayerPrefs.SetString("sound", "on");
        playBtnSound();
    }

    public void soundOff()
    {
        AudioListener.volume = 0;
        soundOffBtn.SetActive(false);
        soundOnBtn.SetActive(true);
        PlayerPrefs.SetString("sound", "off");
        playBtnSound();
    }

    public void checkSound()
    {
        if (!PlayerPrefs.HasKey("sound"))
        {
            soundOn();
        }
        else if (PlayerPrefs.GetString("sound") == "on")
        {
            soundOn();
        }
        else if (PlayerPrefs.GetString("sound") == "off")
        {
            soundOff();
        }
    }

}
