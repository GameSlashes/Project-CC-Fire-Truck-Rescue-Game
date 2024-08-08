
using UnityEngine;


public static class Logging
{
   // [System.Diagnostics.Conditional("ENABLE_LOG")]
    static public void Log(object message)
    {
       if(!AdmobAdsManager.Logs)
        UnityEngine.Debug.Log(message);

    }
}
public class Admob_LogHelper : MonoBehaviour
{
    public static void LogSender(AdmobEvents Log)
    {
        switch (Log)
        {
            case AdmobEvents.Initializing:

                LogGAEvent("AdmobInitializing");
                break;
            case AdmobEvents.Initialized:

                LogGAEvent("AdmobInitialized");
                break;
            case AdmobEvents.LoadInterstitial_High_Ecpm:
                LogGAEvent("Admob_iAd_LoadRequest_H_Ecpm");
                break;
            case AdmobEvents.LoadInterstitial_Medium_Ecpm:
                LogGAEvent("Admob_iAd_LoadRequest_M_Ecpm");
                break;
            case AdmobEvents.LoadIntersitiatial_Low_Ecpm:
                LogGAEvent("Admob_iAd_LoadRequest_L_Ecpm");
                break;
            case AdmobEvents.ShowInterstitial_High_Ecpm:
                LogGAEvent("Admob_iAd_ShowCall_H_Ecpm");
                break;
            case AdmobEvents.ShowInterstitial_Medium_Ecpm:
                LogGAEvent("Admob_iAd_ShowCall_M_Ecpm");
                break;
            case AdmobEvents.ShowIntersititial_Low_Ecpm:
                LogGAEvent("Admob_iAd_ShowCall_L_Ecpm");
                break;
            case AdmobEvents.Interstitial_WillDisplay_High_Ecpm:
                LogGAEvent("Admob_iAd_WillDisplay_H_Ecpm");
                break;
            case AdmobEvents.Interstitial_WillDisplay_Medium_Ecpm:
                LogGAEvent("Admob_iAd_WillDisplay_M_Ecpm");
                break;
            case AdmobEvents.Interstitial_WillDisplay_Low_Ecpm:
                LogGAEvent("Admob_iAd_WillDisplay_L_Ecpm");
                break;
            case AdmobEvents.Interstitial_Displayed_High_Ecpm:
                LogGAEvent("Admob_iAd_Displayed_H_Ecpm");
                break;
            case AdmobEvents.Interstilial_Displayed_Medium_Ecpm:
                LogGAEvent("Admob_iAd_Displayed_M_Ecpm");
                break;
            case AdmobEvents.Interstital_Displayed_Low_Ecpm:
                LogGAEvent("Admob_iAd_Displayed_L_Ecpm");
                break;
            case AdmobEvents.Interstitial_NoInventory_High_Ecpm:
                LogGAEvent("Admob_iAd_NoInventery_H_Ecpm");
                break;
            case AdmobEvents.Interstilial_NoInventory_Medium_Ecpm:
                LogGAEvent("Admob_iAd_NoInventery_M_Ecpm");
                break;
            case AdmobEvents.Interstitial_NoInventory_Low_Ecpm:
                LogGAEvent("Admob_iAd_NoInventery_L_Ecpm");
                break;
            case AdmobEvents.Interstitial_Closed_High_Ecpm:
                LogGAEvent("Admob_iAd_Closed_H_Ecpm");
                break;
            case AdmobEvents.Interstitial_Closed_Medium_Ecpm:
                LogGAEvent("Admob_iAd_Closed_M_Ecpm");
                break;
            case AdmobEvents.Interstitial_Closed_Low_Ecpm:
                LogGAEvent("Admob_iAd_Closed_L_Ecpm");
                break;
            case AdmobEvents.Interstitial_Loaded_High_Ecpm:
                LogGAEvent("Admob_iAd_Loaded_H_Ecpm");
                break;
            case AdmobEvents.Interstitial_Loaded_Medium_Ecpm:
                LogGAEvent("Admob_iAd_Loaded_M_Ecpm");

                break;
            case AdmobEvents.Interstitial_Loaded_Low_Ecpm:
                LogGAEvent("Admob_iAd_Loaded_L_Ecpm");

                break;
            case AdmobEvents.SmallBanner_Load_HighEcpm:
                LogGAEvent("Admob_AB_LoadRequest_H_Ecpm");
                break;
            case AdmobEvents.SmallBanner_Load_MediumEcpm:
                LogGAEvent("Admob_AB_LoadRequest_M_Ecpm");
                break;
            case AdmobEvents.SmallBanner_Load_LowEcpm:
                LogGAEvent("Admob_AB_LoadRequest_L_Ecpm");
                break;
            case AdmobEvents.SmallBanner_Show_HighEcpm:
                LogGAEvent("Admob_AB_ShowCall_H_Ecpm");
                break;
            case AdmobEvents.SmallBanner_Show_MediumEcpm:
                LogGAEvent("Admob_AB_ShowCall_M_Ecpm");
                break;
            case AdmobEvents.SmallBanner_Show_LowEcpm:
                LogGAEvent("Admob_AB_ShowCall_L_Ecpm");
                break;
            case AdmobEvents.SmallBanner_Loaded_HighEcpm:
                LogGAEvent("Admob_AB_Loaded_H_Ecpm");
                break;
            case AdmobEvents.SmallBannner_Loaded_MediumEcpm:
                LogGAEvent("Admob_AB_Loaded_M_Ecpm");
                break;
            case AdmobEvents.SmallBanner_Loaded_LowEcpm:
                LogGAEvent("Admob_AB_Loaded_L_Ecpm");
                break;
            case AdmobEvents.SmallBanner_NoInventory_HighEcpm:
                LogGAEvent("Admob_AB_NoInventory_H_Ecpm");
                break;
            case AdmobEvents.SmallBanner_NoInventory_MediumEcpm:
                LogGAEvent("Admob_AB_NoInventory_M_Ecpm");
                break;
            case AdmobEvents.SmallBanner_NoInventory_LowEcpm:
                LogGAEvent("Admob_AB_NoInventory_L_Ecpm");
                break;
            case AdmobEvents.SmallBanner_Displayed_HighEcpm:
                LogGAEvent("Admob_AB_Displayed_H_Ecpm");
                break;
            case AdmobEvents.SmallBanner_Displayed_MediumEcpm:
                LogGAEvent("Admob_AB_Displayed_M_Ecpm");
                break;
            case AdmobEvents.SmallBanner_Displayed_LowEcpm:
                LogGAEvent("Admob_AB_Displayed_L_Ecpm");
                break;
            case AdmobEvents.MediumBanner_Load_HighEcpm:
                LogGAEvent("Admob_MB_LoadRequest_H_Ecpm");
                break;
            case AdmobEvents.MediumBanner_Load_MediumEcpm:
                LogGAEvent("Admob_MB_LoadRequest_M_Ecpm");
                break;
            case AdmobEvents.MediumBanner_Load_LowEcpm:
                LogGAEvent("Admob_MB_LoadRequest_L_Ecpm");
                break;
            case AdmobEvents.MediumBanner_Show_HighEcpm:
                LogGAEvent("Admob_MB_ShowCall_H_Ecpm");
                break;
            case AdmobEvents.MediumBanner_Show_MediumEcpm:
                LogGAEvent("Admob_MB_ShowCall_M_Ecpm");
                break;
            case AdmobEvents.MediumBanner_Show_LowEcpm:
                LogGAEvent("Admob_MB_ShowCall_L_Ecpm");
                break;
            case AdmobEvents.MediumBanner_Loaded_HighEcpm:
                LogGAEvent("Admob_MB_Loaded_H_Ecpm");
                break;
            case AdmobEvents.MediumBanner_Loaded_MediumEcpm:
                LogGAEvent("Admob_MB_Loaded_M_Ecpm");
                break;
            case AdmobEvents.MediumBanner_Loaded_LowEcpm:
                LogGAEvent("Admob_MB_Loaded_L_Ecpm");
                break;
            case AdmobEvents.MediumBanner_NoInventory_HighEcpm:
                LogGAEvent("Admob_MB_NoInventory_H_Ecpm");
                break;
            case AdmobEvents.MediumBanner_NoInventory_MediumEcpm:
                LogGAEvent("Admob_MB_NoInventory_M_Ecpm");
                break;
            case AdmobEvents.MediumBanner_NoInventory_LowEcpm:
                LogGAEvent("Admob_MB_NoInventory_L_Ecpm");
                break;
            case AdmobEvents.MediumBanner_Displayed_HighEcpm:
                LogGAEvent("Admob_MB_Displayed_H_Ecpm");
                break;
            case AdmobEvents.MediumBanner_Displayed_MediumEcpm:
                LogGAEvent("Admob_MB_Displayed_M_Ecpm");
                break;
            case AdmobEvents.MediumBanner_Displayed_LowEcpm:
                LogGAEvent("Admob_MB_Displayed_L_Ecpm");
                break;
            case AdmobEvents.LoadRewardedVideo_High_Ecpm:
                LogGAEvent("Admob_rAd_LoadRequest_H_Ecpm");
                break;
            case AdmobEvents.LoadRewardedVideo_Medium_Ecpm:
                LogGAEvent("Admob_rAd_LoadRequest_M_Ecpm");
                break;
            case AdmobEvents.LoadRewardedVideo_Low_Ecpm:
                LogGAEvent("Admob_rAd_LoadRequest_L_Ecpm");
                break;
            case AdmobEvents.ShowRewardedVideo_High_Ecpm:
                LogGAEvent("Admob_rAd_ShowCall_H_Ecpm");
                break;
            case AdmobEvents.ShowRewardedVideo_Medium_Ecpm:
                LogGAEvent("Admob_rAd_ShowCall_M_Ecpm");
                break;
            case AdmobEvents.ShowRewardedVideo_Low_Ecpm:
                LogGAEvent("Admob_rAd_ShowCall_L_Ecpm");
                break;
            case AdmobEvents.RewardedVideo_WillDisplay_High_Ecpm:
                LogGAEvent("Admob_rAd_WillDisplay_H_Ecpm");
                break;
            case AdmobEvents.RewardedVideo_WillDisplay_Medium_Ecpm:
                LogGAEvent("Admob_rAd_WillDisplay_M_Ecpm");
                break;
            case AdmobEvents.RewardedVideo_WillDisplay_Low_Ecpm:
                LogGAEvent("Admob_rAd_WillDisplay_L_Ecpm");
                break;
            case AdmobEvents.RewardedVideo_Displayed_High_Ecpm:
                LogGAEvent("Admob_rAd_Displayed_H_Ecpm");
                break;
            case AdmobEvents.RewardedVideo_Displayed_Medium_Ecpm:
                LogGAEvent("Admob_rAd_Displayed_M_Ecpm");
                break;
            case AdmobEvents.RewardedVideo_Displayed_Low_Ecpm:
                LogGAEvent("Admob_rAd_Displayed_L_Ecpm");
                break;
            case AdmobEvents.RewardedVideo_NoInventory_High_Ecpm:
                LogGAEvent("Admob_rAd_NoInventery_H_Ecpm");
                break;
            case AdmobEvents.RewardedVideo_NoInventory_Medium_Ecpm:
                LogGAEvent("Admob_rAd_NoInventery_M_Ecpm");
                break;
            case AdmobEvents.RewardedVideo_NoInventory_Low_Ecpm:
                LogGAEvent("Admob_rAd_NoInventery_L_Ecpm");
                break;
            case AdmobEvents.RewardedVideo_Closed_High_Ecpm:
                LogGAEvent("Admob_rAd_Closed_H_Ecpm");
                break;
            case AdmobEvents.RewardedVideo_Closed_Medium_Ecpm:
                LogGAEvent("Admob_rAd_Closed_M_Ecpm");
                break;
            case AdmobEvents.RewardedVideo_Closed_Low_Ecpm:
                LogGAEvent("Admob_rAd_Closed_L_Ecpm");
                break;
            case AdmobEvents.RewardedVideo_Loaded_High_Ecpm:
                LogGAEvent("Admob_rAd_Loaded_H_Ecpm");
                break;
            case AdmobEvents.RewardedVideo_Loaded_Medium_Ecpm:
                LogGAEvent("Admob_rAd_Loaded_M_Ecpm");

                break;
            case AdmobEvents.RewardedVideo_Loaded_Low_Ecpm:
                LogGAEvent("Admob_rAd_Loaded_L_Ecpm");

                break;

            case AdmobEvents.LoadRewardedInterstitialAd_H_ECPM:
                LogGAEvent("Admob_riAd_LoadRequest_H_Ecpm");
                break;
            case AdmobEvents.LoadRewardedInterstitialAd_M_ECPM:
                LogGAEvent("Admob_riAd_LoadRequest_M_Ecpm");
                break;
            case AdmobEvents.LoadRewardedInterstitialAd_L_ECPM:
                LogGAEvent("Admob_riAd_LoadRequest_L_Ecpm");
                break;
            case AdmobEvents.ShowRewardedInterstitialAd_H_ECPM:
                LogGAEvent("Admob_riAd_ShowCall_H_Ecpm");
                break;
            case AdmobEvents.ShowRewardedInterstitialAd_M_ECPM:
                LogGAEvent("Admob_riAd_ShowCall_M_Ecpm");
                break;
            case AdmobEvents.ShowRewardedInterstitialAd_L_ECPM:
                LogGAEvent("Admob_riAd_ShowCall_L_Ecpm");
                break;
           
            case AdmobEvents.RewardedInterstitialAdDisplayed_H_ECPM:
                LogGAEvent("Admob_riAd_Displayed_H_Ecpm");
                break;
            case AdmobEvents.RewardedInterstitialAdDisplayed_M_ECPM:
                LogGAEvent("Admob_riAd_Displayed_M_Ecpm");
                break;
            case AdmobEvents.RewardedInterstitialAdDisplayed_L_ECPM:
                LogGAEvent("Admob_riAd_Displayed_L_Ecpm");
                break;
            case AdmobEvents.RewardedInterstitialNoInventory_H_ECPM:
                LogGAEvent("Admob_riAd_NoInventery_H_Ecpm");
                break;
            case AdmobEvents.RewardedInterstitialNoInventory_M_ECPM:
                LogGAEvent("Admob_riAd_NoInventery_M_Ecpm");
                break;
            case AdmobEvents.RewardedInterstitialNoInventory_L_ECPM:
                LogGAEvent("Admob_riAd_NoInventery_L_Ecpm");
                break;
            case AdmobEvents.RewardedInterstitialAdClosed_H_ECPM:
                LogGAEvent("Admob_riAd_Closed_H_Ecpm");
                break;
            case AdmobEvents.RewardedInterstitialAdClosed_M_ECPM:
                LogGAEvent("Admob_riAd_Closed_M_Ecpm");
                break;
            case AdmobEvents.RewardedInterstitialAdClosed_L_ECPM:
                LogGAEvent("Admob_riAd_Closed_L_Ecpm");
                break;
           

               
            default:
                break;
        }
    }


    public static void LogGAEvent(string log)
    {
        Logging.Log("GR >> " + log);
        Firebase.Analytics.FirebaseAnalytics.LogEvent(log);
        //FB.LogAppEvent(eventName);
    }

    public static void MissionOrLevelStartedEventLog(string GameMode, int LevelNumber)
    {
        Logging.Log("GR >> LevelStarted_" + GameMode + "_Level_" + LevelNumber.ToString());
       
        Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelStarted_" + GameMode + "_Level_" + LevelNumber.ToString());
      
    }
    public static void MissionOrLevelFailEventLog(string GameMode, int LevelNumber)
    {
        Logging.Log("GR >> LevelFail_" + GameMode + "_Level_Number_" + LevelNumber.ToString());
      
        Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelFail_" + GameMode + "_Level_" + LevelNumber.ToString());
       
    }

    public static void MissionOrLevelCompletedEventLog(string GameMode, int LevelNumber)
    {
        Logging.Log("GR >> LevelComplete_" + GameMode + "_Level_" + LevelNumber);
        
        Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelComplete_" + GameMode + "_Level_" + LevelNumber);

     
    }

}
