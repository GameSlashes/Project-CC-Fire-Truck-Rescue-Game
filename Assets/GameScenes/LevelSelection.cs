using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class LevelSelection : MonoBehaviour
{
    public static LevelSelection instance;

    [Header("Scene Selection")]
    public Scenes PreviousScene;
    public Scenes NextScene;

    [Header("Settings")]
    public bool Locked;
    public int PlayableLevels = 6;

    [Header("UI Panels")]
    public GameObject LoadingScreen;
    public GameObject LevelsPanel;
    public GameObject ModeSelection;
    public GameObject LevelSelections;

    [Header("Mode Selection")]
    public GameObject mode3Locked;
    public GameObject mode4locked;

    private List<Button> LevelButtons = new List<Button>();
    AsyncOperation async = null;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        LoadingScreen.SetActive(false);
        CacheButtons();
        LevelsInit();
        checkMode();


    }

    void CacheButtons()
    {
        Button[] levelButtons = LevelsPanel.transform.GetComponentsInChildren<Button>();
        for (int i = 0; i < levelButtons.Length; i++)
        {
            LevelButtons.Add(levelButtons[i]);
        }
        LevelButtons = LevelButtons.OrderBy(x => Int32.Parse(x.gameObject.name)).ToList();
        for (int i = 0; i < LevelButtons.Count; i++)
        {
            int LevelIndex = i;
            LevelButtons[i].onClick.AddListener(() => PlayLevel(LevelIndex));
            if (SoundManager.instance)
                SoundManager.instance.onButtonClickSound(SoundManager.instance.buttonMainSound);
        }
    }

    void LevelsInit()
    {
        ModeSelection.SetActive(true);
        LevelSelections.SetActive(false);

        if (!Locked)
        {
            for (int i = 0; i < LevelButtons.Count; i++)
            {
                if (i < PlayableLevels)
                {
                    LevelButtons[i].interactable = true;
                    LevelButtons[i].transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    LevelButtons[i].interactable = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < LevelButtons.Count; i++)
            {
                LevelButtons[i].interactable = false;
                LevelButtons[i].transform.GetChild(1).gameObject.SetActive(true);
            }

            for (int i = 0; i < LevelButtons.Count; i++)
            {
                if (i < SaveData.Instance.Level && i < PlayableLevels)
                {
                    LevelButtons[i].interactable = true;
                    LevelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }

    public void PlayLevel(int level)
    {
        SaveData.Instance.CurrentLevel = level;
        LoadingScreen.SetActive(true);
        SaveData.instance.showAd();
        LoadScene.SceneName = NextScene.ToString();
    }

    public void BackBtn()
    {
        LoadingScreen.SetActive(true);
        SaveData.instance.showAd();
        LoadScene.SceneName = PreviousScene.ToString();
        //SceneManager.LoadScene(PreviousScene.ToString());
    }

    public void checkMode()
    {
        if (PlayerPrefs.GetInt("UnlockMode3") == 2)
            mode3Locked.SetActive(false);
        else
            mode3Locked.SetActive(true);

        if (PlayerPrefs.GetInt("UnlockMode4") == 2)
            mode4locked.SetActive(false);
        else
            mode4locked.SetActive(true);
    }

    public void unlockMode(int modeNumber)
    {
        if(modeNumber == 3)
        {
            mode3Locked.SetActive(false);
            PlayerPrefs.SetInt("UnlockMode3", 2);
        }
        else if(modeNumber == 4)
        {
            mode4locked.SetActive(false);
            PlayerPrefs.SetInt("UnlockMode4", 2);
        }
    }

    public void playMode(int modeID)
    {
        SaveData.instance.ModeID = modeID;

        if(modeID == 1)
        {
            ModeSelection.SetActive(false);
            LevelSelections.SetActive(true);
            PlayerPrefs.SetInt("Mode", 1);
        }
        else if(modeID == 2)
        {
            ModeSelection.SetActive(false);
            LoadingScreen.SetActive(true);
            PlayLevel(0);
            PlayerPrefs.SetInt("Mode", 2);
        }
        else if(modeID == 3)
        {
            if(PlayerPrefs.GetInt("UnlockMode3") == 2)
            {
                ModeSelection.SetActive(false);
                LoadingScreen.SetActive(true);
                PlayLevel(0);
                PlayerPrefs.SetInt("Mode", 2);
            }
            else
            {

            }
            
        }
        else if(modeID == 4)
        {
            if(PlayerPrefs.GetInt("UnlockMode4") == 2)
            {
                ModeSelection.SetActive(false);
                LoadingScreen.SetActive(true);
                PlayLevel(0);
                PlayerPrefs.SetInt("Mode", 4);
            }
            else
            {

            }
            
        }

        if (SoundManager.instance)
            SoundManager.instance.onButtonClickSound(SoundManager.instance.buttonMainSound);
    }




}
