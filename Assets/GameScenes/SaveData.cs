using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public enum Scenes
{
    Splash,
    MainMenu,
    PlayerSelection,
    LevelSelection,
    Gameplay
}

[System.Serializable]
public class Game_Dialogues
{
    public GameObject levelCompleteText;
    public GameObject LevelComplete;

    public GameObject levelFailedText;
    public GameObject LevelFailed;

    public Text levelCompleteGem;
    public Text levelCompleteReward;

    public GameObject gameCompleteRewardCoinsObj;
    public GameObject gameCompleteRewardGemsObj;

    public GameObject doubleRewardBtn;

    public GameObject PauseMenu;
    public GameObject LoadingScreen;
    public GameObject Timer;
    public Text Timer_txt;

    public GameObject objectivePanel;
    public Text objectiveText;

    public GameObject Pipe;
    public GameObject WaterExting;
    public GameObject WaterGun;

}

[System.Serializable]
public class Level_Data
{
    public GameObject LevelObject;
    [Header("Player Spawn")]
    public Transform SpawnPoint;
    [Header("Tasks")]
    public string Objectives;
    [Tooltip("Level Time will not be considered if this field is unchecked.")]
    [Header("Level Time")]
    public bool isTimeBased;
    [Range(0, 60)]
    public int Minutes;
    [Range(10, 60)]
    public int Seconds;
    public bool isRewardBase;
    public int coinReward;
    public int gemReward;
}

[System.Serializable]
public class TyresModel
{
    public string name;
    public GameObject[] Models;
}

public class SaveData 
{
    public static SaveData instance = new SaveData();
    public static SaveData Instance => instance;
    public static event Action allCashUpdate;
    public static event Action allGemsUpdate;

    public int CurrentLevel = 1;
    public int CurrentPlayer = 0;

    public int Coins
    {
        get
        {
            return PlayerPrefs.GetInt("coins");
        }
        set
        {
            PlayerPrefs.SetInt("coins", value);
            if (allCashUpdate != null)
            {
                allCashUpdate();
            }
        }
    }

    public int Gems
    {
        get
        {
            return PlayerPrefs.GetInt("gems");
        }
        set
        {
            PlayerPrefs.SetInt("gems", value);
            if (allGemsUpdate != null)
            {
                allGemsUpdate();
            }
        }
    }

    public int checkAd
    {
        get
        {
            return PlayerPrefs.GetInt("removeads");

        }
        set
        {
            PlayerPrefs.SetInt("removeads", value);
            //AdCalls.instance.RemoveBanner();
        }
    }


    public int finalPlayer
    {
        get
        {
            return PlayerPrefs.GetInt("selectedPlayer", 0);
        }
        set
        {
            PlayerPrefs.SetInt("selectedPlayer", value);
        }
    }

    public int Level
    {
        get
        {
            return PlayerPrefs.GetInt("Level", 1);
        }
        set
        {
            PlayerPrefs.SetInt("Level", value);
        }
    }

    public int ModeID
    {
        get
        {
            return PlayerPrefs.GetInt("playMode", 1);
        }
        set
        {
            PlayerPrefs.SetInt("playMode", value);
        }
    }

    public void showAd()
    {
        //var handler = FindObjectOfType<Handler>();
        //handler?.showWaitInterstitial();
        //PlayerPrefs.SetInt("loadInterstitialAD", 5);
    }

   
}
