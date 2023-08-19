using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingsManager : MonoBehaviour
{
    [SerializeField] private SetFullScreen setFullScreen;
    [SerializeField] private ColorSlider colorSlider;
    [SerializeField] private CrosshairDrop crosshairDrop;
    [SerializeField] private SenVolSettings SensSlider;
    [SerializeField] private SenVolSettings musicSlider;
    [SerializeField] private SenVolSettings soundSlider;

    private void Start()
    {
        LoadSettings();
    }

    public void SaveSettings()
    {
        bool isFullScreen = setFullScreen.IsFullScreen;
        float redValue = colorSlider.sliderRed.value;
        float greenValue = colorSlider.sliderGreen.value;
        float blueValue = colorSlider.sliderBlue.value;
        int crosshairIndex = crosshairDrop.CHDD.value;
        float sensitivityValue = SensSlider.sensSlider.value;
        float musicValue = musicSlider.musicSlider.value;
        float soundValue = soundSlider.soundSlider.value;

        PlayerPrefs.SetInt("IsFullScreen", isFullScreen ? 1 : 0);
        PlayerPrefs.SetFloat("RedValue", redValue);
        PlayerPrefs.SetFloat("GreenValue", greenValue);
        PlayerPrefs.SetFloat("BlueValue", blueValue);
        PlayerPrefs.SetInt("CrosshairIndex", crosshairIndex);
        PlayerPrefs.SetFloat("SensitivityValue", sensitivityValue);
        PlayerPrefs.SetFloat("MusicValue", musicValue);
        PlayerPrefs.SetFloat("SoundValue", soundValue);

        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {

        bool isFullScreen = PlayerPrefs.GetInt("IsFullScreen", 1) == 1;
        float redValue = PlayerPrefs.GetFloat("RedValue", 1f);
        float greenValue = PlayerPrefs.GetFloat("GreenValue", 1f);
        float blueValue = PlayerPrefs.GetFloat("BlueValue", 1f);
        int crosshairIndex = PlayerPrefs.GetInt("CrosshairIndex", 0);
        float sensitivityValue = PlayerPrefs.GetFloat("SensitivityValue", 1f);
        float musicValue = PlayerPrefs.GetFloat("MusicValue", 1f);
        float soundValue = PlayerPrefs.GetFloat("SoundValue", 1f);


        setFullScreen.FullsScreen(isFullScreen);
        colorSlider.sliderRed.value = redValue;
        colorSlider.sliderGreen.value = greenValue;
        colorSlider.sliderBlue.value = blueValue;
        crosshairDrop.CHDD.value = crosshairIndex;
        SensSlider.sensSlider.value = sensitivityValue;
        musicSlider.musicSlider.value = musicValue;
        soundSlider.soundSlider.value = soundValue;
    }
    public void OnSaveClick()
    {
        SaveSettings();
    }

    public void OnLoadClick()
    {
        LoadSettings();
    }

}
