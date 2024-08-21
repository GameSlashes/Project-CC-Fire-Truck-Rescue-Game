using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadInter : MonoBehaviour
{
    bool check;
    public float count;
    public bool intLoaded;

    public void Update()
    {
        if (PlayerPrefs.GetInt("InterstitialAdLoadDelay") == 5)
        {
            if (check == false)
            {
                count = count + 1 * Time.deltaTime;

                if (count >= 3.3f)
                {
                    intLoaded = true;

                    if (FindObjectOfType<Handler>())
                        FindObjectOfType<Handler>().LoadInterstitialAd();


                    count = 0;
                    check = true;
                    
                    PlayerPrefs.SetInt("InterstitialAdLoadDelay", 0);
                }
            }
        }
        else
        {
            check = false;
        }

    }

}
