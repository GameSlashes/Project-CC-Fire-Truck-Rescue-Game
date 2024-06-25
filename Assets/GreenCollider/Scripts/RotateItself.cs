
using UnityEngine;

public class RotateItself : MonoBehaviour
{
    public static RotateItself instance;
    public Transform player; // Assign the player's transform in the Inspector
    public float updateInterval = 2.0f; // Adjust the interval based on your needs
    bool player_Triggered;
    private void Start()
    {
        instance = this;
        // Start invoking the UpdateRotation method at regular intervals
        InvokeRepeating("UpdateRotation", 0f, updateInterval);
       

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            player_Triggered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player_Triggered = false;
        }
    }
    private void UpdateRotation()
    {
        if (player != null && player_Triggered != true)
        {

            // Get the target's position and only use the Y component
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);

            // Calculate the rotation towards the target on the Y-axis only
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);

            // Apply the rotation to the GameObject
            transform.rotation = targetRotation;
        }
    }
}
