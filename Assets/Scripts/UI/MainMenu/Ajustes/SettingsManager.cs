using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    [Header("Audio")]
    public float volume = 0.5f;
    [Header("Brightness")]
    public Image panelBrillo;
    public float valueBrillo;
    [Header("FullScreen")]
    public LogicalFullScreen logicalFullScreen;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadSettings();
    }
    void LoadSettings()
    {
        volume = PlayerPrefs.GetFloat("volumen", 0.5f);
        AudioListener.volume = volume;
        valueBrillo = PlayerPrefs.GetFloat("brillo", 0.5f);
        panelBrillo.color = new Color(panelBrillo.color.r, panelBrillo.color.g, panelBrillo.color.b, (1 - valueBrillo) / 2);
    }

    public void SetVolume(float value)
    {
        volume = value;
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("volumen", value);
    }
    public void SetBrightness(float value)
    {
        valueBrillo = value;
        panelBrillo.color = new Color(panelBrillo.color.r, panelBrillo.color.g, panelBrillo.color.b, (1 - valueBrillo) / 2);
        PlayerPrefs.SetFloat("brillo", value);
    }
    public void SetFullScreen(bool value)
    {
        Screen.fullScreen = value;
        PlayerPrefs.SetInt("fullscreen", value ? 1 : 0);
    }
}
