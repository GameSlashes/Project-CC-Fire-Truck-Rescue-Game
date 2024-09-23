using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [Header("Settings")]
    public Slider musicSlider;
    public Slider soundSlider;

    private void Start()
    {
        InitializeSoundSettings();
    }

    void InitializeSoundSettings()
    {
        musicSlider.onValueChanged.AddListener(delegate { AdjustMusicVolume(); });
        soundSlider.onValueChanged.AddListener(delegate { AdjustSoundVolume(); });

        LoadSoundSettings();
    }

    void LoadSoundSettings()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            musicSlider.value = 1.0f;
            PlayerPrefs.SetFloat("musicVolume", 1.0f);
        }

        if (PlayerPrefs.HasKey("soundVolume"))
        {
            soundSlider.value = PlayerPrefs.GetFloat("soundVolume");
        }
        else
        {
            soundSlider.value = 1.0f;
            PlayerPrefs.SetFloat("soundVolume", 1.0f);
        }

        AdjustMusicVolume();
        AdjustSoundVolume();
    }

    public void AdjustMusicVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.Save();
        ApplyMusicVolume();
        playBtnSound();
    }

    public void AdjustSoundVolume()
    {
        PlayerPrefs.SetFloat("soundVolume", soundSlider.value);
        PlayerPrefs.Save();
        ApplySoundVolume();
        playBtnSound();
    }

    void ApplyMusicVolume()
    {
        // Adjusting the volume for all sound components in a single loop
        AudioSource[] audioSources = {
        SoundManager.instance.mainMenuSound.GetComponent<AudioSource>(),
        SoundManager.instance.gamePlaySound.GetComponent<AudioSource>(),
        SoundManager.instance.siren.GetComponent<AudioSource>(),
        SoundManager.instance.missionComplete.GetComponent<AudioSource>(),
        SoundManager.instance.missionFail.GetComponent<AudioSource>(),
        SoundManager.instance.panelSlider.GetComponent<AudioSource>()
    };

        foreach (AudioSource source in audioSources)
        {
            source.volume = musicSlider.value;
        }
    }


    void ApplySoundVolume()
    {
        // Apply the sound volume in your game
        // Adjust your sound effects volume here, if different from AudioListener
        // For now, assuming it affects AudioListener volume
        AudioListener.volume = soundSlider.value;
    }

    public void playBtnSound()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound);
        }
    }
}
