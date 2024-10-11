using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class InGameMissionsManager : MonoBehaviour
{
    public static InGameMissionsManager Instance;
    [Header("Mission UI Elements")]
    [Tooltip("Panel displaying the in-game mission options.")]
    public GameObject missionPanel;

    [HideInInspector]
    public controllerCollision playerTruck;   
    [HideInInspector]
    public bool inGameMission; 
    
    [HideInInspector]
    public GameObject triggerObject;

    [Tooltip("Button to start the mission and unlock the truck.")]
    public Button acceptMissionButton;

    [Tooltip("Button for the player to opt out of the mission.")]
    public Button declineMissionButton;

    [Tooltip("Controller detection script to manage player interactions.")]
    public controllerDetection controllerDetector;

    public GameObject alreadyInMission;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        // Ensure buttons have listeners attached
        if (acceptMissionButton != null)
            acceptMissionButton.onClick.AddListener(AcceptMission);

        if (declineMissionButton != null)
            declineMissionButton.onClick.AddListener(DeclineMission);
    }

    /// <summary>
    /// Called when the player accepts the mission.
    /// It starts the mission, activates the truck, and sets the controller.
    /// </summary>
    private void AcceptMission()
    {
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.OnInGameAcceptMission();
        }
        GameController.instance.myPlayers.GetComponent<RCCP_CarController>().KillEngine();
        inGameMission = true;
        missionPanel.SetActive(false);
        TimerScriptAD.instance.isMission = true;
        triggerObject.SetActive(false);
        Invoke("ActivateMyObjets", 10f);
    }
    public void ActivateMyObjets()
    {
        controllerDetector.controllerName = "Collectable";
        controllerDetector.controller = playerTruck.gameObject;
        playerTruck.GetComponent<controllerCollision>().carEngineEnabled.SetActive(true);
        controllerDetector.getInBtn();
    }

    /// <summary>
    /// Called when the player declines the mission.
    /// It hides the mission panel and deactivates the mission.
    /// </summary>
    private void DeclineMission()
    {
        missionPanel.SetActive(false);
        playerTruck = null;
        TimerScriptAD.instance.isMission = false;
    }
}
