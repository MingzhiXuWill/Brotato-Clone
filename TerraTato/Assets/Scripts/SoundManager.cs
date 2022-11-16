using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager sndman;

    private AudioSource audioSource;

    public AudioClip FireSounds;

    public AudioClip HurtSounds;

    public AudioClip KilledSounds;

    public float PitchFix;

    public float DefaultVolume;

    void Start()
    {
        sndman = this;
        audioSource = GetComponent<AudioSource>();
        ResetVolume();
    }

    private void Update()
    {
        audioSource.pitch = Random.Range(1f - PitchFix, 1f + PitchFix);
    }

    public void ResetVolume() {
        audioSource.volume = DefaultVolume;
    }

    public void SetVolume(float volume) {
        audioSource.volume = volume;
    }

    public void PlayFireSounds()
    {
        //SetVolume(0.1f);
        audioSource.PlayOneShot(FireSounds);
        //ResetVolume();
    }

    public void PlayHurtSounds() 
    {
        audioSource.PlayOneShot(HurtSounds);
    }

    public void PlayKilledSounds()
    {
        audioSource.PlayOneShot(KilledSounds);
    }
}