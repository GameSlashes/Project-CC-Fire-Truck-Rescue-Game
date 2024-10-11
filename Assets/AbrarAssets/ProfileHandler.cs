using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileHandler : MonoBehaviour
{
    [Header("Profile")]
    public GameObject inputNameField; // Reference for the player's name input field
    public GameObject inputAgeField;  // Reference for the player's age input field
    public Image[] playerIcons;
    public Image profileIcon;
    public GameObject profilePanel;
    public GameObject mainMenu;
    public Text playerNameText;
    public Text playerAgeText; // Text for displaying player's age
    public Image playerIconImage;

    [Header("StoresData")]
    public Image storePlayerIcon;

    [Header("SettingData")]
    public Image settingPlayerIcon;

    private const int MinNameLength = 4;
    private const int MaxNameLength = 12;
    private const int MinAge = 10;
    private const int MaxAge = 100;

    public Button setObjects; // Button to activate when valid input is provided

    void Start()
    {
        InitializeProfile();
        inputNameField.GetComponent<InputField>().onValueChanged.AddListener(delegate { ValidateInput(); });
        inputAgeField.GetComponent<InputField>().onValueChanged.AddListener(delegate { ValidateAge(); });
        setObjects.gameObject.SetActive(true); // Set button inactive initially
    }

    private void InitializeProfile()
    {
        if (!PlayerPrefs.HasKey("userName"))
        {
            profilePanel.SetActive(true);
            PlayerPrefs.SetInt("PlayerIcon", 0);
            PlayerPrefs.SetString("userName", "Player Name");
            PlayerPrefs.SetInt("userAge", 0); // Default age
        }

        UpdateProfileUI();
    }

    private void UpdateProfileUI()
    {
        SetPlayerName(playerNameText);
        SetPlayerAge(playerAgeText);
        SetPlayerIcon(playerIconImage);
        SetPlayerIcon(storePlayerIcon);
        SetPlayerIcon(settingPlayerIcon);
    }

    public void GetPlayerIcon(int playerNum)
    {
        PlayerPrefs.SetInt("PlayerIcon", playerNum);
        profileIcon.sprite = playerIcons[PlayerPrefs.GetInt("PlayerIcon")].sprite;
        UpdateProfileUI();
        Firebase.Analytics.FirebaseAnalytics.LogEvent("PlayerIconChange");
    }

    public void GetName()
    {
        string enteredName = inputNameField.GetComponent<InputField>().text;
        PlayerPrefs.SetString("userName", enteredName);
    }

    public void GetAge()
    {
        int enteredAge = int.Parse(inputAgeField.GetComponent<InputField>().text);
        PlayerPrefs.SetInt("userAge", enteredAge);
    }

    private void SetPlayerName(Text playerName)
    {
        playerName.text = PlayerPrefs.GetString("userName");
    }

    private void SetPlayerAge(Text playerAge)
    {
        playerAge.text = PlayerPrefs.GetInt("userAge").ToString();
    }

    private void SetLevelText(Text levelText)
    {
        int num = PlayerPrefs.GetInt("LevelsDone", 0) + 1;
        levelText.text = "LV." + num.ToString();
    }

    private void SetPlayerIcon(Image userImage)
    {
        int playerIconIndex = PlayerPrefs.GetInt("PlayerIcon");
        userImage.sprite = playerIcons[playerIconIndex].sprite;
    }

    private void SetTotalScores(Text scores)
    {
        scores.text = PlayerPrefs.GetInt("LevelPoints").ToString();
    }

    private void ValidateInput()
    {
        string input = inputNameField.GetComponent<InputField>().text;

        if (input.Length < MinNameLength || input.Length > MaxNameLength)
        {
            inputNameField.GetComponent<Image>().color = Color.red; // Set input field color to red if invalid
        }
        else
        {
            inputNameField.GetComponent<Image>().color = Color.white; // Reset color if valid
        }

        CheckValidation(); // Check if both fields are valid
    }

    private void ValidateAge()
    {
        string ageInput = inputAgeField.GetComponent<InputField>().text;

        if (int.TryParse(ageInput, out int age))
        {
            if (age < MinAge || age > MaxAge)
            {
                inputAgeField.GetComponent<Image>().color = Color.red; // Set input field color to red if invalid
            }
            else
            {
                inputAgeField.GetComponent<Image>().color = Color.white; // Reset color if valid
            }
        }
        else
        {
            inputAgeField.GetComponent<Image>().color = Color.red; // Invalid input (non-numeric)
        }

        CheckValidation(); // Check if both fields are valid
    }

    private void CheckValidation()
    {
        string inputName = inputNameField.GetComponent<InputField>().text;
        string inputAge = inputAgeField.GetComponent<InputField>().text;

        bool isNameValid = inputName.Length >= MinNameLength && inputName.Length <= MaxNameLength;
        bool isAgeValid = int.TryParse(inputAge, out int age) && age >= MinAge && age <= MaxAge;

        // If both name and age are valid, activate the setObjects button
        //if (isNameValid && isAgeValid)
        //{
        //    setObjects.gameObject.SetActive(true);
        //}
        //else
        //{
        //    setObjects.gameObject.SetActive(false);
        //}
    }

    public void OnBackButtonPressed()
    {
        if (FindObjectOfType<Handler>())
        {
            FindObjectOfType<Handler>().showWaitInterstitial();
            PlayerPrefs.SetInt("loadInterstitialAD", 5);
            PlayerPrefs.SetInt("adShowMore", 1);
        }

        InputField nameInputField = inputNameField.GetComponent<InputField>();
        InputField ageInputField = inputAgeField.GetComponent<InputField>();

        string inputName = nameInputField.text;
        string inputAge = ageInputField.text;

        bool isNameValid = inputName.Length >= MinNameLength && inputName.Length <= MaxNameLength;
        bool isAgeValid = int.TryParse(inputAge, out int age) && age >= MinAge && age <= MaxAge;

        // Validate both name and age
        if (isNameValid && isAgeValid)
        {
            GetName(); // Save the valid name
            GetAge();  // Save the valid age

            // Log valid input event
            Firebase.Analytics.FirebaseAnalytics.LogEvent("Profiler_ValidInput", new Firebase.Analytics.Parameter[]
            {
            new Firebase.Analytics.Parameter("user_name", inputName),
            new Firebase.Analytics.Parameter("user_age", age),
            new Firebase.Analytics.Parameter("validation_status", "valid")
            });
        }
        else
        {
            // Log invalid input event (defaults applied)
            Firebase.Analytics.FirebaseAnalytics.LogEvent("Profiler_InvalidInput", new Firebase.Analytics.Parameter[]
            {
            new Firebase.Analytics.Parameter("entered_name", inputName),
            new Firebase.Analytics.Parameter("entered_age", inputAge),
            new Firebase.Analytics.Parameter("validation_status", "invalid")
            });

            // Set a default dynamic name and age if validation fails
            if (!isNameValid)
            {
                Debug.Log("Name is invalid. Setting dynamic name.");
                nameInputField.text = "Player_" + Random.Range(1000, 9999).ToString(); // Dynamic name
                GetName(); // Save dynamic name
            }

            if (!isAgeValid)
            {
                Debug.Log("Age is invalid. Setting default age.");
                ageInputField.text = "18"; // Default age
                GetAge(); // Save default age
            }

            // Log the defaults that were applied
            Firebase.Analytics.FirebaseAnalytics.LogEvent("Profiler_DefaultsApplied", new Firebase.Analytics.Parameter[]
            {
            new Firebase.Analytics.Parameter("dynamic_name", nameInputField.text),
            new Firebase.Analytics.Parameter("default_age", ageInputField.text)
            });
        }

        UpdateProfileUI(); // Update the UI with the saved values
        mainMenu.SetActive(true);
        profilePanel.SetActive(false);

        // Set PlayerPrefs so this block won't execute again
        PlayerPrefs.SetInt("HasOpenedPlayerProfiler", 1);
        PlayerPrefs.Save(); // Save PlayerPrefs immediately

        Firebase.Analytics.FirebaseAnalytics.LogEvent("Profiler_UseEnter");
    }


}
