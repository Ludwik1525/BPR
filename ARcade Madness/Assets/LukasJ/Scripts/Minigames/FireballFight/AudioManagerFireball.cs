using UnityEngine;

public class AudioManagerFireball : MonoBehaviour
{
    public AudioSource backgroundTheme, shield, generalSource;

    public AudioClip fireballThrow, fireballExplode, death, scoreboard;

    void Start()
    {
        SetVolume();
    }

    private void SetVolume()
    {
        backgroundTheme.volume = PlayerPrefs.GetFloat("MusicVol");

        shield.volume = PlayerPrefs.GetFloat("SoundsVol");
        generalSource.volume = PlayerPrefs.GetFloat("SoundsVol");

        TurnOffShieldSounds();
    }

    public void TurnOffShieldSounds()
    {
        shield.gameObject.SetActive(false);
    }

    public void TurnOnShieldSounds()
    {
        shield.gameObject.SetActive(true);
    }

    public void PlayFireballThrowSound()
    {
        generalSource.PlayOneShot(fireballThrow);
    }

    public void PlayFireballExplodeSound()
    {
        generalSource.PlayOneShot(fireballExplode);
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
