using MTAssets.EasyMinimapSystem;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class UIElements
{
    public GameObject carEnterBtn;
    public GameObject carExitBtn;
    public GameObject fadeManager;
    public GraphicRaycaster _DialoguePopup;
    public GameObject pausePanel;
}

[System.Serializable]
public class AllControllers
{
    public GameObject Controller;
    public GameObject Camera;
    public GameObject playerPosition;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AllControllers[] allControllers;
    public UIElements uiElements;
    public bool jerking;

    [HideInInspector]
    public GameObject bikeTrafficObj;
    public controllerDetection _controllerDetection;

    public GameObject ResetCarAvalible;
    public GameObject IsBusted;
    public GameObject rccCamera;
    public MinimapRenderer _CamProvider;


    public void OnEnable()
    {
        instance = this;
    }

    public void Start()
    {

    }

    public void setController(AllControllers controllerName)
    {
        if (controllerName.playerPosition)
        {
            if (FindObjectOfType<RCCP_Camera>())
            {
                controllerName.playerPosition.transform.position = FindObjectOfType<RCCP_Camera>().cameraTarget.playerVehicle.GetComponentInChildren<carEngineEnable>().playerPosition.transform.position;
            }              
        }
        for (int i = 0; i < allControllers.Length; i++)
        {
            allControllers[i].Controller.SetActive(false);
            allControllers[i].Camera.SetActive(false);
            if (allControllers[i].playerPosition)
                allControllers[i].playerPosition.SetActive(false);
        }

        controllerName.Controller.SetActive(true);
        controllerName.Camera.SetActive(true);

        if (controllerName.playerPosition)
            controllerName.playerPosition.SetActive(true);

    }

    public void setTrafficController()
    {
        for (int i = 0; i < allControllers.Length; i++)
        {
            allControllers[i].Controller.SetActive(false);
            allControllers[i].Camera.SetActive(false);
            if (allControllers[i].playerPosition)
                allControllers[i].playerPosition.SetActive(false);
        }
    }

    public void playBtnSound()
    {
        //if (soundManager.instance)
        //    soundManager.instance.playBtnSound();
    }
}
