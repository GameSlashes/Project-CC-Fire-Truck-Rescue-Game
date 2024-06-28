using UnityEngine;
using UnityEngine.UI;

public class OnTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            ParticalsEmissionReduction emissionReduction = other.GetComponent<ParticalsEmissionReduction>();
            if (emissionReduction != null)
            {
                Image fillerImage = emissionReduction.filler.GetComponent<Image>();
                fillerImage.fillAmount -= emissionReduction.hSliderValue_1;

                Color colorAdjustment = new Color(emissionReduction.hSliderValue, emissionReduction.hSliderValue, emissionReduction.hSliderValue, emissionReduction.hSliderValue) * Time.deltaTime;

                foreach (ParticleSystem ps in emissionReduction.ps)
                {
                    var mainModule = ps.main;
                    mainModule.startColor = new ParticleSystem.MinMaxGradient(mainModule.startColor.color + colorAdjustment);
                }

                if (fillerImage.fillAmount <= 0)
                {
                    emissionReduction.OnFillerComplete();
                }
            }
        }
    }
}
