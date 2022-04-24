using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUnit : MonoBehaviour
{
    public System.Action<AudioUnit> OnSoundFinished;
    private AudioSource source;
        
    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
    }

    public void Init()
    {
        if (!source)
            source = GetComponent<AudioSource>();

        source.clip = null;
        source.loop = false;
        source.playOnAwake = false;

        StopAllCoroutines();
    }

    public void Play(AudioClip soundToPlay, bool loop = false)
    {
        source.volume = 1.0f;
        source.clip = soundToPlay;
        source.loop = loop;

        if (!loop)
            StartCoroutine(ReturnAfterFinishAsync(soundToPlay.length));

        source.Play();
    }

    private IEnumerator ReturnAfterFinishAsync(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        OnSoundFinished(this);
    }

    public void StopSound(bool fadeOut = false)
    {
        if (fadeOut)
            StartCoroutine(FadeOutSound());
        else
            source.Stop();
    }
    private IEnumerator FadeOutSound()
    {
        while(source.volume > 0.0f)
        {
            source.volume -= 0.005f;
            yield return null;
        }
        source.Stop();

        OnSoundFinished(this);
    }
}
