using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireFighterManager : MonoBehaviour
{
    [Header("Firefighting Equipment")]
    public GameObject fireGun; // The fire gun used by the firefighter
    public GameObject firePipe; // The fire pipe used for larger fires
    public GameObject water; // The water object for visual effects

    [Header("UI Elements")]
    public GameObject fireExtinguisherButton; // Button for equipping the fire extinguisher
    public GameObject waterButton; // Button for activating/deactivating water
    public Slider waterCapacitySlider; // Slider for water capacity
    public GameObject refillPanel; // Panel for refilling water
    public Button refillButton; // Button for refilling water

    private float maxWaterCapacity = 100f; // Maximum water capacity
    private float currentWaterCapacity;
    private bool isFirePipeActive = false;

    private void Start()
    {
        InitializeUI();
        UpdateWaterButtonState();
        currentWaterCapacity = maxWaterCapacity; // Initialize the water capacity to maximum
        UpdateWaterSlider();
    }

    /// <summary>
    /// Initialize the UI elements with their respective functions.
    /// </summary>
    private void InitializeUI()
    {
        if (fireExtinguisherButton != null)
        {
            fireExtinguisherButton.GetComponent<Button>().onClick.AddListener(TogglePipeState);
            //fireExtinguisherButton.GetComponent<Button>().onClick.AddListener(() => SetPipeActive(!firePipe.activeSelf));
            fireExtinguisherButton.SetActive(false); // Ensure the button is initially inactive
        }
        else
        {
            Debug.LogWarning("Fire extinguisher button is not assigned.");
        }

        if (waterButton != null)
        {
            waterButton.GetComponent<Button>().onClick.AddListener(() => SetWaterActive(!IsWaterActive()));
        }
        else
        {
            Debug.LogWarning("Water button is not assigned.");
        }

        if (waterCapacitySlider != null)
        {
            waterCapacitySlider.maxValue = maxWaterCapacity;
            waterCapacitySlider.value = currentWaterCapacity;
        }
        else
        {
            Debug.LogWarning("Water capacity slider is not assigned.");
        }

        if (refillButton != null)
        {
            refillButton.onClick.AddListener(RefillWater);
        }
        else
        {
            Debug.LogWarning("Refill button is not assigned.");
        }

        if (refillPanel != null)
        {
            refillPanel.SetActive(false); // Ensure the panel is initially inactive
        }
        else
        {
            Debug.LogWarning("Refill panel is not assigned.");
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
            Debug.Log("Fire pipe state set to: " + state);
            UpdateWaterButtonState();
        }
        else
        {
            Debug.LogWarning("Fire pipe or fire gun is not assigned.");
        }
    }

    public void TogglePipeState()
    {
        isFirePipeActive = !isFirePipeActive;
        SetPipeActive(isFirePipeActive);
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
            Debug.LogWarning("Water object is not assigned.");
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
            Debug.Log("Water state set to: " + state);
            if (state) // If activating water, reduce the capacity
            {
                StartCoroutine(ConsumeWater());
            }
        }
        else
        {
            Debug.LogWarning("Water object is not assigned.");
        }
    }

    /// <summary>
    /// Coroutine to consume water over time.
    /// </summary>
    private IEnumerator ConsumeWater()
    {
        while (IsWaterActive() && currentWaterCapacity > 0)
        {
            currentWaterCapacity -= 1f; // Adjust this value to control the water consumption rate
            UpdateWaterSlider();
            if (currentWaterCapacity <= 0)
            {
                SetWaterActive(false);
                waterButton.SetActive(false);
                Debug.Log("Water capacity reached zero. Deactivating water.");
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
        currentWaterCapacity = maxWaterCapacity;
        UpdateWaterSlider();
        Debug.Log("Water refilled to maximum capacity.");

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
        if (other.CompareTag("Action"))
        {
            if (fireExtinguisherButton != null)
            {
                fireExtinguisherButton.SetActive(true);
                Debug.Log("Fire extinguisher button activated.");
            }
        }
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
                Debug.Log("Fire extinguisher button deactivated.");
            }
        }
    }
}
