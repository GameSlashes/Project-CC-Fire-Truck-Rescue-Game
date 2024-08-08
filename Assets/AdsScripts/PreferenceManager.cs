using UnityEngine;

public class PreferenceManager
{


    const string RemoveAds = "RemoveAds";

    public static bool GetAdsStatus()
    {
       
        return (PlayerPrefs.GetInt(RemoveAds, 0) == 0);
    }

    public static void SetAdsStatus(int value)
    {
        PlayerPrefs.SetInt(RemoveAds, value);
    }



}
