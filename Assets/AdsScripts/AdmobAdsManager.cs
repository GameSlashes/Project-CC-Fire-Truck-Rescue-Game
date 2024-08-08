
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

[Serializable]
public class ADMobID
{
    [Header("Unity ID")]
    public string UnityId;
    [Header("Unity Interstitial_ID")]
    public string Unity_Interstitial_ID;
    [Header("Unity RewardedVideo_ID")]
    public string Unity_RewardedVideo;
    [Header("AdMobAppID")]
    public string AdmobAPPID;
    [Header("Intersitial")]
    public string Intersitial;
    [Header("SmallBanner_1_Medium_Ecpm")]
    public string SmallBanner_1_Medium_Ecpm;
    [Header("Small_banners_1_Low_Ecpm")]
    public string Small_banners_1_Low_Ecpm;
    [Header("SmallBanner_2_Medium_Ecpm")]
    public string SmallBanner_2_Medium_Ecpm;
    [Header("Small_banners_2_Low_Ecpm")]
    public string Small_banners_2_Low_Ecpm;
    [Header("MediumBanner_Medium_Ecpm")]
    public string MediumBanner_Medium_Ecpm;
    [Header("MediumBanner_Low_Ecpm")]
    public string MediumBanner_Low_Ecpm;
    [Header("RewardedVideo")]
    public string RewardedVideo;
    [Header("RewardedInt")]
    public string RewardedInt;

}
public class AdmobAdsManager : Handler, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public AdPosition banner1;
    public AdPosition banner2;

    public ADMobID AndroidAdmob_ID = new ADMobID();
    public ADMobID IosAndroid_ID = new ADMobID();
    public ADMobID TestAdmob_ID = new ADMobID();
    [HideInInspector]
    public ADMobID ADMOB_ID = new ADMobID();

    public static bool isSmallBannerLoadedFirst = false;
    public static bool isSmallBannerLoadedSecond = false;
    public static bool isMediumBannerLoaded = false;
    bool isAdmobInitialized = false;



    #region IntersitialAds_Var
    [HideInInspector]
    public InterstitialAd Interstitial_High_Ecpm;

    public delegate void InterstitialUnity();
    public static event InterstitialUnity Int_Unity;

    public static bool Interstitial_HighEcpm = true, UnityAds = false;
    #endregion

    #region SmallBanner_Var
    [HideInInspector]
    public BannerView SmallBanner_L_Low_ECPM;

    [HideInInspector]
    public BannerView SmallBanner_L_Medium_Ecpm;

    public delegate void SmallBannerFirstMediumEcpm();
    public static event SmallBannerFirstMediumEcpm First_Small_Banner_Medium_Ecpm;
    public delegate void SmallFirstBannerLow();
    public static event SmallFirstBannerLow First_Small_Banner_Low_Ecpm;
    public static bool FirstBanner_Medium_Ecpm = true, FirstBanner_Low_Ecpm = false;


    /// <summary>
    /// 2nd Banner
    /// </summary>
    [HideInInspector]
    public BannerView SmallBanner_R_Low_ECPM;


    [HideInInspector]
    public BannerView SmallBanner_R_Medium_Ecpm;

    public delegate void SmallBanner2ndMediumEcpm();
    public static event SmallBanner2ndMediumEcpm Second_Small_banner_Medium_Ecpm;
    public delegate void SmallBannr2ndLowEcmp();
    public static event SmallBannr2ndLowEcmp Second_Small_banner_Low_Ecpm;
    public static bool SecondBanner_Medium_Ecpm = true, SecondBanner_Low_Ecpm = false;
    public static bool Logs;

    #endregion

    #region MediumBanner_Var

    [HideInInspector]
    public BannerView MediumBannerMediumEcpm;
    [HideInInspector]
    public BannerView MediumBannerLowEcpm;


    public delegate void MediumBannerMediumECPM();
    public static event MediumBannerMediumECPM MediumbannerMediumEcpm;
    public delegate void MediumBannerLowECPM();
    public static event MediumBannerLowECPM MediumbannerLowEcpm;
    public static bool MediumBanner_Medium_Ecpm = true, MediumBanner_Low_Ecpm = false;

    #endregion

    #region RewardedVideo_Var
    private static RewardUserDelegate NotifyReward;

    [HideInInspector]
    public RewardedAd rewardBasedVideo;

    public delegate void RewardVideoUnity();
    public static event RewardVideoUnity RewardVideo_Unity;
    public static bool RewardVideo_High_Ecpm = true, UnityRewarded = false;
    #endregion

    #region RewardedInterstitialAds

    [HideInInspector]
    public RewardedInterstitialAd rewardedInterstitialAd;
    [HideInInspector]
    public bool rewardedInterstitialHighECPMLoaded;

    #endregion

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DontDestroyOnLoad(this.gameObject);
        Logs = DisableDebugLogs;
        if (EnableTestModeAds)
        {
            ADMOB_ID = TestAdmob_ID;
        }
        else
        {
#if UNITY_ANDROID
            ADMOB_ID = AndroidAdmob_ID;
#elif UNITY_IOS
        ADMOB_ID = IosAndroid_ID;
          RequestConfiguration requestConfiguration = new RequestConfiguration.Builder()
       .SetSameAppKeyEnabled(false)
       .build();
        MobileAds.SetRequestConfiguration(requestConfiguration);
#endif
        }
    }
    private void Start()
    {
        InitAdmob();
        InitializeAds();
    }
    public void InitializeAds()
    {
        Advertisement.Initialize(ADMOB_ID.UnityId, EnableTestModeAds, this);
    }
    public void OnInitializationComplete()
    {
        Admob_LogHelper.LogGAEvent("unity_advertisement_initialized_done");
    }
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"unity_advertisement_initialization_failed: {error.ToString()} - {message}");
    }
    private void InitAdmob()
    {
        Admob_LogHelper.LogSender(AdmobEvents.Initializing);

        MobileAds.Initialize((initStatus) =>
        {
            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        break;
                    case AdapterState.Ready:
                        Admob_LogHelper.LogSender(AdmobEvents.Initialized);
                        MediationAdapterConsent(className);
                        break;
                }
            }
        });
