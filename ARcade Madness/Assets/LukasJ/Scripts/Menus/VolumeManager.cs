using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public AudioSource musicSource, neonSoundsSource, countdownSoundsSource;

    public Slider musicSlider, soundsSlider;

    void Start()
    {
        if(!PlayerPrefs.HasKey("MusicVol"))
        {
            PlayerPrefs.SetFloat("MusicVol", 0.5f);
        }

        if (!PlayerPrefs.HasKey("SoundsVol"))
        {
            PlayerPrefs.SetFloat("SoundsVol", 0.5f);
        }

        musicSource.volume = PlayerPrefs.GetFloat("MusicVol");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol");

        countdownSoundsSource.volume = PlayerPrefs.GetFloat("SoundsVol");
        neonSoundsSource.volume = PlayerPrefs.GetFloat("SoundsVol");
        soundsSlider.value = PlayerPrefs.GetFloat("SoundsVol");

        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        soundsSlider.onValueChanged.AddListener(ChangeSoundsVolume);
    }
    
    void ChangeMusicVolume(float value)
    {
        musicSource.volume = value;
        float musicValue = value;
        PlayerPrefs.SetFloat("MusicVol", musicValue);
    }

    void ChangeSoundsVolume(float value)
    {
        countdownSoundsSource.volume = value;
        neonSoundsSource.volume = value;
        float soundsValue = value;
        PlayerPrefs.SetFloat("SoundsVol", soundsValue);
    }
}
