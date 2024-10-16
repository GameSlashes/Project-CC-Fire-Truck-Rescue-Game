﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SplashPanel : MonoBehaviour
{
    [Header("Scene Selection")]
    public Scenes NextScene;

    //public GameObject adsScript;

    public Image fillBar;

    bool oneTime;
    bool single;
    private void Awake()
    {
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
    }
    void Start()
    {
        fillBar.fillAmount = 0;
        Firebase.Analytics.FirebaseAnalytics.LogEvent("SplashScreen_Open");
    }

    public void Update()
    {
        if (fillBar.fillAmount < 1)
        {
            fillBar.fillAmount += 0.1f * Time.deltaTime;

            if (fillBar.fillAmount >= 0.85f)
            {
                if (oneTime == false)
                {
                    oneTime = true;
                    if (FindObjectOfType<Handler>())
                    {
                        FindObjectOfType<Handler>().Show_SmallBanner1();
                        FindObjectOfType<Handler>().Show_SmallBanner2();
                        PlayerPrefs.SetInt("loadInterstitialAD", 5);
                    }
                    if (AppOpenAdController.Instance)
                    {
                        AppOpenAdController.Instance.LoadAppOpenAd();
                    }

                }
            }
        }
        else
        {
            if (single == false)
            {

                //int state = PlayerPrefs.GetInt("playBtnState", 0);
                //if (state == 0)
                //{
                //    PlayerPrefs.SetInt("adShowMore", 5);
                //    SceneManager.LoadScene("Gameplay_1");
                //    PlayerPrefs.SetInt("playBtnState", 1);
                //}
                //else
                //{
                    SceneManager.LoadScene(NextScene.ToString());
                //}

                single = true;
            }

        }
    }
}
