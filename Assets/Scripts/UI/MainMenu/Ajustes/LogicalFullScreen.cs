using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using TMPro;
using System;

public class LogicalFullScreen : MonoBehaviour
{
    public Toggle toggle;
    public TMP_Dropdown tMP_Dropdown;
    Resolution[] resolutions;
    void Start()
    {
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
        LookResolution();
    }

    public void ActiveFULLS(bool fullscreen)
    {
        SettingsManager.instance.SetFullScreen(fullscreen);
    }

    public void LookResolution()
    {
        resolutions = Screen.resolutions;
        tMP_Dropdown.ClearOptions();
        List<String> listOptions = new List<String>();
        int resoltuionNow = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            listOptions.Add(option);

            if (Screen.fullScreen &&
                resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                resoltuionNow = i;
            }
        }
        tMP_Dropdown.AddOptions(listOptions);
        tMP_Dropdown.value = resoltuionNow;
        tMP_Dropdown.RefreshShownValue();

        tMP_Dropdown.value = PlayerPrefs.GetInt("numeroResolucion", 0);
    }
    public void ChangeResolution(int idxResolution)
    {
        PlayerPrefs.SetInt("numeroResolucion", tMP_Dropdown.value);

        Resolution resolution = resolutions[idxResolution];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
