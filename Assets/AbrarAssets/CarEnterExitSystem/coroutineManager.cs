using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coroutineManager : MonoBehaviour
{
    public static coroutineManager instance;
    //public GameObject _Controller;
    public void Start()
    {
        instance = this;
    }

    public void getIn(string controllerName)
    {
        StartCoroutine(getInControllerDelay(controllerName));
        Firebase.Analytics.FirebaseAnalytics.LogEvent("EnterCar");
    }

    public void getOut(string controllerName)
    {
        StartCoroutine(getOutControllerDelay(controllerName));
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ExitCar");
    }

    IEnumerator getInControllerDelay(string controllerName)
    {
        yield return new WaitForSeconds(0.8f);
        if(controllerName == "Collectable")
        {
            GameManager.instance.uiElements.carEnterBtn.SetActive(false);
            if (!TimerScriptAD.instance.isMission)
            {
                GameManager.instance.uiElements.carExitBtn.SetActive(true);
            }
            else
            {
                GameManager.instance.uiElements.carExitBtn.SetActive(false);
            }
        }
    }

    IEnumerator getOutControllerDelay(string controllerName)
    {
        yield return new WaitForSeconds(0.7f);
        if(controllerName == "Collectable")
        {
            GameManager.instance.uiElements.carEnterBtn.SetActive(false);
            GameManager.instance.uiElements.carExitBtn.SetActive(false);
            GameManager.instance.setController(GameManager.instance.allControllers[0]);
        }
    }
}
