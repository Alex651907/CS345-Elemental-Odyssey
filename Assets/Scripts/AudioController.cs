using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource soundEffects;
    public AudioClip jumpAudio;
    public AudioClip deathAudio;
    public AudioClip waterDeathAudio;
    public AudioClip gameOverAudio;
    public AudioClip powerUpCollectAudio;
    public AudioClip levelCompletedAudio;

    public void playJump()
    {
        soundEffects.PlayOneShot(jumpAudio);
    }

    public void playDeathWater()
    {
        soundEffects.PlayOneShot(waterDeathAudio);
    }

    public void playDeath()
    {
        soundEffects.PlayOneShot(deathAudio);
    }

    public void playGameOver()
    {
        soundEffects.PlayOneShot(gameOverAudio);
    }

    public void playPowerUpCollect()
    {
        soundEffects.PlayOneShot(powerUpCollectAudio);
    }

    public void playLevelComplete()
    {
        soundEffects.PlayOneShot(levelCompletedAudio);
    }
}
