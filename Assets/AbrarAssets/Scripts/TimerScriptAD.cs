using UnityEngine;
using UnityEngine.UI;
public class TimerScriptAD : MonoBehaviour
{
    public static TimerScriptAD instance;
    public GameObject adObject;
    public float myFloat = 45f;
    //public Text timer;

    public bool showAD;
    public bool showAD_1;
    public bool rateUs;
    public bool isMission;
    public void Awake()
    {
        instance = this;
    }
    void Update()
    {

        //timer.text = intValue.ToString();
        if (!isMission)
        {
            myFloat -= Time.deltaTime;
            myFloat = Mathf.Max(myFloat, 0f);
            int intValue = Mathf.RoundToInt(myFloat);

            if (myFloat < 10 && myFloat > 0)
            {
                showAD = false;
                if (PlayerPrefs.GetInt("RateUsStatus") != 0)
                {
                    if (showAD_1)
                    {
                        Time.timeScale = .3f;
                        adObject.SetActive(true);
                        showAD_1 = false;
                    }
                }

            }
            else if (myFloat <= 0)
            {
                showAD = true;
                showAD_1 = true;
                afterRateUs();
                if (PlayerPrefs.GetInt("RateUsStatus") == 0)
                {
                    if (rateUsManager.instance)
                    {
                        rateUsManager.instance.activeRateUs();
                    }
                }
                //adObject.SetActive(false);
            }
            else if (myFloat > 10)
            {
                showAD = false;
                showAD_1 = true;
                //adObject.SetActive(false);
            }
        }

    }

    public void checkInterstitial()
    {
        if (myFloat <= 13)
        {
            myFloat += 10;
        }
    }
    public void afterRateUs()
    {
        myFloat = 40;
    }
}
