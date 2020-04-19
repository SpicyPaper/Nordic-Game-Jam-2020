using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip audioClipDay;
    [SerializeField] private AudioClip audioClipNight;
    [SerializeField] private AudioClip audioClipBloop1;
    [SerializeField] private AudioClip audioClipBloop2;
    [SerializeField] private AudioClip audioClipFireball;

    [SerializeField] private AudioSource mainAudioSource;
    [SerializeField] private AudioSource soundEffectAudioSource;
    [SerializeField] private AudioSource soundEffect2AudioSource;

    private void OnEnable()
    {
        DayNightManager.OnStartDayEvent += StartDay;
        DayNightManager.OnStartNightEvent += StartNight;
        IsometricPlayerMovementController.OnElementCollectedEvent += Bloop;
        IsometricPlayerMovementController.OnShootFireballEvent += Shoot;
    }

    private void OnDisable()
    {
        DayNightManager.OnStartDayEvent -= StartDay;
        DayNightManager.OnStartNightEvent -= StartNight;
        IsometricPlayerMovementController.OnElementCollectedEvent -= Bloop;
        IsometricPlayerMovementController.OnShootFireballEvent -= Shoot;
    }

    private void Shoot()
    {
        soundEffect2AudioSource.clip = audioClipFireball;
        soundEffect2AudioSource.Play();
    }

    private void Bloop()
    {
        int rand = Random.Range(0, 2);

        if (rand == 0)
        {
            soundEffectAudioSource.clip = audioClipBloop1;
        }
        else
        {
            soundEffectAudioSource.clip = audioClipBloop2;
        }
        soundEffectAudioSource.Play();
    }

    private void StartDay()
    {
        mainAudioSource.Stop();
        mainAudioSource.clip = audioClipDay;
        mainAudioSource.Play();
    }

    private void StartNight()
    {
        mainAudioSource.Stop();
        mainAudioSource.clip = audioClipNight;
        mainAudioSource.Play();
    }
}
