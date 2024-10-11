using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableFalse : MonoBehaviour
{
    public void OnEnable ()
    {
        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);
    }
}
