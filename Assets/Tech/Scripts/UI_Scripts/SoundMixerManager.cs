using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using System.Collections.Generic;
public class SoundMixerManager : MonoBehaviour
{
    private AudioMixer audioMixer;
    private VisualElement rootVis;
    private List<Slider> sliders = new List<Slider>();
    float nextSFXtime;

    void Start()
    {
        audioMixer = Resources.Load<AudioMixer>("Sounds/MainMixer");
        rootVis = GetComponent<UIDocument>().rootVisualElement;
        sliders = rootVis.Query<Slider>("SettingsSlider").ToList();
        foreach(var slider in sliders)
        {
            slider.RegisterCallback<ChangeEvent<float>>(SetVolume);
            AudioSliderSetting(slider);
        }
    }
    public void AudioSliderSetting(Slider slider)
    {
        float percent = slider.value / 100f;
        percent = Mathf.Clamp(percent, 0.0001f, 1f);
        float decibel = Mathf.Log10(percent)* 20f;
        
        switch (slider.tooltip)
        {
            case "Master":
                audioMixer.SetFloat("masterVolume", decibel);
                break;
            case "Music":
                audioMixer.SetFloat("musicVolume", decibel);
                break;
            case "Environment":
                audioMixer.SetFloat("soundscapeVolume", decibel);
                break;
            case "SoundEffects":
                audioMixer.SetFloat("sfxVolume", decibel);
                break;
        }
    }
    public void SetVolume(ChangeEvent<float> evt)
    {
        
        Slider slider = (Slider) evt.target;
        AudioSliderSetting(slider);
        PlaySliderSound();
    }
    public void PlaySliderSound()
    {
        if(nextSFXtime < Time.unscaledTime)
        {
           SFXManager.PlayEffect("SliderSound");
           nextSFXtime = Time.unscaledTime + 0.03f; 
        }
    }
}
