using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadScene : MonoBehaviour
{

    public Image loadingslider;
    public GameObject ContinueButton, Loadingtext;
    public static string SceneName;
    AsyncOperation asyncLoad;
    void Start()
    {

        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        asyncLoad = SceneManager.LoadSceneAsync(SceneName);
        asyncLoad.allowSceneActivation = false;
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / .9f);
            if (loadingslider)
                loadingslider.fillAmount = progress;

            if (asyncLoad.progress >= 0.9f)
            {
                Loadingtext.SetActive(false);
                ContinueButton.SetActive(true);
            }
            yield return null;
        }
    }
    public void ContinueLoadScene()
    {
        //if(AdCalls.instance)
        //{
        //    AdCalls.instance.Admob_Unity();
        //}
        
        asyncLoad.allowSceneActivation = true;
    }
}