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
    }

    public void getOut(string controllerName)
    {
        StartCoroutine(getOutControllerDelay(controllerName));
    }

    IEnumerator getInControllerDelay(string controllerName)
    {
        yield return new WaitForSeconds(0.8f);
        if(controllerName == "Collectable")
        {
            GameManager.instance.uiElements.carEnterBtn.SetActive(false);
            GameManager.instance.uiElements.carExitBtn.SetActive(true);
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
