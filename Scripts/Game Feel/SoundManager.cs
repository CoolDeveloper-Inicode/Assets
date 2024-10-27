using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip swordSwingSFX;
    public AudioClip hitSwordSFX;
    public AudioClip parrySFX;
    public AudioClip rollSFX;

    [Header("Audio Source")]
    public AudioSource audioSource;
    public AudioSource lowVolumeAudioSource;

    public void PlayTargetSound(AudioSource targetSource, AudioClip targetSFX)
    {
        targetSource.pitch = Random.Range(0.9f, 1.1f);
        targetSource.PlayOneShot(targetSFX);
    }
}
