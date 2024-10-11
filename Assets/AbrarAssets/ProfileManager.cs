using Invector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance; // Singleton instance to ensure the profile persists across scenes.

    [Header("Profile Settings")]
    public string playerName; // Stores player's name
    public int playerAge;     // Stores player's age
    public int playerIconIndex; // Stores player's selected icon index

    public Image profileIcon; // Reference to the icon displayed on the profile UI
    public Text playerNameText; // Reference to display the player's name
    public Text playerAgeText; // Reference to display the player's age
    public Image[] playerIcons; // Array of possible player icons

    private const string PlayerNameKey = "PlayerName";
    private const string PlayerAgeKey = "PlayerAge";
    private const string PlayerIconKey = "PlayerIcon";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keeps this object active across all scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadPlayerProfile(); // Load player profile when the game starts
    }

    // Call this method when the player updates their profile data
    public void SetPlayerProfile(string name, int age, int iconIndex)
    {
        playerName = name;
        playerAge = age;
        playerIconIndex = iconIndex;

        // Save to PlayerPrefs for persistence
        PlayerPrefs.SetString(PlayerNameKey, playerName);
        PlayerPrefs.SetInt(PlayerAgeKey, playerAge);
        PlayerPrefs.SetInt(PlayerIconKey, playerIconIndex);
        PlayerPrefs.Save();

        // Update UI elements
        UpdateProfileUI();
    }

    private void LoadPlayerProfile()
    {
        // Load player data from PlayerPrefs, or set default values if they don't exist
        playerName = PlayerPrefs.GetString(PlayerNameKey, "Player Name");
        playerAge = PlayerPrefs.GetInt(PlayerAgeKey, 18); // Default age is 18
        playerIconIndex = PlayerPrefs.GetInt(PlayerIconKey, 0); // Default icon is the first one

        // Update the UI with the loaded profile data
        UpdateProfileUI();
    }

    // Updates the profile UI elements
    public void UpdateProfileUI()
    {
        if (playerNameText != null)
            playerNameText.text = playerName;

        if (playerAgeText != null)
            playerAgeText.text = playerAge.ToString();

        if (profileIcon != null && playerIcons.Length > 0)
            profileIcon.sprite = playerIcons[playerIconIndex].sprite;
    }

    // Use this method to clear profile data if necessary
    public void ClearProfile()
    {
        PlayerPrefs.DeleteKey(PlayerNameKey);
        PlayerPrefs.DeleteKey(PlayerAgeKey);
        PlayerPrefs.DeleteKey(PlayerIconKey);
        PlayerPrefs.Save();
    }
}
