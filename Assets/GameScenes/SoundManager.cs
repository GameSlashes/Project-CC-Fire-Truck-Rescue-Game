using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip buttonMainSound;
    public AudioSource soundSource;

    public GameObject mainMenuSound;
    public GameObject gamePlaySound;

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

    public void Start()
    {
        playMainMenuSound();
    }

    public void onButtonClickSound(AudioClip Sound)
    {
        soundSource.PlayOneShot(Sound);
    }

    public void playMainMenuSound()
    {
        mainMenuSound.SetActive(true);
        gamePlaySound.SetActive(false);
    }

    public void playGamePlaySound()
    {
        mainMenuSound.SetActive(false);
        gamePlaySound.SetActive(true);
    }
}
