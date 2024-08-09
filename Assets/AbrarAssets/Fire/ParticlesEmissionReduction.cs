using Invector.vItemManager;
using UnityEngine;

public class ParticlesEmissionReduction : MonoBehaviour
{
    public static ParticlesEmissionReduction Instance { get; private set; }

    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem[] particleSystems;
    [SerializeField] private float fireReductionRate = 1f;

    private bool isFireExtinguished;
    private bool callItOnce = true;

    private void Awake()
    {
        // Ensure there's only one instance of this class
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("water"))
        {
            ReduceParticleEmission();
            ReduceFireFiller();
        }
    }

    private void ReduceParticleEmission()
    {
        foreach (var particleSystem in particleSystems)
        {
            var emission = particleSystem.emission;
            float currentRate = emission.rateOverTime.constant;

            if (currentRate > 0)
            {
                emission.rateOverTime = Mathf.Max(0, currentRate - fireReductionRate);

                // Trigger actions if the fire is extinguished and flag is not set
                if (emission.rateOverTime.constant == 0 && !isFireExtinguished)
                {
                    HandleFireExtinguished();
                    CheckMissionFireAmount();
                }
            }
        }
    }

    private void HandleFireExtinguished()
    {
        var missionManager = MissionManager.Instance;
        if (missionManager == null)
        {
            Debug.LogWarning("MissionManager instance is null.");
            return;
        }

        var currentMission = missionManager.Missions[missionManager.currentMissionIndex];
        if (currentMission == null)
        {
            Debug.LogWarning("Current mission is null.");
            return;
        }

        if (callItOnce)
        {
            //Debug.Log("Current fire amount: " + currentMission.fireAmount);
            currentMission.fireAmount++;
            missionManager.GameElements.fireAmount.text = currentMission.fireAmount.ToString();
            GetComponent<AudioSource>().enabled = false;
            callItOnce = false;
        }
    }

    private void CheckMissionFireAmount()
    {
        var missionManager = MissionManager.Instance;
        if (missionManager == null)
        {
            Debug.LogWarning("MissionManager instance is null.");
            return;
        }

        var currentMission = missionManager.Missions[missionManager.currentMissionIndex];
        if (currentMission == null) return;

        if (currentMission.fireAmount >= currentMission.fireAmountToDone &&
            currentMission.rescueManCounter >= currentMission.rescueManCounterToDone)
        {
            isFireExtinguished = true;
            Invoke(nameof(CompleteMissionActions), 3f);
        }
    }

    private void CompleteMissionActions()
    {
        var firefighterManager = FireFighterManager.instance;
        if (firefighterManager != null)
        {
            if (firefighterManager.debugMode) Debug.Log("Completing mission actions...");

            firefighterManager.itemManager.UnequipCurrentEquipedItem(0);
            //firefighterManager.itemManager.EquipItemToEquipSlot(1, 1, firefighterManager.Melee);
            //firefighterManager.itemManager.EquipItemToCurrentEquipSlot(firefighterManager.Melee, 1);

            firefighterManager.SetWaterActive(false);
            firefighterManager.waterButton.SetActive(false);
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

        var missionManager = MissionManager.Instance;
        if (missionManager != null)
        {
            missionManager.CompleteMission();
            //Debug.Log("Mission completed.");
        }
        else
        {
            //Debug.LogWarning("MissionManager instance is null.");
        }
    }

    private void ReduceFireFiller()
    {
        var missionManager = MissionManager.Instance;
        if (missionManager == null)
        {
            Debug.LogWarning("MissionManager instance is null.");
            return;
        }

        var gameElements = missionManager.GameElements;
        if (gameElements?.fireFiller != null)
        {
            gameElements.fireFiller.value -= fireReductionRate;
        }
        else
        {
            Debug.LogWarning("GameElements or fireFiller is null.");
        }
    }
}
