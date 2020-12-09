using UnityEngine;

public class AudioManagerSpinner : MonoBehaviour
{
    public AudioSource backgroundTheme, generalSource;

    public AudioClip death, scoreboard;

    void Start()
    {
        SetVolume();
    }

    private void SetVolume()
    {
        backgroundTheme.volume = PlayerPrefs.GetFloat("MusicVol");

        generalSource.volume = PlayerPrefs.GetFloat("SoundsVol");
    }

    public void PlayDeathSound()
    {
        generalSource.PlayOneShot(death);
    }

    public void PlayScoreboardSound()
    {
        generalSource.PlayOneShot(scoreboard);
    }
}