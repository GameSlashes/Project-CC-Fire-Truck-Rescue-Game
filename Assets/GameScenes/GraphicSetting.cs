using UnityEngine;

public class GraphicSetting : MonoBehaviour
{
    [Header("Graphics")]
    public GameObject[] lowGraphic;
    public GameObject[] mediumGraphic;
    public GameObject[] highGraphic;

    private void OnEnable()
    {
        LoadGraphics();
    }

    public void ActivateLowGraphics()
    {
        SetGraphicsState(lowGraphic, true);
        SetGraphicsState(mediumGraphic, false);
        SetGraphicsState(highGraphic, false);
    }

    public void ActivateMediumGraphics()
    {
        SetGraphicsState(lowGraphic, false);
        SetGraphicsState(mediumGraphic, true);
        SetGraphicsState(highGraphic, false);
    }

    public void ActivateHighGraphics()
    {
        SetGraphicsState(lowGraphic, false);
        SetGraphicsState(mediumGraphic, false);
        SetGraphicsState(highGraphic, true);
    }

    public void SetGraphicsQuality(int qualityID)
    {
        string qualityLevel;

        switch (qualityID)
        {
            case 0:
                QualitySettings.SetQualityLevel(0); // Use appropriate index based on your quality levels
                qualityLevel = "Low";
                ActivateLowGraphics();
                break;
            case 1:
                QualitySettings.SetQualityLevel(2); // Use appropriate index based on your quality levels
                qualityLevel = "Medium";
                ActivateMediumGraphics();
                break;
            case 2:
                QualitySettings.SetQualityLevel(4); // Use appropriate index based on your quality levels
                qualityLevel = "High";
                ActivateHighGraphics();
                break;
            default:
                qualityLevel = "High";
                break;
        }

        PlayerPrefs.SetString("QualityLevel", qualityLevel);

        if (SoundManager.instance)
        {
            SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound);
        }
    }

    private void LoadGraphics()
    {
        if (PlayerPrefs.HasKey("QualityLevel"))
        {
            string qualityLevel = PlayerPrefs.GetString("QualityLevel");

            switch (qualityLevel)
            {
                case "Low":
                    SetGraphicsQuality(0);
                    break;
                case "Medium":
                    SetGraphicsQuality(1);
                    break;
                case "High":
                    SetGraphicsQuality(2);
                    break;
                default:
                    SetGraphicsQuality(2); // Default to High if an unknown value is found
                    break;
            }
        }
        else
        {
            SetGraphicsQuality(2); // Default to High if no key is found
        }
    }

    private void SetGraphicsState(GameObject[] graphics, bool state)
    {
        foreach (var graphic in graphics)
        {
            graphic.SetActive(state);
        }
    }
}