#if UNITY_IOS
        MobileAds.SetiOSAppPauseOnBackground(true);    
#endif
    }
    void MediationAdapterConsent(string AdapterClassname)
    {
        if (AdapterClassname.Contains("ExampleClass"))
        {
            isAdmobInitialized = true;
            Load_SmallBanner1();
            Load_SmallBanner2();
            LoadMediumBanner();
            LoadRewardedInterstitial();
            LoadRewardedVideo();
        }
        if (AdapterClassname.Contains("MobileAds"))
        {
            isAdmobInitialized = true;
            Load_SmallBanner1();
            Load_SmallBanner2();
            LoadMediumBanner();
            LoadRewardedInterstitial();
            LoadRewardedVideo();
        }
    }


    #region IntersititialCodeBlock
    public override bool IsInterstitialAdReady()
    {
        if (this.Interstitial_High_Ecpm != null)
            return this.Interstitial_High_Ecpm.CanShowAd();
        else
            return false;
    }

    public override void showWaitInterstitial()
    {
        if (!PreferenceManager.GetAdsStatus() || !isAdmobInitialized)
        {
            return;
        }

        if (Interstitial_HighEcpm)
        {
            if (this.Interstitial_High_Ecpm != null)
            {
                if (this.Interstitial_High_Ecpm.CanShowAd())
                {

                    Loading.SetActive(true);
                }
            }
        }
        else if (UnityAds)
        {
            Loading.SetActive(true);
        }

    }

    public override void ShowInterstitialAd()
    {
        if (!PreferenceManager.GetAdsStatus() || !isAdmobInitialized)
        {
            return;
        }

        if (Interstitial_HighEcpm)
        {
            if (this.Interstitial_High_Ecpm != null)
            {
                if (this.Interstitial_High_Ecpm.CanShowAd())
                {
                    Loading.SetActive(false);

                    if (AppOpenAdController.Instance)
                        AppOpenAdController.Instance.AdShowing = true;

                    Admob_LogHelper.LogSender(AdmobEvents.Interstitial_WillDisplay_High_Ecpm);
                    this.Interstitial_High_Ecpm.Show();

                }
            }
        }
        else if (UnityAds)
        {
            Loading.SetActive(false);

            if (AppOpenAdController.Instance)
                AppOpenAdController.Instance.AdShowing = true;

            Admob_LogHelper.LogGAEvent("unity_interstitial_loaded");
            Advertisement.Show(ADMOB_ID.Unity_Interstitial_ID, this);
        }
    }
    public override void LoadInterstitialAd()
    {
        if (!isAdmobInitialized || IsInterstitialAdReady() || iAdStatus == AdsLoadingStatus.Loading || !PreferenceManager.GetAdsStatus())
        {
            return;
        }
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork | Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            if (Interstitial_HighEcpm)
            {
                Int_Unity += LoadInterstitialAd;
                Admob_LogHelper.LogSender(AdmobEvents.LoadInterstitial_High_Ecpm);
                var request = new AdRequest();
                iAdStatus = AdsLoadingStatus.Loading;

                InterstitialAd.Load(ADMOB_ID.Intersitial, request, (InterstitialAd ad, LoadAdError error) =>
                {
                    if (error != null)
                    {
                        Admob_LogHelper.LogGAEvent("Interstitial ad failed to load an ad with error : " + error);
                        return;
                    }

                    if (ad == null)
                    {
                        Admob_LogHelper.LogGAEvent("Unexpected error: Interstitial load event fired with null ad and null error.");
                        return;
                    }

                    Admob_LogHelper.LogGAEvent("Interstitial ad loaded with response : " + ad.GetResponseInfo());
                    this.Interstitial_High_Ecpm = ad;

                    BindIntersititialHighEcpmEvents();
                });
            }
            else if (UnityAds)
            {
                Admob_LogHelper.LogGAEvent("Load_Unity_Int");
                Advertisement.Load(ADMOB_ID.Unity_Interstitial_ID, this);
            }
        }
    }

    #endregion

    #region IntersititialEventCallBacks
    //HighEcpmEvents
    private void BindIntersititialHighEcpmEvents()
    {
        this.Interstitial_High_Ecpm.OnAdPaid += (AdValue adValue) =>
        {
            Admob_LogHelper.LogGAEvent(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        this.Interstitial_High_Ecpm.OnAdImpressionRecorded += () =>
        {
            Admob_LogHelper.LogGAEvent("Interstitial ad recorded an impression.");

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (Interstitial_HighEcpm)
                {
                    iAdStatus = AdsLoadingStatus.Loaded;

                    Admob_LogHelper.LogSender(AdmobEvents.Interstitial_Loaded_High_Ecpm);
                    Int_Unity -= LoadInterstitialAd;
                    Interstitial_HighEcpm = true;
                    UnityAds = false;
                }

            });
        };
        this.Interstitial_High_Ecpm.OnAdClicked += () =>
        {
            Admob_LogHelper.LogGAEvent("Interstitial ad was clicked.");
        };
        this.Interstitial_High_Ecpm.OnAdFullScreenContentOpened += () =>
        {
            Admob_LogHelper.LogGAEvent("Interstitial ad full screen content opened.");

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                iAdStatus = AdsLoadingStatus.NotLoaded;

                if (Interstitial_HighEcpm)
                {
                    Admob_LogHelper.LogSender(AdmobEvents.Interstitial_Displayed_High_Ecpm);
                    Int_Unity -= LoadInterstitialAd;
                }
            });
        };
        this.Interstitial_High_Ecpm.OnAdFullScreenContentClosed += () =>
        {
            Admob_LogHelper.LogGAEvent("Interstitial ad full screen content closed.");

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Admob_LogHelper.LogSender(AdmobEvents.Interstitial_Closed_High_Ecpm);
                iAdStatus = AdsLoadingStatus.NotLoaded;
                if (Interstitial_HighEcpm)
                {
                    this.Interstitial_High_Ecpm.Destroy();
                    Int_Unity -= LoadInterstitialAd;
                    Interstitial_HighEcpm = true;
                    UnityAds = false;
                }
            });
        };
        this.Interstitial_High_Ecpm.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Admob_LogHelper.LogGAEvent("Interstitial ad failed to open full screen content with error : "
                + error);

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (Interstitial_HighEcpm)
                {
                    iAdStatus = AdsLoadingStatus.NoInventory;
                    Admob_LogHelper.LogSender(AdmobEvents.Interstitial_NoInventory_High_Ecpm);
                    Interstitial_HighEcpm = false;
                    UnityAds = true;
                    if (Int_Unity != null)
                        Int_Unity();
                }
            });
        };
    }


    #endregion

    #region BannerCodeBlock
    public override bool IsSmallFirstBannerReady()
    {
        return isSmallBannerLoadedFirst;
    }
    public override void Load_SmallBanner1()
    {
        if (!PreferenceManager.GetAdsStatus() || IsSmallFirstBannerReady() || smallBannerStatus == AdsLoadingStatus.Loading)
        {
            return;
        }
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork | Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {

            if (FirstBanner_Medium_Ecpm)
            {
                this.SmallBanner_L_Medium_Ecpm = new BannerView(ADMOB_ID.SmallBanner_1_Medium_Ecpm, AdSize.Banner, banner1);

                First_Small_Banner_Low_Ecpm += Load_SmallBanner1;
                Logging.Log("FirstSmallBanner_M_Ecpm");
                BindSmallBannerFirstMediumEcpm();
                var request = new AdRequest();
                this.SmallBanner_L_Medium_Ecpm.LoadAd(request);
                this.SmallBanner_L_Medium_Ecpm.Hide();
            }
            else
            if (FirstBanner_Low_Ecpm)
            {
                this.SmallBanner_L_Low_ECPM = new BannerView(ADMOB_ID.Small_banners_1_Low_Ecpm, AdSize.Banner, banner1);

                Logging.Log("FirstSmallBanner_L_Ecpm");
                BindSmallBannerFirst();
                var request = new AdRequest();
                this.SmallBanner_L_Low_ECPM.LoadAd(request);
                this.SmallBanner_L_Low_ECPM.Hide();
            }
        }
    }
    public override void Hide_SmallBanner1Event()
    {
        if (this.SmallBanner_L_Medium_Ecpm != null)
        {
            this.SmallBanner_L_Medium_Ecpm.Hide();
            Logging.Log("Admob:smallBanner:Hide_M_Ecpm");
        }

        if (this.SmallBanner_L_Low_ECPM != null)
        {
            Logging.Log("Admob:smallBanner:Hide_L_Ecpm");
            this.SmallBanner_L_Low_ECPM.Hide();
        }
    }
    public void ShowBanner()
    {
        Show_SmallBanner1();
    }
    public override void Show_SmallBanner1()
    {
        Hide_SmallBanner1Event();

        try
        {
            if (!PreferenceManager.GetAdsStatus() || !isAdmobInitialized)
            {
                return;
            }


            if (FirstBanner_Medium_Ecpm)
            {
                if (SmallBanner_L_Medium_Ecpm != null)
                {
                    Logging.Log("GR >> FirstBanner_Medium_Ecpm_Show");
                    this.SmallBanner_L_Medium_Ecpm.Hide();

                    this.SmallBanner_L_Medium_Ecpm.Show();
                    this.SmallBanner_L_Medium_Ecpm.SetPosition(banner1);
                }
            }
            else if (FirstBanner_Low_Ecpm)
            {
                if (SmallBanner_L_Low_ECPM != null)
                {

                    this.SmallBanner_L_Low_ECPM.Hide();
                    Logging.Log("GR >> SmallBanner_First_ECPM_Show");
                    this.SmallBanner_L_Low_ECPM.Show();
                    this.SmallBanner_L_Low_ECPM.SetPosition(banner1);
                }
            }
            else
            {
                Load_SmallBanner1();
            }



        }
        catch (Exception error)
        {
            Logging.Log("Small Banner Error: " + error);
        }
    }
    private void BindSmallBannerFirst()
    {
        this.SmallBanner_L_Low_ECPM.OnBannerAdLoaded += () =>
        {
            Logging.Log("Banner view loaded an ad with response : "
                + this.SmallBanner_L_Low_ECPM.GetResponseInfo());

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (FirstBanner_Low_Ecpm)
                {
                    smallBannerStatus = AdsLoadingStatus.Loaded;
                    isSmallBannerLoadedFirst = true;
                    FirstBanner_Medium_Ecpm = false;
                    FirstBanner_Low_Ecpm = true;
                    Logging.Log("FirstSmallBanner_l_Loaded_Ecpm");
                }
            });
        };

        this.SmallBanner_L_Low_ECPM.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Logging.Log("Banner view failed to load an ad with error : " + error);

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                {
                    smallBannerStatus = AdsLoadingStatus.NoInventory;

                    Logging.Log("FirstSmallBanner_l_Fail_Ecpm");

                    isSmallBannerLoadedFirst = false;

                }
            });
        };

    }
    private void BindSmallBannerFirstMediumEcpm()
    {
        this.SmallBanner_L_Medium_Ecpm.OnBannerAdLoaded += () =>
        {
            Logging.Log("Banner view loaded an ad with response : "
                + this.SmallBanner_L_Medium_Ecpm.GetResponseInfo());

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (FirstBanner_Medium_Ecpm)
                {
                    smallBannerStatus = AdsLoadingStatus.Loaded;
                    First_Small_Banner_Low_Ecpm -= Load_SmallBanner1;
                    Logging.Log("FirstSmallBanner_M_Loaded_Ecpm");
                    isSmallBannerLoadedFirst = true;
                    FirstBanner_Medium_Ecpm = true;
                    FirstBanner_Low_Ecpm = false;
                }
            });
        };

        this.SmallBanner_L_Medium_Ecpm.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : " + error);

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (FirstBanner_Medium_Ecpm)
                {
                    smallBannerStatus = AdsLoadingStatus.NoInventory;
                    Logging.Log("FirstSmallBanner_M_Fail_Ecpm");
                    isSmallBannerLoadedFirst = false;
                    FirstBanner_Medium_Ecpm = false;
                    FirstBanner_Low_Ecpm = true;

                    if (First_Small_Banner_Low_Ecpm != null)
                        First_Small_Banner_Low_Ecpm();
                }
            });
        };

    }
    /// <summary>
    /// 2nd BannerCode
    /// </summary>
    private void BindSmallBannerSecondEcpm()
    {
        this.SmallBanner_R_Low_ECPM.OnBannerAdLoaded += () =>
        {
            Logging.Log("Banner view loaded an ad with response : "
                + this.SmallBanner_R_Low_ECPM.GetResponseInfo());

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (SecondBanner_Low_Ecpm)
                {

                    small2ndBannerStatus = AdsLoadingStatus.Loaded;
                    Second_Small_banner_Low_Ecpm -= Load_SmallBanner2;

                    isSmallBannerLoadedSecond = true;

                    SecondBanner_Medium_Ecpm = false;
                    SecondBanner_Low_Ecpm = true;
                    Logging.Log("2ndSmallBanner_l_Loaded_Ecpm");
                }

            });
        };

        this.SmallBanner_R_Low_ECPM.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Logging.Log("Banner view failed to load an ad with error : " + error);

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (SecondBanner_Low_Ecpm)
                {
                    small2ndBannerStatus = AdsLoadingStatus.NoInventory;
                    Logging.Log("2ndSmallBanner_l_Fail_Ecpm");
                    isSmallBannerLoadedSecond = false;
                }
            });
        };

    }
    private void BindSmallBannerSecondMediumEcpm()
    {
        this.SmallBanner_R_Medium_Ecpm.OnBannerAdLoaded += () =>
        {
            Logging.Log("Banner view loaded an ad with response : "
                + this.SmallBanner_R_Medium_Ecpm.GetResponseInfo());


            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (SecondBanner_Medium_Ecpm)
                {
                    small2ndBannerStatus = AdsLoadingStatus.Loaded;
                    Logging.Log("2ndSmallBanner_M_Loaded_Ecpm");
                    First_Small_Banner_Low_Ecpm -= Load_SmallBanner2;
                    isSmallBannerLoadedSecond = true;
                    SecondBanner_Medium_Ecpm = true;
                    SecondBanner_Low_Ecpm = false;

                }
            });
        };

        this.SmallBanner_R_Medium_Ecpm.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Logging.Log("Banner view failed to load an ad with error : " + error);

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (SecondBanner_Medium_Ecpm)
                {
                    small2ndBannerStatus = AdsLoadingStatus.NoInventory;


                    isSmallBannerLoadedSecond = false;

                    SecondBanner_Medium_Ecpm = false;
                    SecondBanner_Low_Ecpm = true;
                    Logging.Log("2ndSmallBanner_M_Failed_Ecpm");
                    if (Second_Small_banner_Low_Ecpm != null)
                        Second_Small_banner_Low_Ecpm();

                }
            });
        };

    }
    public override bool IsSecondBannerReady()
    {
        return isSmallBannerLoadedSecond;
    }
    public override void Load_SmallBanner2()
    {
        if (!PreferenceManager.GetAdsStatus() || IsSecondBannerReady() || small2ndBannerStatus == AdsLoadingStatus.Loading)
        {
            return;
        }
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork | Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            if (SecondBanner_Medium_Ecpm)
            {
                this.SmallBanner_R_Medium_Ecpm = new BannerView(ADMOB_ID.SmallBanner_2_Medium_Ecpm, AdSize.Banner, banner2);

                Second_Small_banner_Low_Ecpm += Load_SmallBanner2;
                Logging.Log("SecondSmallBanner_M_Ecpm");
                BindSmallBannerSecondMediumEcpm();
                var request = new AdRequest();
                this.SmallBanner_R_Medium_Ecpm.LoadAd(request);
                this.SmallBanner_R_Medium_Ecpm.Hide();
            }
            else
            if (SecondBanner_Low_Ecpm)
            {
                this.SmallBanner_R_Low_ECPM = new BannerView(ADMOB_ID.Small_banners_2_Low_Ecpm, AdSize.Banner, banner2);

                Logging.Log("SecondSmallBanner_L_Ecpm");
                BindSmallBannerSecondEcpm();

                var request = new AdRequest();

                this.SmallBanner_R_Low_ECPM.LoadAd(request);
                this.SmallBanner_R_Low_ECPM.Hide();

            }
        }
    }
    public override void Hide_SmallBanner2Event()
    {
        if (this.SmallBanner_R_Medium_Ecpm != null)
        {
            this.SmallBanner_R_Medium_Ecpm.Hide();
            Logging.Log("Admob:smallBanner:Hide_M_Ecpm");
        }

        if (this.SmallBanner_R_Low_ECPM != null)
        {
            Logging.Log("Admob:smallBanner:Hide_L_Ecpm");
            this.SmallBanner_R_Low_ECPM.Hide();
        }
    }
    public override void Show_SmallBanner2()
    {

        Hide_SmallBanner2Event();
        try
        {
            if (!PreferenceManager.GetAdsStatus() || !isAdmobInitialized)
            {
                return;
            }

            if (SecondBanner_Medium_Ecpm)
            {
                if (SmallBanner_R_Medium_Ecpm != null)
                {

                    this.SmallBanner_R_Medium_Ecpm.Hide();

                    this.SmallBanner_R_Medium_Ecpm.Show();
                    this.SmallBanner_R_Medium_Ecpm.SetPosition(banner2);
                    Logging.Log("SecondBanner_Medium__Ecpm_Show");
                }
            }
            else if (SecondBanner_Low_Ecpm)
            {
                if (SmallBanner_R_Low_ECPM != null)
                {

                    this.SmallBanner_R_Low_ECPM.Hide();

                    this.SmallBanner_R_Low_ECPM.Show();
                    this.SmallBanner_R_Low_ECPM.SetPosition(banner2);
                    Logging.Log("SecondBanner_Low_Ecpm_Show");
                }
            }
            else
            {
                Load_SmallBanner2();
            }

        }
        catch (Exception error)
        {
            Logging.Log("Small Banner Error: " + error);
        }
    }

    #endregion

    #region MediumBannerCodeBlocks
    public override bool IsMediumBannerReady()
    {
        return isMediumBannerLoaded;
    }

    public override void LoadMediumBanner()
    {
        if (!PreferenceManager.GetAdsStatus() || IsMediumBannerReady() || mediumBannerStatus == AdsLoadingStatus.Loading)
        {
            return;
        }
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork | Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            if (MediumBanner_Medium_Ecpm)
            {
                MediumbannerLowEcpm += LoadMediumBanner;
                Admob_LogHelper.LogSender(AdmobEvents.MediumBanner_Load_MediumEcpm);
                this.MediumBannerMediumEcpm = new BannerView(ADMOB_ID.MediumBanner_Medium_Ecpm, AdSize.MediumRectangle, AdPosition.BottomLeft);
                BindMediumBannerEvents_M_Ecpm();
                var request = new AdRequest();
                this.MediumBannerMediumEcpm.LoadAd(request);
                this.MediumBannerMediumEcpm.Hide();
            }
            else if (MediumBanner_Low_Ecpm)
            {
                Admob_LogHelper.LogSender(AdmobEvents.MediumBanner_Load_LowEcpm);
                this.MediumBannerLowEcpm = new BannerView(ADMOB_ID.MediumBanner_Low_Ecpm, AdSize.MediumRectangle, AdPosition.BottomLeft);
                BindMediumBannerEvents_L_Ecpm();
                var request = new AdRequest();
                this.MediumBannerLowEcpm.LoadAd(request);
                this.MediumBannerLowEcpm.Hide();
            }
        }

    }
    public override void ShowMediumBanner(AdPosition pos)
    {
        try
        {
            if (!PreferenceManager.GetAdsStatus() || !isAdmobInitialized)
            {
                return;
            }

            if (MediumBanner_Medium_Ecpm)
            {
                Admob_LogHelper.LogSender(AdmobEvents.MediumBanner_Show_MediumEcpm);
                if (MediumBannerMediumEcpm != null)
                {

                    this.MediumBannerMediumEcpm.Hide();
                    this.MediumBannerMediumEcpm.Show();
                    this.MediumBannerMediumEcpm.SetPosition(pos);
                }
            }
            else if (MediumBanner_Low_Ecpm)
            {
                Admob_LogHelper.LogSender(AdmobEvents.MediumBanner_Show_LowEcpm);
                if (MediumBannerLowEcpm != null)
                {

                    this.MediumBannerLowEcpm.Hide();
                    this.MediumBannerLowEcpm.Show();
                    this.MediumBannerLowEcpm.SetPosition(pos);
                }
            }
        }
        catch (Exception e)
        {

            Logging.Log("Medium Banner Error: " + e);
        }
    }
    public override void HideMediumBannerEvent()
    {

        if (this.MediumBannerMediumEcpm != null)
        {
            Logging.Log("Admob:mediumBanner:Hide_M_Ecpm");
            this.MediumBannerMediumEcpm.Hide();
        }

        if (this.MediumBannerLowEcpm != null)
        {
            Logging.Log("Admob:mediumBanner:Hide_M_Ecpm");
            this.MediumBannerLowEcpm.Hide();
        }
    }

    #endregion

    #region MediumBannerCallBack Handlers

    //MediumBanner2
    private void BindMediumBannerEvents_M_Ecpm()
    {
        this.MediumBannerMediumEcpm.OnBannerAdLoaded += () =>
        {
            Logging.Log("Banner view loaded an ad with response : "
                + this.MediumBannerMediumEcpm.GetResponseInfo());

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (MediumBanner_Medium_Ecpm)
                {
                    mediumBannerStatus = AdsLoadingStatus.Loaded;
                    Admob_LogHelper.LogSender(AdmobEvents.MediumBanner_Loaded_MediumEcpm);
                    MediumbannerLowEcpm -= LoadMediumBanner;
                    isMediumBannerLoaded = true;
                }
            });
        };

        this.MediumBannerMediumEcpm.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Logging.Log("Banner view failed to load an ad with error : " + error);

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (MediumBanner_Medium_Ecpm)
                {
                    mediumBannerStatus = AdsLoadingStatus.NotLoaded;
                    Admob_LogHelper.LogSender(AdmobEvents.MediumBanner_NoInventory_MediumEcpm);
                    MediumBanner_Medium_Ecpm = false;
                    MediumBanner_Low_Ecpm = true;
                    isMediumBannerLoaded = false;
                    if (MediumbannerLowEcpm != null)
                    {
                        MediumbannerLowEcpm();
                    }
                }
            });
        };

        this.MediumBannerMediumEcpm.OnAdPaid += (AdValue adValue) =>
        {
            Logging.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };

        this.MediumBannerMediumEcpm.OnAdImpressionRecorded += () =>
        {
            Logging.Log("Banner view recorded an impression.");
        };

        this.MediumBannerMediumEcpm.OnAdClicked += () =>
        {
            Logging.Log("Banner view was clicked.");
        };

        this.MediumBannerMediumEcpm.OnAdFullScreenContentOpened += () =>
        {
            Logging.Log("Banner view full screen content opened.");

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (MediumBanner_Medium_Ecpm)
                {

                    Admob_LogHelper.LogSender(AdmobEvents.MediumBanner_Displayed_MediumEcpm);
                    MediumbannerLowEcpm -= LoadMediumBanner;
                }
            });
        };

        this.MediumBannerMediumEcpm.OnAdFullScreenContentClosed += () =>
        {
            Logging.Log("Banner view full screen content closed.");

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                MediumbannerLowEcpm -= LoadMediumBanner;
            });
        };
    }

    //MediumBanner3
    private void BindMediumBannerEvents_L_Ecpm()
    {
        this.MediumBannerLowEcpm.OnBannerAdLoaded += () =>
        {
            Logging.Log("Banner view loaded an ad with response : "
                + this.MediumBannerLowEcpm.GetResponseInfo());

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (MediumBanner_Low_Ecpm)
                {
                    mediumBannerStatus = AdsLoadingStatus.Loaded;
                    Admob_LogHelper.LogSender(AdmobEvents.MediumBanner_Loaded_LowEcpm);
                    isMediumBannerLoaded = true;
                }
            });
        };

        this.MediumBannerLowEcpm.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Logging.Log("Banner view failed to load an ad with error : " + error);

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (MediumBanner_Low_Ecpm)
                {
                    mediumBannerStatus = AdsLoadingStatus.NotLoaded;
                    Admob_LogHelper.LogSender(AdmobEvents.MediumBanner_NoInventory_LowEcpm);
                    isMediumBannerLoaded = false;

                }

            });
        };

        this.MediumBannerLowEcpm.OnAdPaid += (AdValue adValue) =>
        {
            Logging.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };

        this.MediumBannerLowEcpm.OnAdImpressionRecorded += () =>
        {
            Logging.Log("Banner view recorded an impression.");
        };

        this.MediumBannerLowEcpm.OnAdClicked += () =>
        {
            Logging.Log("Banner view was clicked.");
        };

        this.MediumBannerLowEcpm.OnAdFullScreenContentOpened += () =>
        {
            Logging.Log("Banner view full screen content opened.");

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (MediumBanner_Low_Ecpm)
                {

                    Admob_LogHelper.LogSender(AdmobEvents.MediumBanner_Displayed_LowEcpm);


                }
            });
        };

        this.MediumBannerLowEcpm.OnAdFullScreenContentClosed += () =>
        {
            Logging.Log("Banner view full screen content closed.");

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (MediumBanner_Low_Ecpm)
                {
                    Logging.Log("GR >> Admob:mediumBanner:Closed");


                }
            });
        };
    }

    #endregion

    #region RewardedVideoCodeBlock
    public override void LoadRewardedVideo()
    {
        if (!isAdmobInitialized || IsRewardedAdReady() || rAdStatus == AdsLoadingStatus.Loading)
        {
            return;
        }
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork | Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            if (RewardVideo_High_Ecpm)
            {
                RewardVideo_Unity += LoadRewardedVideo;
                Admob_LogHelper.LogSender(AdmobEvents.LoadRewardedVideo_High_Ecpm);
                var request = new AdRequest();
                rAdStatus = AdsLoadingStatus.Loading;
                RewardedAd.Load(ADMOB_ID.RewardedVideo, request, (RewardedAd ad, LoadAdError error) =>
                {
                    if (error != null)
                    {
                        Logging.Log("Rewarded ad failed to load an ad with error : " + error);
                        return;
                    }

                    if (ad == null)
                    {
                        Logging.Log("Unexpected error: Rewarded load event fired with null ad and null error.");
                        return;
                    }

                    Logging.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
                    this.rewardBasedVideo = ad;
                    BindRewardedEvents_H_Ecpm();
                });
            }
            else if (UnityRewarded)
            {
                Advertisement.Load(ADMOB_ID.Unity_RewardedVideo);
            }
        }
    }
    public override bool IsRewardedAdReady()
    {
        if (this.rewardBasedVideo != null)
            return this.rewardBasedVideo.CanShowAd();
        else
            return false;
    }
    public override void ShowRewardedVideo(RewardUserDelegate _delegate)
    {
        if (RewardVideo_High_Ecpm)
        {
            NotifyReward = _delegate;
            Admob_LogHelper.LogSender(AdmobEvents.ShowRewardedVideo_High_Ecpm);

            if (this.rewardBasedVideo.CanShowAd())
            {
                if (AppOpenAdController.Instance)
                    AppOpenAdController.Instance.AdShowing = true;

                Admob_LogHelper.LogSender(AdmobEvents.RewardedVideo_WillDisplay_High_Ecpm);

                this.rewardBasedVideo.Show((Reward reward) =>
                {
                    Logging.Log(String.Format("Rewarded ad granted a reward: {0} {1}",
                                            reward.Amount,
                                            reward.Type));
                });
            }
        }
        else if (UnityRewarded)
        {
            if (AppOpenAdController.Instance)
                AppOpenAdController.Instance.AdShowing = true;

            NotifyReward = _delegate;
            Advertisement.Show(ADMOB_ID.Unity_RewardedVideo, this);
        }
    }

    #endregion

    #region RewardedVideoEvents
    //***** Rewarded Events *****//
    private void BindRewardedEvents_H_Ecpm()
    {
        rewardBasedVideo.OnAdPaid += (AdValue adValue) =>
        {

        };

        rewardBasedVideo.OnAdImpressionRecorded += () =>
        {
            Logging.Log("Rewarded ad recorded an impression.");

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (RewardVideo_High_Ecpm)
                {
                    RewardVideo_Unity -= LoadRewardedVideo;
                    rAdStatus = AdsLoadingStatus.Loaded;
                    UnityRewarded = false;
                    Admob_LogHelper.LogSender(AdmobEvents.RewardedVideo_Loaded_High_Ecpm);

                }
            });
        };

        rewardBasedVideo.OnAdClicked += () =>
        {
            Logging.Log("Rewarded ad was clicked.");
        };

        rewardBasedVideo.OnAdFullScreenContentOpened += () =>
        {
            Logging.Log("Rewarded ad full screen content opened.");

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (RewardVideo_High_Ecpm)
                {
                    RewardVideo_Unity -= LoadRewardedVideo;
                    rAdStatus = AdsLoadingStatus.NotLoaded;

                    if (NotifyReward != null)
                        NotifyReward();

                    Admob_LogHelper.LogSender(AdmobEvents.RewardedVideo_Displayed_High_Ecpm);
                }
            });
        };

        rewardBasedVideo.OnAdFullScreenContentClosed += () =>
        {
            Logging.Log("Rewarded ad full screen content closed.");

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (RewardVideo_High_Ecpm)
                {
                    RewardVideo_Unity -= LoadRewardedVideo;
                    rAdStatus = AdsLoadingStatus.NotLoaded;
                    Logging.Log("GR >> Admob:rad:Closed_H_Ecpm");
                    Admob_LogHelper.LogSender(AdmobEvents.RewardedVideo_Closed_High_Ecpm);
                    LoadRewardedVideo();
                }
            });
        };

        rewardBasedVideo.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Logging.Log("Rewarded ad failed to open full screen content with error : " + error);

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (RewardVideo_High_Ecpm)
                {
                    rAdStatus = AdsLoadingStatus.NoInventory;
                    Admob_LogHelper.LogSender(AdmobEvents.RewardedVideo_NoInventory_High_Ecpm);
                    RewardVideo_High_Ecpm = false;

                    UnityRewarded = true;
                    if (RewardVideo_Unity != null)
                    {
                        RewardVideo_Unity();
                    }
                }
            });
        };
    }

    #endregion

    #region RewardedInterstial
    public override void LoadRewardedInterstitial()
    {
        if (!isAdmobInitialized || IsRewardedInterstitialAdReady() || riAdStatus == AdsLoadingStatus.Loading)
        {
            return;
        }

        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            Admob_LogHelper.LogSender(AdmobEvents.LoadRewardedInterstitialAd_H_ECPM);
            var request = new AdRequest();
            riAdStatus = AdsLoadingStatus.Loading;

            RewardedInterstitialAd.Load(ADMOB_ID.RewardedInt, request, (RewardedInterstitialAd ad, LoadAdError error) =>
            {
                if (error != null)
                {
                    Logging.Log("Rewarded interstitial ad failed to load an ad with error : " + error);
                    return;
                }

                if (ad == null)
                {
                    Logging.Log("Unexpected error: Rewarded interstitial load event fired with null ad and null error.");
                    return;
                }

                Logging.Log("Rewarded interstitial ad loaded with response : " + ad.GetResponseInfo());
                rewardedInterstitialAd = ad;
                RegisterEventHandlers(ad);
            });
        }
    }

    public override void ShowRewardedInterstitialAd(RewardUserDelegate _delegate)
    {
        NotifyReward = _delegate;
        Admob_LogHelper.LogSender(AdmobEvents.ShowRewardedInterstitialAd_H_ECPM);

        if (this.rewardedInterstitialAd != null)
        {
            if (rewardedInterstitialHighECPMLoaded)
            {
                if (AppOpenAdController.Instance)
                    AppOpenAdController.Instance.AdShowing = true;

                this.rewardedInterstitialAd.Show(userEarnedRewardCallback);
            }
        }
    }

    private void userEarnedRewardCallback(Reward reward)
    {

    }


    public override bool IsRewardedInterstitialAdReady()
    {
        if (this.rewardedInterstitialAd != null)
        {
            if (rewardedInterstitialHighECPMLoaded)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region RewardedInterstitialCallbackHandler

    ///////// Rewarded Interstitial High ECPM Callbacks //////////
    protected void RegisterEventHandlers(RewardedInterstitialAd ad)
    {
        rewardedInterstitialHighECPMLoaded = true;

        ad.OnAdPaid += (AdValue adValue) =>
        {

        };
        ad.OnAdImpressionRecorded += () =>
        {
            Logging.Log("Rewarded interstitial ad recorded an impression.");
        };
        ad.OnAdClicked += () =>
        {
            Logging.Log("Rewarded interstitial ad was clicked.");
        };
        ad.OnAdFullScreenContentOpened += () =>
        {
            Logging.Log("Rewarded interstitial ad has presented.");
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                {
                    riAdStatus = AdsLoadingStatus.NotLoaded;

                    Admob_LogHelper.LogSender(AdmobEvents.RewardedInterstitialAdDisplayed_H_ECPM);
                }
            });
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            Logging.Log("Rewarded interstitial ad has dismissed presentation.");
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                {
                    rewardedInterstitialHighECPMLoaded = false;
                    riAdStatus = AdsLoadingStatus.NotLoaded;
                    Admob_LogHelper.LogSender(AdmobEvents.RewardedInterstitialAdClosed_H_ECPM);
                    NotifyReward();
                    LoadRewardedInterstitial();
                }
            });
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {

                {
                    riAdStatus = AdsLoadingStatus.NotLoaded;
                    Logging.Log("Admob:riad:FailedToShow:HCPM");
                }
            });
        };
    }

    #endregion

    #region UnityCallBack
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Enter1");
        // Optionally execute code if the Ad Unit successfully loads content.
        if (adUnitId == ADMOB_ID.Unity_Interstitial_ID)
        {
            iAdStatus = AdsLoadingStatus.Loaded;
            UnityAds = true;
            Interstitial_HighEcpm = false;


        }
        else if (adUnitId == ADMOB_ID.Unity_RewardedVideo)
        {
            rAdStatus = AdsLoadingStatus.Loaded;
            RewardVideo_High_Ecpm = false;
            UnityRewarded = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        if (adUnitId == ADMOB_ID.Unity_Interstitial_ID)
        {
            iAdStatus = AdsLoadingStatus.Loaded;
            UnityAds = true;
            Interstitial_HighEcpm = false;
        }
        else if (adUnitId == ADMOB_ID.Unity_RewardedVideo)
        {
            rAdStatus = AdsLoadingStatus.Loaded;
            RewardVideo_High_Ecpm = false;
            UnityRewarded = true;
            Debug.Log("Ad_Failed");
        }
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        if (adUnitId == ADMOB_ID.Unity_Interstitial_ID)
        {
            iAdStatus = AdsLoadingStatus.Loaded;
            UnityAds = false;
            Interstitial_HighEcpm = true;


        }
        else if (adUnitId == ADMOB_ID.Unity_RewardedVideo)
        {
            rAdStatus = AdsLoadingStatus.Loaded;
            RewardVideo_High_Ecpm = true;

            UnityRewarded = false;

        }
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {


        //  ADmobInterstial = true;
        if (adUnitId == ADMOB_ID.Unity_Interstitial_ID)
        {
            iAdStatus = AdsLoadingStatus.NotLoaded;
            Interstitial_HighEcpm = true;

            UnityAds = false;
            Debug.Log("Ad_completed");
        }
        else

        if (adUnitId.Equals(ADMOB_ID.Unity_RewardedVideo) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            if (adUnitId == ADMOB_ID.Unity_RewardedVideo)
            {
                RewardVideo_High_Ecpm = true;

                UnityRewarded = false;
                NotifyReward();
                Debug.Log("Ad_completed");
            }
            // Load another ad:
            Advertisement.Load(ADMOB_ID.Unity_RewardedVideo, this);
        }
    }

    #endregion

}
