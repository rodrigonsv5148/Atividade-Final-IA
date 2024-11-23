using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public Slider volumeSlider;
    private string allVolume = "MasterVolume";

    void Start()
    {
        volumeSlider = FindAnyObjectByType<Slider>();

        float currentVolume;

        if(volumeSlider != null) 
        {
            if (AudioManager.instance.audioMixer.GetFloat(allVolume, out currentVolume))
            {
                volumeSlider.value = Mathf.Pow(10, currentVolume / 20);
            }
            else
            {
                UnityEngine.Debug.LogError($"Parameter {allVolume} not found in AudioMixer");
            }
            volumeSlider.onValueChanged.AddListener(SetVolumeFromSlider);
        }
    }

    public void SetVolume(float volume)
    {
        if (volume <= 0.0001f)
        {
            volume = 0.0001f;
        }

        float dB = Mathf.Log10(volume) * 20;
        AudioManager.instance.audioMixer.SetFloat(allVolume, dB);
    }

    public void SetVolumeFromSlider(float volume)
    {
        SetVolume(volume);
    }
}
