using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSounds", menuName = "Audio/CharacterSounds", order = 1)]
public class CharacterSounds : ScriptableObject
{
    [Header("Sound type")]
    public CharacterType typeOfCharacter;
    public SoundType typeOfSound;

    [Header("Data")]
    public List<AudioClip> audioClips;
}
