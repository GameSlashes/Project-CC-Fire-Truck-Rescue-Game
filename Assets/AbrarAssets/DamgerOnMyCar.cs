using Invector;
using UnityEngine;

public class DamgerOnMyCar : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f; // Maximum health value
    public float currentHealth { get; private set; } // Expose current health as read-only

    // Reference to the damage system
    private DamageSystem damageSystem;

    private void Start()
    {
        currentHealth = maxHealth;
        // Locate the DamageSystem (assumes it's in the scene and manages all vehicles)
        damageSystem = FindObjectOfType<DamageSystem>(true);
        if (damageSystem != null)
        {
            damageSystem.SetCarDamageSystem(this); // Notify the DamageSystem of this car
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= 5f) // Adjust threshold as necessary
        {
            ApplyDamage(collision.relativeVelocity.magnitude); // Apply damage based on collision velocity
        }
    }

    public void ApplyDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateDamageGauge();

        // Inform the DamageSystem of the health update
        if (damageSystem != null)
        {
            damageSystem.UpdateDamageGauge(currentHealth, maxHealth);
        }
    }

    public void RepairDamage(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateDamageGauge();

        if (damageSystem != null)
        {
            damageSystem.UpdateDamageGauge(currentHealth, maxHealth);
        }
    }

    private void UpdateDamageGauge()
    {
        Debug.Log("Health: " + currentHealth + "/" + maxHealth);
        // This function can later be expanded to update visuals on the car
    }
}
