using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindow : WindowBase
{
    public Slider bgmSlider;
    public Slider soundSlider;
    public Slider txtPopSpeedSlider;
    public Toggle lyricsToggle;
    public Toggle autoPopToggle;
    public Slider txtPopAwaitSlider;

    protected override void OnAwake()
    {
        base.OnAwake();
        titleText.text = "Settings";
        bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
        txtPopSpeedSlider.onValueChanged.AddListener(OnPopSpeedSliderChanged);
        txtPopAwaitSlider.onValueChanged.AddListener(OnPopAwaitSliderChanged);
        lyricsToggle.onValueChanged.AddListener(OnLrcToggleChanged);
        autoPopToggle.onValueChanged.AddListener(OnPopToggleChanged);

        LoadConfig();
    }


    void OnBgmSliderChanged(float value)
    {
        
    }

    void OnSoundSliderChanged(float value)
    {

    }

    void OnPopSpeedSliderChanged(float value)
    {

    }

    void OnPopAwaitSliderChanged(float value)
    {

    }


    void OnLrcToggleChanged(bool value)
    {

    }

    void OnPopToggleChanged(bool value)
    {

    }

    protected override void Close()
    {
        SaveConfig();
        base.Close();
    }

    void LoadConfig()
    {
        
    }

    void SaveConfig()
    {
        Debug.Log("Save Config");
    }
}
