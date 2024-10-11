using Invector;
using UnityEngine;
using UnityEngine.UI;

public class DamageSystem : MonoBehaviour
{
    private DamgerOnMyCar currentCarDamageSystem;

    [Tooltip("Damage gauge image")]
    public Image damageGauge;

    [Tooltip("Damage panel UI")]
    public GameObject damagePanel;

    [Tooltip("Repair with coins button")]
    public Button repairWithCoinsButton;

    [Tooltip("Repair by watching an ad button")]
    public Button repairWithAdButton;

    public float repairAmount = 0.5f;

    private void Start()
    {
        // Attach listeners to the repair buttons
        if (repairWithCoinsButton != null)
            repairWithCoinsButton.onClick.AddListener(RepairWithCoins);

        if (repairWithAdButton != null)
            repairWithAdButton.onClick.AddListener(RepairWithAd);
    }

    // Set the current car's damage system to be managed
    public void SetCarDamageSystem(DamgerOnMyCar carDamageSystem)
    {
        currentCarDamageSystem = carDamageSystem;
        UpdateDamageGauge(currentCarDamageSystem.currentHealth, currentCarDamageSystem.maxHealth); // Initialize the gauge
    }

    // Update the damage gauge based on the car's health
    public void UpdateDamageGauge(float currentHealth, float maxHealth)
    {
        if (damageGauge != null)
        {
            damageGauge.fillAmount = 1 - (currentHealth / maxHealth); // Invert the fill amount for damage
        }
    }

    // Repair the car using coins
    private void RepairWithCoins()
    {
        if (SaveData.instance.Coins >= 100 && currentCarDamageSystem != null)
        {
            currentCarDamageSystem.RepairDamage(repairAmount * currentCarDamageSystem.maxHealth);
            SaveData.instance.Coins -= 100;
        }
    }

    // Repair the car by watching an ad
    private void RepairWithAd()
    {
        var handler = FindObjectOfType<Handler>();
        if (handler && currentCarDamageSystem != null)
        {
            handler.ShowRewardedAdsBoth(() => currentCarDamageSystem.RepairDamage(repairAmount * currentCarDamageSystem.maxHealth));
        }
    }
}
