using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Clips")]
    public AudioClip buttonClickSound;

    [Header("Audio Sources")]
    public AudioSource soundSource;

    [Header("Background Sounds")]
    public GameObject mainMenuSound;
    public GameObject gamePlaySound;

    [Header("Event Sounds")]
    public GameObject siren;
    public GameObject missionComplete;
    public GameObject missionFail;
    public GameObject panelSlider;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        PlayMainMenuSound();
    }

    /// <summary>
    /// Plays the provided sound clip for button clicks.
    /// </summary>
    /// <param name="sound">The sound clip to play.</param>
    public void PlayButtonClickSound(AudioClip sound)
    {
        if (soundSource != null && sound != null)
        {
            soundSource.PlayOneShot(sound);
        }
    }

    /// <summary>
    /// Activates the main menu background sound and deactivates gameplay sound.
    /// </summary>
    public void PlayMainMenuSound()
    {
        SetBackgroundSound(mainMenuSound, gamePlaySound);
    }

    /// <summary>
    /// Activates the gameplay background sound and deactivates main menu sound.
    /// </summary>
    public void PlayGamePlaySound()
    {
        SetBackgroundSound(gamePlaySound, mainMenuSound);
    }

    /// <summary>
    /// Handles sound for mission completion.
    /// </summary>
    public void MissionCompleteOn()
    {
        missionComplete.SetActive(true);
    }  
    /// <summary>
    /// Handles sound for mission completion.
    /// </summary>
    public void MissionCompleteOff()
    {
        missionComplete.SetActive(false);
    }

    /// <summary>
    /// Handles sound for mission failure.
    /// </summary>
    public void MissionFailOn()
    {
        missionFail.SetActive(true);
    }   
    /// <summary>
    /// Handles sound for mission failure.
    /// </summary>
    public void MissionFailOff()
    {
        missionFail.SetActive(false);
    }

    /// <summary>
    /// Turns the siren sound on.
    /// </summary>
    public void SirenOn()
    {
        siren.SetActive(true);
    }

    /// <summary>
    /// Opens the panel slider sound.
    /// </summary>
    public void PanelOpen()
    {
        panelSlider.SetActive(true);
    }  
    /// <summary>
    /// Opens the panel slider sound.
    /// </summary>
    public void PanelOff()
    {
        panelSlider.SetActive(false);
    }

    /// <summary>
    /// Turns the siren sound off.
    /// </summary>
    public void SirenOff()
    {
        siren.SetActive(false);
    }

    /// <summary>
    /// Helper method to set the active background sound.
    /// </summary>
    /// <param name="toActivate">The GameObject to activate.</param>
    /// <param name="toDeactivate">The GameObject to deactivate.</param>
    private void SetBackgroundSound(GameObject toActivate, GameObject toDeactivate)
    {
        if (toActivate != null)
        {
            toActivate.SetActive(true);
        }

        if (toDeactivate != null)
        {
            toDeactivate.SetActive(false);
        }
    }
}
