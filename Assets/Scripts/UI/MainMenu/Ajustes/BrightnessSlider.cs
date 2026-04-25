using UnityEngine;
using UnityEngine.UI;

public class BrightnessSlider : MonoBehaviour
{
    public Image imagenValueZero;
    void Start()
    {
        if (imagenValueZero != null)
        {
            imagenValueZero.enabled = false;
        }
    }
    public void ChangeBrightness(float value)
    {
        SettingsManager.instance.SetBrightness(value);
        LookIfDark(value);
    }
    private void LookIfDark(float sliderValue)
    {
        if (sliderValue == 0)
        {
            imagenValueZero.enabled = true;
        }
        else
        {
            imagenValueZero.enabled = false;
        }
    }
}
