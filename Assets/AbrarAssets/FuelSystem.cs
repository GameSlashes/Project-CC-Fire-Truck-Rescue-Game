using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem.XR;

public class FuelSystem : MonoBehaviour
{
    /// <summary>
    /// Main car controller.
    /// </summary>
    private RCCP_CarController carController;

    [Tooltip("Fuel gauge image")]
    public Image fuelGauge;

    [Tooltip("Fuel panel UI")]
    public GameObject fuelPanel;

    [Tooltip("Refill with coins button")]
    public Button refillWithCoinsButton;

    [Tooltip("Refill by watching an ad button")]
    public Button refillWithAdButton;

    [Tooltip("Text that shows when there are not enough coins.")]
    public Text notEnoughCoinsText;

    [Tooltip("Filler image for smooth refilling effect.")]
    public Image fuelFiller;

    // Amount of fuel to refill (expressed as a fraction of the total fuel tank).
    public float refillAmount = 0.5f; // 50% of the fuel tank
    public float refillSpeed = 0.2f;  // Speed at which the fuel refills visually

    private void Start()
    {
        carController = RCCP_SceneManager.Instance.activePlayerVehicle;

        // Attach listeners to the refill buttons
        if (refillWithCoinsButton != null)
            refillWithCoinsButton.onClick.AddListener(RefillFuelWithCoins);

        if (refillWithAdButton != null)
            refillWithAdButton.onClick.AddListener(RefillFuelWithAd);

        // Initially hide the "Not Enough Coins" text
        notEnoughCoinsText.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        UpdateFuelGauge();
        UpdateFiller();
        CheckFuelLevel();
    }

    // Check if the fuel level has reached 0 and show the fuel panel
    private void CheckFuelLevel()
    {
        if (carController != null && carController.OtherAddonsManager?.FuelTank != null)
        {
            if (carController.OtherAddonsManager.FuelTank.fuelTankFillAmount <= 0f)
            {
                fuelPanel.SetActive(true);  // Show the fuel panel
            }
            else
            {
                fuelPanel.SetActive(false); // Hide the fuel panel if there's fuel
            }
        }
        else
        {
            fuelPanel.SetActive(false); // Hide the fuel panel if no fuel tank exists
        }
    }


    private void UpdateFuelGauge()
    {
        if (carController != null && carController.OtherAddonsManager?.FuelTank != null)
        {
            fuelGauge.fillAmount = carController.OtherAddonsManager.FuelTank.fuelTankFillAmount;
        }
        else
        {
            fuelGauge.fillAmount = 0f; // Set the gauge to empty if no fuel tank exists.
        }
    }

    // Refill fuel using coins
    private void RefillFuelWithCoins()
    {
        if (SaveData.instance.Coins >= 1000)
        {
            Refill(refillAmount); // Refill fuel with the specified amount
            SaveData.instance.Coins -= 1000; // Deduct coins after refilling
            notEnoughCoinsText.gameObject.SetActive(false); // Hide warning text
        }
        else
        {
            // Show "Not Enough Coins" warning
            notEnoughCoinsText.gameObject.SetActive(true);
        }
    }

    // Refill fuel by watching an ad
    private void RefillFuelWithAd()
    {
        var handler = FindObjectOfType<Handler>();
        if (handler)
        {
            handler.ShowRewardedAdsBoth(() => Refill(refillAmount)); // Use a lambda to call Refill after ad completion
        }
    }

    // Method to refill the fuel tank
    private void Refill(float amount)
    {
        if (carController?.OtherAddonsManager?.FuelTank != null)
        {
            carController.Engine.StartEngine();
            carController.OtherAddonsManager.FuelTank.fuelTankCapacity = refillSpeed;
            //StartCoroutine(SmoothRefill(amount));
        }
    }

    // Coroutine for smooth refilling of the fuel gauge
    private IEnumerator SmoothRefill(float amount)
    {
        float targetAmount = Mathf.Clamp(carController.OtherAddonsManager.FuelTank.fuelTankFillAmount + amount, 0f, 1f);

        while (carController.OtherAddonsManager.FuelTank.fuelTankFillAmount < targetAmount)
        {
            carController.OtherAddonsManager.FuelTank.fuelTankFillAmount += Time.deltaTime * refillSpeed;
            carController.OtherAddonsManager.FuelTank.fuelTankFillAmount = Mathf.Clamp(carController.OtherAddonsManager.FuelTank.fuelTankFillAmount, 0f, targetAmount);
            yield return null;
        }
    }

    // Smoothly update the fuel filler image (optional)
    private void UpdateFiller()
    {
        if (fuelFiller != null && carController?.OtherAddonsManager?.FuelTank != null)
        {
            fuelFiller.fillAmount = carController.OtherAddonsManager.FuelTank.fuelTankFillAmount;
        }
    }
}
