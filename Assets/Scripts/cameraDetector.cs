using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraDetector : MonoBehaviour
{
    public int speed;
    public Color[] colors;

    public GameObject captureEffect;
    public playerLook player;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13)
        {
            // Cache references and speed value for better performance
            var carController = player.playerObject.gameObject.transform.parent?.parent?.GetComponent<RCCP_CarController>();

            if (carController != null)
            {
                float currentSpeed = carController.speed;  // Cache speed value
                int speedInt = (int)currentSpeed;

                // Show speed captured UI element
                GameManager.instance.uiElements.speedCheckObject.SetActive(true);

                if (currentSpeed >= speed)
                {
                    // Display success message
                    GameManager.instance.uiElements.speedCheckText.text = $"Speed Captured: {speedInt} km/h";
                    GameManager.instance.uiElements.speedCheckText.color = colors[1];  // Use correct color
                }
                else
                {
                    // Display failure message for low speed
                    GameManager.instance.uiElements.speedCheckText.text = $"Too Low Speed Captured: {speedInt} km/h";
                    GameManager.instance.uiElements.speedCheckText.color = colors[0];  // Use different color
                }

                // Activate capture effect
                captureEffect.SetActive(true);
            }
            else
            {
                Debug.LogError("RCCP_CarController component not found!");
            }
        }
    }

}
