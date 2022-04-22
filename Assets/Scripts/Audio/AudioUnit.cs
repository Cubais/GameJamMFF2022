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
}
