using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPanel : MonoBehaviour
{
    void Start()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ExitPanel_Open");
    }
}
