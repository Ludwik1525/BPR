﻿using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundTheme, rocket, generalSource;

    public AudioClip diceRoll, coinSteal, trophy, chestSpawn, scoreboard;

    void Start()
    {
        SetVolume();
    }
    
    private void SetVolume()
    {
        backgroundTheme.volume = PlayerPrefs.GetFloat("MusicVol");
        
        rocket.volume = PlayerPrefs.GetFloat("SoundsVol");
        generalSource.volume = PlayerPrefs.GetFloat("SoundsVol");

        TurnOffRocketSounds();
    }

    public void TurnOffRocketSounds()
    {
        rocket.gameObject.SetActive(false);
    }

    public void TurnOnRocketSounds()
    {
        rocket.gameObject.SetActive(true);
    }

    public void PlayDiceRollSound()
    {
        generalSource.PlayOneShot(diceRoll);
    }

    public void PlayCoinStealSound()
    {
        generalSource.PlayOneShot(coinSteal);
    }

    public void PlayTrophySound()
    {
        generalSource.PlayOneShot(trophy);
    }
    
    public void PlayChestSpawnSound()
    {
        generalSource.PlayOneShot(chestSpawn);
    }

    public void PlayScoreboardSound()
    {
        generalSource.PlayOneShot(scoreboard);
    }
}
