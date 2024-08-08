using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PromoAdsRemove : MonoBehaviour
{
    private static int tempCountForPromotionRateUs = 3;
    private int showCount = 3;
    public GameObject ReviewPanel, RateUs;
    public bool isReview;

    public void SetCountToMax()
    {
        tempCountForPromotionRateUs = showCount;
    }

    void OnEnable()
    {

        if (isReview)
        {
            if (PlayerPrefs.GetInt("RateUsStatus") == 0)
            {
                if (ReviewPanel)
                {
                    ReviewPanel.SetActive(true);
                    Time.timeScale = 0.3f;
                }
            }

            //if (PrefsManager.GetCurrentLevel()==2 || PrefsManager.GetCurrentLevel()==27)
            {
                //RateUs.SetActive(true);
            }
        }

    }


}
