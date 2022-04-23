using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Impact = 0,
    Melee = 1,
    Range = 2
}

public enum CharacterType
{
    Player = 0,
    Reporter = 1,
    Soldier = 2,
    Alien = 3
}

public class AudioManager : Singleton<AudioManager>
{
    public GameObject AudioUnitPrefab;

    private Stack<AudioUnit> freeAudioUnits = new Stack<AudioUnit>();
    private Dictionary<CharacterType, CharacterSounds[]> soundDatabase = new Dictionary<CharacterType, CharacterSounds[]>();
    
    private void Awake()
    {
        LoadSoundDatabase();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            AudioListener.volume = (AudioListener.volume == 1f) ? 0f : 1f;
        }   
    }

    public void Prewarm(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var newAudioUnit = Instantiate(AudioUnitPrefab, transform).GetComponent<AudioUnit>();
            newAudioUnit.Init();            
            freeAudioUnits.Push(newAudioUnit);
        }
    }

    public AudioUnit Play(CharacterType characterType, SoundType soundType, bool loop = false)
    {
        var audioClipToPlay = soundDatabase[characterType][(int)soundType].audioClips[0];
        var audioUnit = Pull();

        audioUnit.Play(audioClipToPlay, loop);

        return audioUnit;
    }

    public AudioUnit Play(AudioClip clipToPlay, bool loop = false)
    {        
        var audioUnit = Pull();

        audioUnit.Play(clipToPlay, loop);

        return audioUnit;
    }


    private AudioUnit Pull()
    {
        AudioUnit pulledUnit;

        if (freeAudioUnits.Count != 0)
            pulledUnit = freeAudioUnits.Pop();
        else
        {
            pulledUnit = Instantiate(AudioUnitPrefab, transform).GetComponent<AudioUnit>();
        }

        pulledUnit.Init();
        pulledUnit.OnSoundFinished += ReturnToPool;

        return pulledUnit;
    }

    private void ReturnToPool(AudioUnit audioUnit)
    {
        audioUnit.Init();
        audioUnit.OnSoundFinished -= ReturnToPool;
        freeAudioUnits.Push(audioUnit);
    }

    private void LoadSoundDatabase()
    {
        for (int i = 0; i < 4; i++)
        {
            var listSounds = new CharacterSounds[3];          
            soundDatabase.Add((CharacterType)i, listSounds);
        }

        var characterSoundsData = Resources.LoadAll<CharacterSounds>("Sounds");
        foreach (var soundData in characterSoundsData)
        {
            soundDatabase[soundData.typeOfCharacter][(int)soundData.typeOfSound] = soundData;
        }
    }
}
