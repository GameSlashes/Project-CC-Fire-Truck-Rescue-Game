using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static RTC_CarController;

public class RewardTimer : MonoBehaviour
{
    public static RewardTimer Instance;

    public Canvas rewardPanel;
    public GameObject congratesPanel;

    public GameObject[] ClaimBtns, readyReward, Claimed;
    public Text befor2x, after2x, grandTotal;

    public float timeRemaining;
    public Text timeText;

    DateTime currentDate;
    DateTime LastClaimed;
    DateTime oldDate;
    private int Day = 0;
    private bool collected;
    public bool isPlayerSelection;

    private void Awake()
    {
        Instance = this;

        if (Instance == null)
        {
            Instance = FindObjectOfType<RewardTimer>();
        }

        for(int i = 0; i < readyReward.Length; i++)
        {
            readyReward[i].SetActive(false);
        }
    }
    private int After2x;
    public void CallitOnEnable()
    {
        befor2x.text = currentReward.ToString();
        After2x = currentReward * 2;
        //after2x.text = After2x.ToString();
    }
    void Start()
    {
        timeRemaining = timeRemaining - PlayerPrefs.GetFloat("Timepassed");
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("LastClaimed")))
        {
            oldDate = System.DateTime.Parse(PlayerPrefs.GetString("LastClaimed"));
        }
        Check();
        ClaimedRewardDays();
        activeCalimedOnes();
        UpdateProgressBar();
    }
    private void UpdateProgressBar()
    {
        int claimedDays = 0;
        for (int i = 0; i < 7; i++)
        {
            if (PlayerPrefs.GetInt("Day" + i) == 1)
            {
                claimedDays++;
            }
            else
            {

            }
        }

        float fillAmount = (float)claimedDays / 6f;
    }

    public void back()
    {
        //HideMedioumBannerad();
        if (FindObjectOfType<Handler>())
        {
            FindObjectOfType<Handler>().Show_SmallBanner1();

            FindObjectOfType<Handler>().showWaitInterstitial();
            PlayerPrefs.SetInt("loadInterstitialAD", 5);
        }

        if (FindObjectOfType<TimerScriptAD>())
            FindObjectOfType<TimerScriptAD>().checkInterstitial();

        rewardPanel.enabled = (false);

    }

    public void come()
    {
        PlayerPrefs.SetInt("loadInterstitialAD", 5);
        if (FindObjectOfType<TimerScriptAD>())
            FindObjectOfType<TimerScriptAD>().checkInterstitial();

        rewardPanel.enabled = (true);
        //ShowMedioumBannerad();
    }

    private void activeCalimedOnes()
    {
        for (int i = 0; i < 7; i++)
        {
            if (PlayerPrefs.GetInt("Day" + i) == 1)
            {
                Claimed[i].SetActive(true);
            }
        }
    }

    private void Check()
    {

        currentDate = System.DateTime.Now;
        System.TimeSpan TimeDiff = currentDate.Subtract(oldDate);

        timeRemaining = (float)(86400 - TimeDiff.TotalSeconds);

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }

        if (PlayerPrefs.GetInt("Day0") != 1)
        {
            if (TimeDiff.Days >= 1 || TimeDiff.Hours >= 24/*TimeDiff.Seconds >= 5*/)
            {
                ClaimBtns[PlayerPrefs.GetInt("lastClaimedDay")].SetActive(true);
                readyReward[PlayerPrefs.GetInt("lastClaimedDay")].SetActive(true);
                UpdateProgressBar();
            }

        }
        else
        {
            if (TimeDiff.Days >= 1 || TimeDiff.Hours >= 24/*TimeDiff.Seconds >= 5*/)
            {
                timeRemaining = 0;
                if (PlayerPrefs.GetInt("clicked") != 1)
                {

                    PlayerPrefs.SetInt("lastClaimedDay", PlayerPrefs.GetInt("lastClaimedDay") + 1);
                    if (PlayerPrefs.GetInt("lastClaimedDay") < 6)
                    {
                        ClaimBtns[PlayerPrefs.GetInt("lastClaimedDay")].SetActive(true);
                        readyReward[PlayerPrefs.GetInt("lastClaimedDay")].SetActive(true);
                    }
                    else
                    if (PlayerPrefs.GetInt("lastClaimedDay") == 6)
                    {
                        ClaimBtns[PlayerPrefs.GetInt("lastClaimedDay")].SetActive(true);
                        readyReward[PlayerPrefs.GetInt("lastClaimedDay")].SetActive(true);
                    }
                    UpdateProgressBar();
                    PlayerPrefs.SetInt("clicked", 1);
                }
            }
            else
            {

            }
        }
    }
    private void ClaimedRewardDays()
    {
        currentDate = System.DateTime.Now;
        System.TimeSpan TimeDiff = currentDate.Subtract(oldDate);
        if (PlayerPrefs.GetInt("lastClaimedDay") < 6)
        {
            if (TimeDiff.Days >= 1 || TimeDiff.Hours >= 24/*TimeDiff.Seconds >= 5*/)
            {

                if (PlayerPrefs.GetInt("lastClaimedDay") < 6)
                {
                    ClaimBtns[PlayerPrefs.GetInt("lastClaimedDay")].SetActive(true);
                    readyReward[PlayerPrefs.GetInt("lastClaimedDay")].SetActive(true);
                    if (!isPlayerSelection)
                    {
                        rewardPanel.enabled = (true);
                        timeText.enabled = false;
                        ShowMedioumBannerad();
                    }
                }
            }
        }

    }
    public void ShowMedioumBannerad()
    {
        //if (FindObjectOfType<Handler>())
        //{
        //    FindObjectOfType<Handler>().Hide_SmallBanner1Event();
        //    FindObjectOfType<Handler>().ShowMediumBanner(GoogleMobileAds.Api.AdPosition.BottomLeft);
        //}
    }
    public void HideMedioumBannerad()
    {
        //if (FindObjectOfType<Handler>())
        //{
        //    FindObjectOfType<Handler>().Show_SmallBanner1();
        //    FindObjectOfType<Handler>().HideMediumBannerEvent();
        //}
    }
    private void FixedUpdate()
    {
        Check();
    }

    private void ClaimedCheck(int x)
    {
        oldDate = System.DateTime.Now;
        ClaimBtns[x].SetActive(false);
    }


    private void DisplayTime(float timeToDisplay)
    {
        float hours = Mathf.FloorToInt((timeToDisplay / 3600) % 48);
        float minutes = Mathf.FloorToInt((timeToDisplay / 60) % 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        string time = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        timeText.text = time;
    }

    public static int coinReward = 0;
    public int currentReward = coinReward;


    public void ClaimReward(int i)
    {
        PlayerPrefs.SetInt("Day" + i, 1);
        PlayerPrefs.SetInt("Day" + i, 1);
        readyReward[i].SetActive(false);

        Claimed[i].SetActive(true);

        PlayerPrefs.SetInt("clicked", 0);
        LastClaimed = System.DateTime.Now;
        PlayerPrefs.SetString("LastClaimed", LastClaimed.ToString());
        timeText.enabled = true;
        timeRemaining = 10;

        if (PlayerPrefs.GetInt("lastClaimedDay") >= 6)
        {
            PlayerPrefs.SetInt("lastClaimedDay", 0);
            for (int j = 0; j <= 6; j++)
            {
                PlayerPrefs.SetInt("Day" + j, 0);
            }
        }
        else
        {
            PlayerPrefs.SetInt("lastClaimedDay", i);
        }

        ClaimedCheck(i);

        congratesPanel.SetActive(true);

        switch (i)
        {
            case 0:
                currentReward = 100;
                break;
            case 1:
                currentReward = 200;
                break;
            case 2:
                currentReward = 300;
                break;
            case 3:
                currentReward = 400;
                break;
            case 4:
                currentReward = 500;
                break;
            case 5:
                currentReward = 600;
                break;
            case 6:
                currentReward = 700;
                break;
        }

        UpdateProgressBar();
        befor2x.text = currentReward.ToString();
        SaveData.instance.Coins += currentReward;
        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(1f);
        congratesPanel.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void DoubleReward()
    {
        Invoke("delay1", 1f);
        if (FindObjectOfType<Handler>())
            FindObjectOfType<Handler>().ShowRewardedAdsBoth(DoubleTheReward);
    }
    public void DoubleTheReward()
    {
        currentReward *= 2;
        SaveData.instance.Coins += currentReward / 2;
    }
    public void delay1()
    {
        congratesPanel.transform.GetChild(1).gameObject.SetActive(true);
        congratesPanel.transform.GetChild(0).gameObject.SetActive(false);
        Invoke("delay2", 1.5f);
    }
    public void delay2()
    {
        congratesPanel.SetActive(false);
    }
    public void NotWantDoubleReward()
    {
        congratesPanel.SetActive(false);
    }
    public void ShowAd()
    {
        if (FindObjectOfType<Handler>())
        {
            FindObjectOfType<Handler>().showWaitInterstitial();
            PlayerPrefs.SetInt("loadInterstitialAD", 5);
        }

        if (FindObjectOfType<TimerScriptAD>())
            FindObjectOfType<TimerScriptAD>().checkInterstitial();
    }
    public void LoadAd()
    {

    }
    public void claimBtnSound()
    {
        //if (SoundManager.instance)
        //    SoundManager.instance.playBtnSound();
    }
    public void Exitbtnsound()
    {
        //if (SoundManager.instance)
        //    SoundManager.instance.playBtnSound();
    }

}
