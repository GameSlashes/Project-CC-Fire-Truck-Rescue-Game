using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadingAD : MonoBehaviour
{
    public Handler aDs;

    public void OnEnable()
    {
        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return new WaitForSecondsRealtime(.5f);
        this.gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        aDs.ShowInterstitialAd();
    }
}
