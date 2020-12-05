using UnityEngine;

public class AudioManagerPacman : MonoBehaviour
{
    public AudioSource backgroundTheme, generalSource;

    public AudioClip coinCollect, death, scoreboard;

    void Start()
    {
        SetVolume();
    }

    private void SetVolume()
    {
        backgroundTheme.volume = PlayerPrefs.GetFloat("MusicVol");
        
        generalSource.volume = PlayerPrefs.GetFloat("SoundsVol");
    }

    public void PlayCoinCollectSound()
    {
        generalSource.PlayOneShot(coinCollect);
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

