using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public AudioSource musicSource, neonSoundsSource, countdownSoundsSource;

    public Slider musicSlider, soundsSlider;


    void Start()
    {
        // checking if the player has any settings saved
        if(!PlayerPrefs.HasKey("MusicVol"))
        {
            PlayerPrefs.SetFloat("MusicVol", 0.5f);
        }

        if (!PlayerPrefs.HasKey("SoundsVol"))
        {
            PlayerPrefs.SetFloat("SoundsVol", 0.5f);
        }

        // getting all the values
        musicSource.volume = PlayerPrefs.GetFloat("MusicVol");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol");

        countdownSoundsSource.volume = PlayerPrefs.GetFloat("SoundsVol");
        neonSoundsSource.volume = PlayerPrefs.GetFloat("SoundsVol");
        soundsSlider.value = PlayerPrefs.GetFloat("SoundsVol");

        // assigning listeners for volume sliders
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        soundsSlider.onValueChanged.AddListener(ChangeSoundsVolume);
    }
    
    // for changing music volume
    void ChangeMusicVolume(float value)
    {
        musicSource.volume = value;
        float musicValue = value;
        PlayerPrefs.SetFloat("MusicVol", musicValue);
    }

    // for changing sounds volume
    void ChangeSoundsVolume(float value)
    {
        countdownSoundsSource.volume = value;
        neonSoundsSource.volume = value;
        float soundsValue = value;
        PlayerPrefs.SetFloat("SoundsVol", soundsValue);
    }
}
