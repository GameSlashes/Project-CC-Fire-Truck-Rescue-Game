using Invector.vItemManager;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class FireFighterManager : MonoBehaviour
{
    public static FireFighterManager instance;

    [Header("Firefighting Equipment")]
    public GameObject fireGun; // The fire gun used by the firefighter
    public GameObject firePipe; // The fire pipe used for larger fires
    public GameObject water; // The water object for visual effects
    public GameObject fireExtinguisherSpray; // The fire extinguisher spray object for visual effects
    public vItemManager itemManager;
    public vItem /*BaseBallBat,*/ Rifle,Melee, Pistol;

    [Header("UI Elements")]
    public GameObject fireExtinguisherButton; // Button for equipping the fire extinguisher
    public GameObject waterButton; // Button for activating/deactivating water
    public Slider waterCapacitySlider; // Slider for water capacity
    public GameObject refillPanel; // Panel for refilling water
    public Button refillButton; // Button for refilling water
    public Button fireExtinguisherSprayButton; // Button for toggling the fire extinguisher spray
    public Slider fireExtinguisherCapacitySlider; // Slider for fire extinguisher capacity
    public GameObject fireExtinguisherRefillPanel; // Panel for refilling fire extinguisher
    public Button fireExtinguisherRefillButton; // Button for refilling fire extinguisher
    public Button ambulanceManagerAnimationButton; // Button for refilling fire extinguisher

    [Header("Debugging")]
    public bool debugMode; // Enable or disable debug logging

    private float maxWaterCapacity = 100f; // Maximum water capacity
    private float currentWaterCapacity;
    private float maxFireExtinguisherCapacity = 100f; // Maximum fire extinguisher capacity
    private float currentFireExtinguisherCapacity;
    [HideInInspector]
    public bool isFirePipeActive = false;
    private bool isFireExtinguisherSprayActive = false;
    private AmbulanceController ambulanceController;

    public void Start()
    {
        instance = this;
        InitializeUI();
        UpdateWaterButtonState();
        currentWaterCapacity = maxWaterCapacity; // Initialize the water capacity to maximum
        UpdateWaterSlider();
        currentFireExtinguisherCapacity = maxFireExtinguisherCapacity; // Initialize the fire extinguisher capacity to maximum
        UpdateFireExtinguisherSlider();
    }

    /// <summary>
    /// Initialize the UI elements with their respective functions.
    /// </summary>
    private void InitializeUI()
    {
        if (fireExtinguisherButton != null)
        {
            fireExtinguisherButton.SetActive(false); // Ensure the button is initially inactive
        }
        else
        {
            if (debugMode) Debug.LogWarning("Fire extinguisher button is not assigned.");
        } 
        
        if (ambulanceManagerAnimationButton != null)
        {
            ambulanceManagerAnimationButton.gameObject.SetActive(false); // Ensure the button is initially inactive
            ambulanceManagerAnimationButton.onClick.AddListener(ActivateAmbulanceComponents);
        }
        else
        {
            if (debugMode) Debug.LogWarning("ambulanceManagerAnimationButton button is not assigned.");
        }

        if (waterButton != null)
        {
            waterButton.GetComponent<Button>().onClick.AddListener(() => SetWaterActive(!IsWaterActive()));
        }
        else
        {
            if (debugMode) Debug.LogWarning("Water button is not assigned.");
        }

        if (waterCapacitySlider != null)
        {
            waterCapacitySlider.maxValue = maxWaterCapacity;
            waterCapacitySlider.value = currentWaterCapacity;
        }
        else
        {
            if (debugMode) Debug.LogWarning("Water capacity slider is not assigned.");
        }

        if (refillButton != null)
        {
            refillButton.onClick.AddListener(RefillWater);
        }
        else
        {
            if (debugMode) Debug.LogWarning("Refill button is not assigned.");
        }

        if (refillPanel != null)
        {
            refillPanel.SetActive(false); // Ensure the panel is initially inactive
        }
        else
        {
            if (debugMode) Debug.LogWarning("Refill panel is not assigned.");
        }

        if (fireExtinguisherSprayButton != null)
        {
            fireExtinguisherSprayButton.onClick.AddListener(() => ToggleFireExtinguisherSpray());
        }
        else
        {
            if (debugMode) Debug.LogWarning("Fire extinguisher spray button is not assigned.");
        }

        if (fireExtinguisherCapacitySlider != null)
        {
            fireExtinguisherCapacitySlider.maxValue = maxFireExtinguisherCapacity;
            fireExtinguisherCapacitySlider.value = currentFireExtinguisherCapacity;
        }
        else
        {
            if (debugMode) Debug.LogWarning("Fire extinguisher capacity slider is not assigned.");
        }

        if (fireExtinguisherRefillButton != null)
        {
            fireExtinguisherRefillButton.onClick.AddListener(RefillFireExtinguisher);
        }
        else
        {
            if (debugMode) Debug.LogWarning("Fire extinguisher refill button is not assigned.");
        }

        if (fireExtinguisherRefillPanel != null)
        {
            fireExtinguisherRefillPanel.SetActive(false); // Ensure the panel is initially inactive
        }
        else
        {
            if (debugMode) Debug.LogWarning("Fire extinguisher refill panel is not assigned.");
        }
    }

    /// <summary>
    /// Equip or unequip the fire pipe and update the fire gun accordingly.
    /// </summary>
    /// <param name="state">The desired state of the fire pipe (true for active, false for inactive).</param>
    public void SetPipeActive(bool state)
    {
        if (firePipe != null && fireGun != null)
        {
            firePipe.SetActive(state);
            fireGun.SetActive(state); // Ensure the fire gun is the opposite state of the fire pipe
            if (debugMode) Debug.Log("Fire pipe state set to: " + state);
            UpdateWaterButtonState();
        }
        else
        {
            if (debugMode) Debug.LogWarning("Fire pipe or fire gun is not assigned.");
        }
    }

    public void TogglePipeState()
    {
        isFirePipeActive = !isFirePipeActive;
        if (debugMode) Debug.Log(isFirePipeActive);
        SetPipeActive(isFirePipeActive);

    }
    public void SetEquipId()
    {
        Debug.Log(itemManager);
        itemManager.UnequipCurrentEquipedItem(0);
        itemManager.EquipItemToEquipSlot(0, 0, Rifle);
        MissionManager.Instance.GameElements.mapLine.GetComponent<MapLine>().endPoint = MissionManager.Instance.Missions[MissionManager.Instance.currentMissionIndex].missionDataObjectData[2].objTOActivate.gameObject;
        MissionManager.Instance.Missions[MissionManager.Instance.currentMissionIndex].missionDataObjectData[1].objTOActivate.gameObject.SetActive(false);
        //itemManager.EquipItemToCurrentEquipSlot(Rifle, 0);
    }
    /// <summary>
    /// Check if the water object is active.
    /// </summary>
    /// <returns>Returns true if water is active, false otherwise.</returns>
    public bool IsWaterActive()
    {
        if (water != null)
        {
            return water.activeSelf;
        }
        else
        {
            if (debugMode) Debug.LogWarning("Water object is not assigned.");
            return false;
        }
    }

    /// <summary>
    /// Activate or deactivate the water object.
    /// </summary>
    /// <param name="state">The desired state of the water (true for active, false for inactive).</param>
    public void SetWaterActive(bool state)
    {
        if (water != null)
        {
            water.SetActive(state);
            if (debugMode) Debug.Log("Water state set to: " + state);
            if (state) // If activating water, reduce the capacity
            {
                StartCoroutine(ConsumeWater());
            }
        }
        else
        {
            if (debugMode) Debug.LogWarning("Water object is not assigned.");
        }
    }

    /// <summary>
    /// Coroutine to consume water over time.
    /// </summary>
    private IEnumerator ConsumeWater()
    {
        while (IsWaterActive() && currentWaterCapacity > 0)
        {
            currentWaterCapacity -= .5f; // Adjust this value to control the water consumption rate
            UpdateWaterSlider();
            if (currentWaterCapacity <= 0)
            {
                SetWaterActive(false);
                waterButton.SetActive(false);
                if (debugMode) Debug.Log("Water capacity reached zero. Deactivating water.");
                break;
            }
            yield return new WaitForSeconds(0.1f); // Adjust this value to control the update frequency
        }

        // Show the refill panel when water is depleted
        if (refillPanel != null && currentWaterCapacity <= 0)
        {
            refillPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Update the water capacity slider to reflect the current water capacity.
    /// </summary>
    private void UpdateWaterSlider()
    {
        if (waterCapacitySlider != null)
        {
            waterCapacitySlider.value = currentWaterCapacity;
        }
    }

    /// <summary>
    /// Refill the water capacity to the maximum.
    /// </summary>
    private void RefillWater()
    {
        FillTheWater();
        //if (FindObjectOfType<Handler>())
        //    FindObjectOfType<Handler>().ShowRewardedAdsBoth(FillTheWater);


    }
    public void FillTheWater()
    {
        currentWaterCapacity = maxWaterCapacity;
        UpdateWaterSlider();
        if (debugMode) Debug.Log("Water refilled to maximum capacity.");

        // Hide the refill panel after refilling
        if (refillPanel != null)
        {
            refillPanel.SetActive(false);
            waterButton.SetActive(true);
        }
    }

    /// <summary>
    /// Update the active state of the water button based on the fire pipe.
    /// </summary>
    private void UpdateWaterButtonState()
    {
        if (fireExtinguisherButton != null && waterButton != null)
        {
            waterButton.SetActive(firePipe.activeSelf);
            waterCapacitySlider.gameObject.SetActive(firePipe.activeSelf);
        }
    }

    /// <summary>
    /// Handle trigger events to activate the fire extinguisher button.
    /// </summary>
    /// <param name="other">The collider that triggered the event.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Check for "Action" tag
        if (other.CompareTag("Action"))
        {
            // Activate the fire extinguisher button if it exists
            if (fireExtinguisherButton != null)
            {
                fireExtinguisherButton.SetActive(true);
                if (debugMode) Debug.Log("Fire extinguisher button activated.");
            }

            // Check if the colliding object has an AmbulanceController component
            ambulanceController = other.gameObject.GetComponent<AmbulanceController>();
            if (ambulanceController != null)
            {
                // Call the ActivateAmbulanceComponents method to enable animators and handle patients
                ambulanceManagerAnimationButton.gameObject.SetActive(false);
                if (debugMode) Debug.Log("Ambulance components activated.");
            }
            else
            {
                if (debugMode) Debug.LogWarning("No AmbulanceController found on the colliding object.");
            }
        }
    }

    public void ActivateAmbulanceComponents()
    {
        ambulanceController.ActivateAmbulanceComponents();
    }

    /// <summary>
    /// Handle trigger events to deactivate the fire extinguisher button.
    /// </summary>
    /// <param name="other">The collider that triggered the event.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Action"))
        {
            if (fireExtinguisherButton != null)
            {
                fireExtinguisherButton.SetActive(false);
                if (debugMode) Debug.Log("Fire extinguisher button deactivated.");
            }
        }
    }

    /// <summary>
    /// Toggle the fire extinguisher spray state.
    /// </summary>
    private void ToggleFireExtinguisherSpray()
    {
        isFireExtinguisherSprayActive = !isFireExtinguisherSprayActive;
        SetFireExtinguisherSprayActive(isFireExtinguisherSprayActive);
        fireExtinguisherCapacitySlider.gameObject.SetActive(isFireExtinguisherSprayActive);
    }

    /// <summary>
    /// Activate or deactivate the fire extinguisher spray object.
    /// </summary>
    /// <param name="state">The desired state of the fire extinguisher spray (true for active, false for inactive).</param>
    private void SetFireExtinguisherSprayActive(bool state)
    {
        if (fireExtinguisherSpray != null)
        {
            fireExtinguisherSpray.SetActive(state);
            if (debugMode) Debug.Log("Fire extinguisher spray state set to: " + state);
            if (state) // If activating the spray, reduce the capacity
            {
                StartCoroutine(ConsumeFireExtinguisher());
            }
        }
        else
        {
            if (debugMode) Debug.LogWarning("Fire extinguisher spray object is not assigned.");
        }
    }

    /// <summary>
    /// Coroutine to consume fire extinguisher spray over time.
    /// </summary>
    private IEnumerator ConsumeFireExtinguisher()
    {
        while (isFireExtinguisherSprayActive && currentFireExtinguisherCapacity > 0)
        {
            currentFireExtinguisherCapacity -= 1f; // Adjust this value to control the spray consumption rate
            UpdateFireExtinguisherSlider();
            if (currentFireExtinguisherCapacity <= 0)
            {
                SetFireExtinguisherSprayActive(false);
                fireExtinguisherSprayButton.gameObject.SetActive(false);
                if (debugMode) Debug.Log("Fire extinguisher capacity reached zero. Deactivating spray.");
                break;
            }
            yield return new WaitForSeconds(0.1f); // Adjust this value to control the update frequency
        }

        // Show the refill panel when fire extinguisher spray is depleted
        if (fireExtinguisherRefillPanel != null && currentFireExtinguisherCapacity <= 0)
        {
            fireExtinguisherRefillPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Update the fire extinguisher capacity slider to reflect the current capacity.
    /// </summary>
    private void UpdateFireExtinguisherSlider()
    {
        if (fireExtinguisherCapacitySlider != null)
        {
            fireExtinguisherCapacitySlider.value = currentFireExtinguisherCapacity;
        }
    }

    /// <summary>
    /// Refill the fire extinguisher capacity to the maximum.
    /// </summary>
    private void RefillFireExtinguisher()
    {
        currentFireExtinguisherCapacity = maxFireExtinguisherCapacity;
        UpdateFireExtinguisherSlider();
        if (debugMode) Debug.Log("Fire extinguisher refilled to maximum capacity.");

        // Hide the refill panel after refilling
        if (fireExtinguisherRefillPanel != null)
        {
            fireExtinguisherRefillPanel.SetActive(false);
            fireExtinguisherSprayButton.gameObject.SetActive(true);
        }
    }

}
