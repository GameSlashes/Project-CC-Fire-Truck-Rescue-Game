using System;
using System.Collections;
using System.Drawing;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    [Serializable]
    public class MissionDataObjectData
    {
        public GameObject objTOActivate;
    }
    [Serializable]
    public class MissionData
    {
        [Header("Mission Settings")]
        public GameObject MissionObject;  
        [Header("Mission CutScene")]
        public GameObject CutScene;

        [Header("Mission Objectives")]
        public string Objectives;

        [Header("Mission Timing")]
        public bool IsTimeBased;
        [Range(0, 60)] public int Minutes;
        [Range(10, 60)] public int Seconds;

        [Header("Rewards")]
        public bool IsRewardBased;
        public int CoinReward;

        [Header("RescueMan")]
        public bool IsRescueMan;
        public int rescueManCounter;
        public int rescueManCounterToDone;



        [Header("Fire")]
        public bool IsFire;
        public int fireAmount;
        public int fireAmountToDone;

        [Header("TotalReward")]
        public int totalReward;

        [Header("MissionDataObjectData")]
        public MissionDataObjectData[] missionDataObjectData;
    }

    [Serializable]
    public class GameDialogues
    {
        public GameObject MissionCompleteText, MissionComplete, MissionFailedText, MissionFailed, LoadingScreen, Timer, ObjectivePanel, MissionCompleteRewardCoinsObj, DoubleRewardBtn, MissionInitialization, mapLine;
        public Text MissionCompleteReward, TimerText, ObjectiveText, ObjectiveText_1, CoinRewardText, timerText, rescueMan, rescueManToDone, fireAmount, fireAmountToDone, totalRewardText;
        public Slider fireFiller, timerFiller;
    }

    [Header("Game Dialogues")]
    public GameDialogues GameElements;
    public MissionData[] Missions;
    public Button AcceptButton, RejectButton, NextMissionButton;
    public controllerDetection detector;
    public GameObject vThirdPersonCamera;
    public TimerScriptAD stopTimer;
    public GameObject trafficCars;
    public GameObject myCar;
    public int currentMissionIndex = 0;
    private bool isTimerEnabled, timerPaused = false;
    private float missionTime;
    private RCCP_CarController players;
    private float count = 6;
    private bool flag;
    public Button SirenOnButton;  // Button to turn the siren on
    public Button SirenOffButton; // Button to turn the siren off
    private bool missionConcluded = false; // Flag to track if mission has concluded


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        AcceptButton.onClick.AddListener(OnAcceptMission);
        RejectButton.onClick.AddListener(OnRejectMission);
        NextMissionButton.onClick.AddListener(OnNextMission);
        SirenOnButton.onClick.AddListener(TurnSirenOn);
        SirenOffButton.onClick.AddListener(TurnSirenOff);
    }

    public void CallitOnEnable()
    {
        if (GameElements.ObjectivePanel.activeSelf)
        {
            var mission = Missions[currentMissionIndex];
            GameElements.mapLine.GetComponent<MapLine>().endPoint =
            mission.missionDataObjectData[0].objTOActivate.gameObject;
            GameElements.ObjectiveText.text = mission.Objectives;
            GameElements.ObjectiveText_1.text = mission.Objectives;
            GameElements.CoinRewardText.text = mission.CoinReward.ToString();
        }
        players = myCar.GetComponent<RCCP_CarController>();
        players.GetComponent<Rigidbody>().velocity = Vector3.zero;
        players.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        count = 6;
        GameElements.timerText.text = $"{count} seconds";
        flag = true;
    }

    public void ActivateMission(int missionIndex)
    {
        if (missionIndex < 0 || missionIndex >= Missions.Length)
        {
            Debug.LogWarning("Invalid mission index.");
            return;
        }

        if (myCar == null)
        {
            players = myCar.GetComponent<RCCP_CarController>();
        }
        else
        {
            InitializePlayer(missionIndex);
        }
    }

    private void Update()
    {
        if (isTimerEnabled && !timerPaused)
        {
            UpdateTimer();
        }

        if (flag)
        {
            UpdateMissionInitializationCountdown();
        }
    }

    private void UpdateTimer()
    {
        if (missionTime <= 0) return;

        missionTime -= Time.unscaledDeltaTime;
        GameElements.TimerText.text = $"{(int)(missionTime / 60):00}:{(int)(missionTime % 60):00}";
        GameElements.timerFiller.value = missionTime / (Missions[currentMissionIndex].Minutes * 60 + Missions[currentMissionIndex].Seconds);

        if (missionTime <= 15)
        {
            GameElements.TimerText.color = UnityEngine.Color.red;
        }

        if (missionTime <= 0)
        {
            missionTime = 0;
            OnMissionFailed();
        }
    }

    private void UpdateMissionInitializationCountdown()
    {
        if (count > 0)
        {
            count -= Time.unscaledDeltaTime;
            GameElements.timerText.text = $"{(int)count} seconds";
        }
        else
        {
            GameElements.MissionInitialization.SetActive(true);
            GameElements.ObjectivePanel.SetActive(false);
            flag = false;
        }
    }

    private void OnMissionFailed()
    {
        if (missionConcluded) return; // Prevent both complete and failed messages
        missionConcluded = true; // Set the flag to true

        Time.timeScale = 1;
        GameElements.MissionFailedText.SetActive(true);
        GameElements.mapLine.SetActive(false);
        StartCoroutine(ShowDialogue(GameElements.MissionFailedText, GameElements.MissionFailed));
        isTimerEnabled = false;
        stopTimer.isMission = false;
        stopTimer.myFloat = 30;
        trafficCars.SetActive(true);


        playBtnSound();
        Time.timeScale = 1;
        ResetMissionUI();
        Missions[currentMissionIndex].MissionObject.SetActive(false);
        GameElements.fireAmount.text = "0";
        GameElements.fireAmountToDone.text = "0";
        GameElements.rescueManToDone.text = "0";
        GameElements.rescueMan.text = "0";

        var firefighterManager = FireFighterManager.instance;
        if (firefighterManager != null)
        {
            if (firefighterManager.debugMode) Debug.Log("Fail mission actions...");

            firefighterManager.itemManager.UnequipCurrentEquipedItem(0);

            firefighterManager.SetWaterActive(false);
            firefighterManager.waterButton.SetActive(false);
            firefighterManager.offwaterButton.SetActive(false);
            firefighterManager.SetPipeActive(false);

            if (firefighterManager.isFirePipeActive)
            {
                firefighterManager.TogglePipeState();
            }

            if (firefighterManager.debugMode)
            {
                Debug.Log("Water state set to false.");
                Debug.Log("Water button deactivated.");
                Debug.Log("Pipe state set to false.");
            }
        }
        else
        {
            Debug.LogWarning("FireFighterManager instance is null.");
        }
    }

    public void CompleteMission()
    {
        if (missionConcluded) return; // Prevent both complete and failed messages
        missionConcluded = true; // Set the flag to true

        HandleRewards();
        playBtnSound();
        GameElements.mapLine.SetActive(false);
        GameElements.MissionCompleteText.SetActive(true);
        StartCoroutine(ShowDialogue(GameElements.MissionCompleteText, GameElements.MissionComplete));
        isTimerEnabled = false;
        stopTimer.isMission = false;

        GameElements.fireAmount.text = "0";
        GameElements.fireAmountToDone.text = "0";
        GameElements.rescueManToDone.text = "0";
        GameElements.rescueMan.text = "0";
    }

    private IEnumerator ShowDialogue(GameObject initialText, GameObject finalText)
    {
        yield return new WaitForSeconds(3f);
        Time.timeScale = 1;
        initialText.SetActive(false);
        finalText.SetActive(true);
    }
    private void HandleRewards()
    {
        var mission = Missions[currentMissionIndex];
        // Calculate additional reward based on remaining time
        if (mission.IsTimeBased && missionTime > 0)
        {
            int timeBasedReward = Mathf.FloorToInt(missionTime); // Example: 1 coin per remaining second
            mission.totalReward += timeBasedReward;
            GameElements.totalRewardText.text = mission.totalReward.ToString();
            SaveData.instance.Coins += timeBasedReward;
        }
        if (mission.IsRewardBased && mission.CoinReward > 0)
        {
            GameElements.MissionCompleteReward.text = mission.CoinReward.ToString();
            SaveData.instance.Coins += mission.CoinReward;
        }
        else
        {
            GameElements.MissionCompleteRewardCoinsObj.SetActive(false);
            GameElements.DoubleRewardBtn.SetActive(false);
        }
    }

    public int CurrentMission
    {
        get => PlayerPrefs.GetInt("Mission", 1);
        set => PlayerPrefs.SetInt("Mission", value);
    }

    public void OnAcceptMission()
    {
        Time.timeScale = 1;
        ShowInterstitialAd();
        playBtnSound();
        var mission = Missions[currentMissionIndex];

        vThirdPersonCamera.SetActive(false);
        mission.CutScene.SetActive(true);
        Invoke(nameof(ActivateMyCameras), 10f);

        GameElements.MissionInitialization.SetActive(false);
        flag = false;
        GameElements.mapLine.SetActive(true);

        GameElements.fireAmount.text = mission.fireAmount.ToString();
        GameElements.fireAmountToDone.text = mission.fireAmountToDone.ToString();
        GameElements.rescueManToDone.text = mission.rescueManCounterToDone.ToString();
        GameElements.rescueMan.text = mission.rescueManCounter.ToString();
        stopTimer.isMission = true;
        stopTimer.myFloat = 50;

        ActivateMission(currentMissionIndex);
        AudioListener.pause = false;
        trafficCars.SetActive(false);
        // Log the purchased character's index

        Firebase.Analytics.FirebaseAnalytics.LogEvent("MissionNumber_____Accepted",new Firebase.Analytics.Parameter("Mission_index", currentMissionIndex));
    }
    public void ActivateMyCameras()
    {
        playBtnSound();
        var mission = Missions[currentMissionIndex];
        mission.CutScene.SetActive(false);
    }
    private void OnRejectMission()
    {
        ShowInterstitialAd();
        playBtnSound();
        Time.timeScale = 1;
        Missions[currentMissionIndex].MissionObject.SetActive(false);
        GameElements.ObjectivePanel.SetActive(false);
        GameElements.Timer.SetActive(false);
        isTimerEnabled = false;
        stopTimer.myFloat = 30;
        stopTimer.isMission = false;
        GameElements.MissionInitialization.SetActive(false);
        flag = false;
        Firebase.Analytics.FirebaseAnalytics.LogEvent("MissionNumber_____Reject", new Firebase.Analytics.Parameter("RejectMission_index", currentMissionIndex));

    }

    private void OnNextMission()
    {
        ShowInterstitialAd();
        playBtnSound();
        Time.timeScale = 1;
        ResetMissionUI();
        missionConcluded = false; // Reset the flag for the next mission
        Missions[currentMissionIndex].MissionObject.SetActive(false);
        stopTimer.isMission = false;
        trafficCars.SetActive(true);

        if (currentMissionIndex < Missions.Length - 1)
        {
            currentMissionIndex++;
        }
        else
        {
            currentMissionIndex = 0;
        }
        Firebase.Analytics.FirebaseAnalytics.LogEvent("MissionNumber_____Complete", new Firebase.Analytics.Parameter("Mission_index____Complete", currentMissionIndex));

    }

    private void InitializePlayer(int missionIndex)
    {
        var controllerCollision = players.GetComponentInChildren<controllerCollision>();
        detector.controller = controllerCollision.gameObject;
        controllerCollision.carEngineEnabled.SetActive(true);
        detector.controllerName = "Collectable";
        detector.getInBtn();
        SetupMission(missionIndex);
    }

    private void SetupMission(int missionIndex)
    {
        currentMissionIndex = missionIndex;
        var mission = Missions[currentMissionIndex];
        mission.MissionObject.SetActive(true);
        missionTime = mission.Minutes * 60 + mission.Seconds;
        GameElements.Timer.SetActive(mission.IsTimeBased);
        isTimerEnabled = mission.IsTimeBased;
    }

    private void ResetMissionUI()
    {
        GameElements.MissionCompleteText.SetActive(false);
        GameElements.MissionComplete.SetActive(false);
        isTimerEnabled = false;
    }

    private void ShowInterstitialAd()
    {
        var handler = FindObjectOfType<Handler>();
        handler?.showWaitInterstitial();
        PlayerPrefs.SetInt("InterstitialAdLoadDelay", 5);
    }
    public void playBtnSound()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound);
        }
    }  
    public void MissionCompletOn()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.MissionCompleteOn();
        }
    }   
    public void MissionFailOn()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.MissionFailOn();
        }
    } 
    public void MissionCompletOff()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.MissionCompleteOff();
        }
    }   
    public void MissionFailOff()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.MissionFailOff();
        }
    }


    public void TurnSirenOn()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.SirenOn();
        }
        playBtnSound();
        SirenOnButton.gameObject.SetActive(false); // Hide the "On" button when the siren is on
        SirenOffButton.gameObject.SetActive(true); // Show the "Off" button when the siren is on
    }

    public void TurnSirenOff()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.SirenOff();
        }
        playBtnSound();
        SirenOnButton.gameObject.SetActive(true); // Show the "On" button when the siren is off
        SirenOffButton.gameObject.SetActive(false); // Hide the "Off" button when the siren is off
    }

    public void PanelOpen()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PanelOpen();
        }
    }   
    public void PanelOff()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PanelOff();
        }
    }
}
