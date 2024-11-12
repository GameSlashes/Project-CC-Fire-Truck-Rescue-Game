using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class VersionManager : MonoBehaviour
{
    public static bool isFirebaseInitialized = false;

    [Header("String Value of Remote Configure")]
    public string versionCheckConfigKey;
    public string liveVersionKey;

    [Header("Game Link")]
    public string gameLink;

    public GameObject updateDialogueBox;

    [Header("------------------------Fetch Data------------------------")]

    [Header("Result from Remote Configure")]
    public bool isVersionCheckEnable;
    public double liveVersionNumber;

    [Header("Result from Current Build")]
    public string currentVersionNumber;



    //void Start()
    //{
    //    firebaseInitialization();
    //}

    void Start()
    {
        StartCoroutine(CheckInternetConnection(() =>
        {
            firebaseInitialization();
        }));

        DontDestroyOnLoad(this.gameObject);
    }

    IEnumerator CheckInternetConnection(Action onConnected)
    {
        while (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No internet connection, waiting...");
            yield return new WaitForSeconds(5);
        }
        Debug.Log("Internet connection available.");
        onConnected?.Invoke();
    }


    void firebaseInitialization()
    {
        System.Collections.Generic.Dictionary<string, object> defaults =
          new System.Collections.Generic.Dictionary<string, object>();

        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
          .ContinueWithOnMainThread(task =>
          {
              Logging.Log("RemoteConfig configured and ready!");
              isFirebaseInitialized = true;
              FetchDataAsync();
          });
    }

    public void showFetchData()
    {
        isVersionCheckEnable = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(versionCheckConfigKey).BooleanValue;
        liveVersionNumber = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(liveVersionKey).DoubleValue;

        currentVersionNumber = Application.version;

        if (isVersionCheckEnable == true)
        {
            if (double.Parse(currentVersionNumber) < liveVersionNumber)
            {
                updateDialogueBox.SetActive(true);
            }
        }
    }

    public Task FetchDataAsync()
    {
        Logging.Log("Fetching data...");
        System.Threading.Tasks.Task fetchTask =
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
            TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            Logging.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Logging.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            Logging.Log("Fetch completed successfully!");
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                .ContinueWithOnMainThread(task =>
                {
                    Logging.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                                   info.FetchTime));
                    showFetchData();
                });

                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {

                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        Logging.Log("Fetch failed for unknown reason");

                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        Logging.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                Logging.Log("Latest Fetch call still pending.");
                break;
        }
    }

    public void applicationLink()
    {
        Application.OpenURL(gameLink);
    }
}
