using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchVideoToGetReward : MonoBehaviour
{
    [SerializeField] private int rewardCoins = 100; // Coins reward for watching video
    [SerializeField] private Text congratulationText; // Reference to congratulation message UI
    [SerializeField] private float messageDisplayDuration = 3f; // Time in seconds to display the message
    [SerializeField] private float typewriterSpeed = 0.05f; // Time delay between each character
    private Button rewardButton; // Reference to the button triggering reward
    private Handler adHandler; // Cached reference to ad handler

    private void Start()
    {
        // Cache Handler instance to avoid frequent FindObjectOfType calls
        adHandler = FindObjectOfType<Handler>();

        if (adHandler == null)
        {
            Debug.LogError("Handler not found in the scene! Ads will not work.");
        }

        // Optionally, cache and link button action if attached to the GameObject
        rewardButton = GetComponent<Button>();
        if (rewardButton != null)
        {
            rewardButton.onClick.AddListener(GiveReward);
        }

        // Ensure congratulationText is hidden at start
        if (congratulationText != null)
        {
            congratulationText.gameObject.SetActive(false);
        }
    }

    public void GiveReward()
    {
        if (adHandler != null)
        {
            // Show rewarded ad and pass a callback for reward logic
            adHandler.ShowRewardedAdsBoth(OnRewardedVideoComplete);
            LogRewardEvent(); // Log Firebase analytics event
        }
        else
        {
            Debug.LogWarning("Ad Handler is not available! Cannot show ads.");
        }
    }

    private void OnRewardedVideoComplete()
    {
        // Add reward coins to the player's balance
        SaveData.instance.Coins += rewardCoins;
        Debug.Log($"Player rewarded with {rewardCoins} coins!");

        // Display congratulatory message with typewriter effect, including the number of coins
        if (congratulationText != null)
        {
            congratulationText.gameObject.SetActive(true);
            string message = $"Congratulations! You've earned {rewardCoins} coins!";
            StartCoroutine(TypeWriterEffect(message));
        }
    }

    private IEnumerator TypeWriterEffect(string message)
    {
        congratulationText.text = ""; // Clear the text before starting the effect

        foreach (char letter in message.ToCharArray())
        {
            congratulationText.text += letter; // Add one letter at a time
            yield return new WaitForSeconds(typewriterSpeed); // Delay for the typewriter effect
        }

        // Start a coroutine to hide the message after a delay once the typing effect is complete
        StartCoroutine(HideCongratulationTextAfterDelay());
    }

    private IEnumerator HideCongratulationTextAfterDelay()
    {
        // Wait for the specified duration before hiding the congratulation text
        yield return new WaitForSeconds(messageDisplayDuration);

        // Hide the congratulation message
        congratulationText.gameObject.SetActive(false);
    }

    private void LogRewardEvent()
    {
        // Log the reward video event in Firebase Analytics
        Firebase.Analytics.FirebaseAnalytics.LogEvent("RewardVideoCoins");
        Debug.Log("Firebase: RewardVideoCoins event logged.");
    }
}
