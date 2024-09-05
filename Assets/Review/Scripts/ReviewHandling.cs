using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using Google.Play.Review;
using UnityEngine;
using UnityEngine.UI;

public class ReviewHandling : MonoBehaviour
{
    public GameObject Hand, ReviewPannel;
    public GameObject ReviewObject;
    public GameObject[] AllStars;

    public void FillStars(float fillAmount)
    {
        for (int i = 0; i < fillAmount * 5; i++)
        {
            AllStars[i].SetActive(true);
        }

        if (fillAmount >= 0.6f)
        {
            //if (AppOpenAdController.Instance)
            //    AppOpenAdController.Instance.AdShowing = true;

            ReviewObject.SetActive(true);
            PlayerPrefs.SetInt("RateUsStatus", 5);
            StartCoroutine(delay());
        }
        else
        {
            LetterClick();
            StartCoroutine(delay());
        }
        Hand.SetActive(false);

        AudioListener.volume = 1;
        Time.timeScale = 1;
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.5f);
        ReviewPannel.SetActive(false);

        //if (FindObjectOfType<TimerScriptAD>())
        //    FindObjectOfType<TimerScriptAD>().afterRateUs();

    }
   
    public void LetterClick()
    {
        PlayerPrefs.SetInt("RateUsStatus", 5);
    }

    public void starClick(string name)
    {
        FirebaseAnalytics.LogEvent(name);
    }
}
