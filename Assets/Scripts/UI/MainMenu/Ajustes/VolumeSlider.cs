using UnityEngine;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
    public Image imagenMute;

    void Start()
    {
        if (imagenMute != null)
        {
            imagenMute.enabled = false;
        }
    }
    public void ChangeVolume(float value)
    {
        SettingsManager.instance.SetVolume(value);
        LookIfMute(value);
    }
    private void LookIfMute(float sliderValue)
    {
        if (sliderValue == 0)
        {
            imagenMute.enabled = true;
        }
        else
        {
            imagenMute.enabled = false;
        }
    }
}
