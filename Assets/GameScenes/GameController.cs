using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    
    [Header("--------------------Scene Selection--------------------")]
    public Scenes PreviousScene;
    public Scenes MainMenu;
    [Header("--------------------Players-----------------------------")]
    public GameObject[] players;
    [HideInInspector]
    public GameObject myPlayers;
    [Header("--------------------Levels-----------------------------")]
    public int playableLevels;
    public Level_Data[] levels;
    [Header("--------------------Game Dialogues--------------------")]
    public Game_Dialogues Game_Elements;
    [Header("--------------------Events--------------------")]
    public UnityEvent LevelCompleteEvent;
    public UnityEvent LevelFailedEvent;
    public UnityEvent LevelStartEvent;

    int minutes;
    int seconds;
    string time;

    public bool isTimerEnabled;
    public bool TimerPaused = false;
    public float LevelTime = 0.0f;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        ActivePlayer();
        //ActiveLevel();
        //activeLevelObjective();
        //LevelStartEvent.Invoke();
        //checkModeStats();

        //if (isTimerEnabled)
        //    InvokeRepeating("GameTimer", 0, 1);

        //if (FindObjectOfType<Handler>())
        //{
        //    FindObjectOfType<Handler>().Show_SmallBanner1();
        //    FindObjectOfType<Handler>().Show_SmallBanner2();
        //}
        SoundManager.instance.PlayGamePlaySound();
    }

    public void checkModeStats()
    {
        if(PlayerPrefs.GetInt("Mode") == 1)
        {

        }
        else if (PlayerPrefs.GetInt("Mode") == 2)
        {

        }
        else if (PlayerPrefs.GetInt("Mode") == 3)
        {

        }
        else if (PlayerPrefs.GetInt("Mode") == 4)
        {

        }
    }


    void activeLevelObjective()
    {
        if(levels[SaveData.instance.CurrentLevel].Objectives != "")
        {
            Game_Elements.objectivePanel.SetActive(true);
            Game_Elements.objectiveText.text = levels[SaveData.instance.CurrentLevel].Objectives;
        }
    }

    void ActiveLevel()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (i == SaveData.instance.CurrentLevel)
            {
                levels[i].LevelObject.SetActive(true);
            }
            else
            {
                Destroy(levels[i].LevelObject);
            }
        }

        if (levels[SaveData.instance.CurrentLevel].isTimeBased)
        {
            isTimerEnabled = true;
            Game_Elements.Timer.SetActive(true);
        }
        else
        {
            isTimerEnabled = false;
            Game_Elements.Timer.SetActive(false);
        }

        LevelTime = (levels[SaveData.instance.CurrentLevel].Minutes * 60) + levels[SaveData.instance.CurrentLevel].Seconds;
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    void GameTimer()
    {
        if (!TimerPaused)
        {
            if (LevelTime >= 0.0f)
                LevelTime -= 1;

            minutes = ((int)LevelTime / 60);
            seconds = ((int)LevelTime % 60);
            time = minutes.ToString("00") + ":" + seconds.ToString("00");
            Game_Elements.Timer_txt.text = time;

            if (LevelTime <= 15.0f && LevelTime > 0.0f)
            {
                Game_Elements.Timer_txt.color = Color.red;
            }
            else
            if (LevelTime == 0.0f)
            {
                Game_Elements.levelFailedText.SetActive(true);
                LevelFailedEvent.Invoke();
                StartCoroutine(failedDialogue());
            }
        }
    }

    IEnumerator failedDialogue()
    {
        yield return new WaitForSeconds(1f);
        SaveData.instance.showAd();
        Time.timeScale = 0;
        Game_Elements.levelFailedText.SetActive(false);
        Game_Elements.LevelFailed.SetActive(true);
    }

    void ActivePlayer()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (i == SaveData.instance.finalPlayer)
            {
                players[i].SetActive(true);
                myPlayers= players[i];
                MissionManager.Instance.myCar = myPlayers;
                //SetPlayerPosition(players[i], levels[SaveData.instance.CurrentLevel].SpawnPoint);
            }
            else
            {
                Destroy(players[i]);
            }
        }
    }

    void SetPlayerPosition(GameObject Player, Transform Position)
    {
        Player.transform.position = Position.position;
        Player.transform.rotation = Position.rotation;
    }

    void checkLevelReward()
    {
        if (levels[SaveData.instance.CurrentLevel].isRewardBase)
        {
            if(levels[SaveData.instance.CurrentLevel].coinReward > 0)
            {
                Game_Elements.levelCompleteReward.text = levels[SaveData.instance.CurrentLevel].coinReward.ToString();
                SaveData.instance.Coins += levels[SaveData.instance.CurrentLevel].coinReward;
            }
            else
            {
                Game_Elements.gameCompleteRewardCoinsObj.SetActive(false);
            }

            if(levels[SaveData.instance.CurrentLevel].gemReward > 0)
            {
                if (Game_Elements.levelCompleteGem)
                {
                    Game_Elements.levelCompleteGem.text = levels[SaveData.instance.CurrentLevel].gemReward.ToString();
                    SaveData.instance.Gems += levels[SaveData.instance.CurrentLevel].gemReward;
                }
            }
            else
            {
                Game_Elements.gameCompleteRewardGemsObj.SetActive(false);
            }

        }
        else
        {
            Game_Elements.gameCompleteRewardCoinsObj.SetActive(false);
            Game_Elements.gameCompleteRewardGemsObj.SetActive(false);
            Game_Elements.doubleRewardBtn.SetActive(false);
        }
    }

    public void levelCompleted()
    {
        checkLevelReward();

        if (SaveData.instance.Level == SaveData.instance.CurrentLevel + 1 && SaveData.instance.Level <= playableLevels)
        {
            SaveData.instance.Level += 1;
        }
        Game_Elements.levelCompleteText.SetActive(true);
        LevelCompleteEvent.Invoke();
        StartCoroutine(completeDialogue());
    }
    IEnumerator completeDialogue()
    {
        yield return new WaitForSeconds(1f);
        SaveData.instance.showAd();
        Time.timeScale = 0;
        Game_Elements.levelCompleteText.SetActive(false);
        Game_Elements.LevelComplete.SetActive(true);
    }

    public void NextBtn()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound);
        }

        if (SaveData.instance.CurrentLevel < playableLevels - 1 && SaveData.instance.Level < playableLevels)
            SaveData.instance.CurrentLevel += 1;
        else
            //SaveData.instance.CurrentLevel = Random.Range(0, playableLevels - 1);
        
        Game_Elements.LoadingScreen.SetActive(true);
        LoadScene.SceneName = SceneManager.GetActiveScene().name;
    }

    public void mainMenu()
    {
        showAd();
        Time.timeScale = 1;
        PlayerPrefs.SetInt("adShowMore", 5);
        PlayerPrefs.SetString("fakeScene", "MainMenu");
        SceneManager.LoadScene("FakeLoading");
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound);
        }

    }

    public void restart()
    {
        showAd();
        Time.timeScale = 1;
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound);
        }

        PlayerPrefs.SetInt("adShowMore", 5);
        PlayerPrefs.SetString("fakeScene", "Gameplay");
        SceneManager.LoadScene("FakeLoading");
    }

    public void gamePause()
    {
        Time.timeScale = 0;
        Game_Elements.PauseMenu.SetActive(true);
    }

    public void resume()
    {
        showAd();
        Time.timeScale = 1;
        Game_Elements.PauseMenu.SetActive(false);
    }

    public void doubleRewardBtn()
    {
        //if (AdCalls.instance)
        //{
        //    AdCalls.instance.RewardVideo("2xReward");
        //}
    }

    public void doubleTheReward()
    {
        levels[SaveData.instance.CurrentLevel].coinReward *= 2;
        levels[SaveData.instance.CurrentLevel].gemReward *= 2;
        Game_Elements.levelCompleteReward.text = levels[SaveData.instance.CurrentLevel].coinReward.ToString();
        SaveData.instance.Coins += levels[SaveData.instance.CurrentLevel].coinReward / 2;

        Game_Elements.levelCompleteGem.text = levels[SaveData.instance.CurrentLevel].gemReward.ToString();
        SaveData.instance.Gems += levels[SaveData.instance.CurrentLevel].gemReward / 2;

        Game_Elements.doubleRewardBtn.SetActive(false);
    }
    public void showAd()
    {
        //var handler = FindObjectOfType<Handler>();
        //handler?.showWaitInterstitial();
        //PlayerPrefs.SetInt("loadInterstitialAD", 5);
    }
}

