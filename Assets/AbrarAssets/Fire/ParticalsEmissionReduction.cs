using UnityEngine;
using UnityEngine.UI;
public class ParticalsEmissionReduction : MonoBehaviour
{
    public static ParticalsEmissionReduction istance;
    public ParticleSystem[] ps;
    public float hSliderValue = 1.0f;
    public float hSliderValue_1 = 1.0f;
    public Image filler;
    public void OnEnable()
    {
        istance = this;
    }
    [System.Obsolete]
    public void OnFillerComplete()
    {
        ps[0].startColor = new Color(100f, 100f, 100f);
        ps[1].startColor = new Color(100f, 100f, 100f);
        ps[2].startColor = new Color(100f, 100f, 100f);
    }
    [System.Obsolete]
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("water"))
        {
            ps[0].startColor -= new Color(hSliderValue, hSliderValue, hSliderValue, hSliderValue) * Time.deltaTime;
            ps[1].startColor -= new Color(hSliderValue, hSliderValue, hSliderValue, hSliderValue) * Time.deltaTime;
            ps[2].startColor -= new Color(hSliderValue, hSliderValue, hSliderValue, hSliderValue) * Time.deltaTime;
        }
    }
}
