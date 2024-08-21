using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

[System.Serializable]
public class Selection_CharacterElements
{
    [Header("UI Buttons")]
    public Button PlayBtn;
    public GameObject NextBtn;
    public GameObject PrevBtn;
    public Button buywithWatchVideo;
}

[System.Serializable]
public class CharacterAttributes
{
    public int playerID;
    public GameObject PlayerObject;
    [Header("Unlocking")]
    public bool Locked;
    public bool UnlockThroughWatchVideo;
    public bool UnlockThroughCoins;
    public int CoinsPrice;
    public string playerStats;
}

public class CharacterSelection : MonoBehaviour
{
    public static CharacterSelection instance;

    [Header("UI Elements")]
    public Selection_CharacterElements Selection_UI;

    [Header("Player Attributes")]
    public CharacterAttributes[] Players;


    [HideInInspector]public int current;

    

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        if (FindObjectOfType<Handler>())
        {
            FindObjectOfType<Handler>().Show_SmallBanner1();
            FindObjectOfType<Handler>().Show_SmallBanner2();
        }
        Time.timeScale = 1;
        AudioListener.pause = false;
        GetPlayerInfo();
        
    }


    void GetPlayerInfo()
    {
        checkPlayerPurchasing();

        for (int i = 0; i < Players.Length; i++)
        {
            if (i == current)
            {
                Players[i].PlayerObject.SetActive(true);
            }
            else
            if (i != current)
            {
                Players[i].PlayerObject.SetActive(false);
            }
        }

        if (Players[current].UnlockThroughCoins)
        {
            Selection_UI.buywithWatchVideo.gameObject.SetActive(false);

        }
        else if(Players[current].UnlockThroughWatchVideo)
        {
            Selection_UI.buywithWatchVideo.gameObject.SetActive(true);
        }
        else
        {
            Selection_UI.buywithWatchVideo.gameObject.SetActive(false);
        }



        if (Players[current].Locked)
        {
            Selection_UI.PlayBtn.gameObject.SetActive(false);
        }
        else
        if (!Players[current].Locked)
        {
            Selection_UI.PlayBtn.gameObject.SetActive(true);
            Selection_UI.buywithWatchVideo.gameObject.SetActive(false);
        }

        if (current == 0)
        {
            Selection_UI.PrevBtn.GetComponent<Button>().interactable = false;
            Selection_UI.NextBtn.GetComponent<Button>().interactable = true;
        }
        else
        if (current == Players.Length - 1)
        {
            Selection_UI.PrevBtn.GetComponent<Button>().interactable = true;
            Selection_UI.NextBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            Selection_UI.PrevBtn.GetComponent<Button>().interactable = true;
            Selection_UI.NextBtn.GetComponent<Button>().interactable = true;
        }
    }

    public void buyPlayer()
    {
        playBtnSound();

        if (FindObjectOfType<Handler>())
        {
            FindObjectOfType<Handler>().ShowRewardedAdsBoth(buyWithWatchVideo);
        }
    }

    public void checkPlayerPurchasing()
    {
        for (int i = 1; i < Players.Length; i++)
        {
            if (PlayerPrefs.GetString("UnlockedCharacter" + Players[i].playerID) == "Purchase")
            {
                Players[i].Locked = false;
            }
            else
            {
                Players[i].Locked = true;
            }
        }
    }

    public void Previous()
    {
        current--;
        GetPlayerInfo();
        playBtnSound();
    }

    public void Next()
    {
        current++;
        GetPlayerInfo();
        playBtnSound();
    }


    public void playBtnSound()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound);
        }
    }



    public void buyWithWatchVideo()
    {
        PlayerPrefs.SetString("UnlockedCharacter" + Players[current].playerID, "Purchase");
        Players[current].Locked = false;
        GetPlayerInfo();
        playBtnSound();
    }

    public void NextScen()
    {
        playBtnSound();
        SavepPlayerData.instance.finalPlayer = current;
        PlayerPrefs.SetString("fakeScene", "LevelSelection");
        SceneManager.LoadScene("FakeLoading");
    }
    public void back()
    {
        playBtnSound();
        PlayerPrefs.SetString("fakeScene", "PlayerSelection");
        SceneManager.LoadScene("FakeLoading");
    }
}
