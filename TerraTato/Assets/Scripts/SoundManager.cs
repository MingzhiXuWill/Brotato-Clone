using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager sndman;

    private AudioSource audioSource;

    public AudioClip FireSounds;

    public AudioClip HurtSounds;

    void Start()
    {
        sndman = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFireSounds()
    {
        audioSource.PlayOneShot(FireSounds);
    }

    public void PlayHurtSounds()
    {
        audioSource.PlayOneShot(HurtSounds);
    }
}