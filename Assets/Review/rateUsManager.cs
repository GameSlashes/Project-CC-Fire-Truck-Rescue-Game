using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rateUsManager : MonoBehaviour
{
    public static rateUsManager instance;
    public GameObject rateUsObject;

    public void Start()
    {
        if (PlayerPrefs.GetInt("RateUsStatus") == 0)
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void activeRateUs()
    {
        if (PlayerPrefs.GetInt("RateUsStatus") == 0)
        {
            rateUsObject.SetActive(true);
            Time.timeScale = 0.3f;
            AudioListener.volume = 0;
        }
    }
}
