using UnityEngine;

public class GraphicSetting : MonoBehaviour
{

    [Header("Graphics")]
    public GameObject[] lowGraphic;
    public GameObject[] mediumGraphic;
    public GameObject[] highGraphic;


    private void OnEnable()
    {
        loadGraphic();
    }

    public void acticveLowGraphics()
    {
        lowGraphic[0].SetActive(true);
        lowGraphic[1].SetActive(false);
        mediumGraphic[0].SetActive(false);
        mediumGraphic[1].SetActive(true);
        highGraphic[0].SetActive(false);
        highGraphic[0].SetActive(true);
    }
    public void acticveMediumGraphics()
    {
        lowGraphic[0].SetActive(false);
        lowGraphic[1].SetActive(true);
        mediumGraphic[0].SetActive(true);
        mediumGraphic[1].SetActive(false);
        highGraphic[0].SetActive(true);
        highGraphic[0].SetActive(false);
    }
    public void acticveHighGraphics()
    {
        lowGraphic[0].SetActive(false);
        lowGraphic[1].SetActive(true);
        mediumGraphic[0].SetActive(false);
        mediumGraphic[1].SetActive(true);
        highGraphic[0].SetActive(true);
        highGraphic[1].SetActive(false);
    }

    public void updateGraphics(int graphicID)
    {
        if (graphicID == 0)
        {
            QualitySettings.SetQualityLevel(1);
            PlayerPrefs.SetString("QualityLevel", "Low");

            if (SoundManager.instance)
                SoundManager.instance.onButtonClickSound(SoundManager.instance.buttonMainSound);

            acticveLowGraphics();
        }
        else if (graphicID == 1)
        {
            QualitySettings.SetQualityLevel(3);
            PlayerPrefs.SetString("QualityLevel", "Medium");
            if (SoundManager.instance)
                SoundManager.instance.onButtonClickSound(SoundManager.instance.buttonMainSound);
            acticveMediumGraphics();


        }
        else if (graphicID == 2)
        {
            QualitySettings.SetQualityLevel(4);
            PlayerPrefs.SetString("QualityLevel", "High");
            if (SoundManager.instance)
                SoundManager.instance.onButtonClickSound(SoundManager.instance.buttonMainSound);
            acticveHighGraphics();


        }
    }
    public void loadGraphic()
    {
        if (PlayerPrefs.HasKey("QualityLevel"))
        {
            if (PlayerPrefs.GetString("QualityLevel") == "Low")
            {
                QualitySettings.SetQualityLevel(1);
                acticveLowGraphics();
            }
            else if (PlayerPrefs.GetString("QualityLevel") == "Medium")
            {
                QualitySettings.SetQualityLevel(3);
                acticveMediumGraphics();


            }
            else if (PlayerPrefs.GetString("QualityLevel") == "High")
            {
                QualitySettings.SetQualityLevel(4);
                acticveHighGraphics();


            }
        }
        else if (!PlayerPrefs.HasKey("QualityLevel"))
        {
            updateGraphics(2);
        }
    }
}
