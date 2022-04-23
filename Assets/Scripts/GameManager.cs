using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Player info")]
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private Transform PlayerStartPos;

    [Header("Ambient sounds")]
    [SerializeField] private AudioClip desertMusicAmbient;

    public GameObject playerCharacter { get; private set; }

    private AudioUnit currentAmbientMusic;

    private void Awake()
    {
        BackgroundManager.instance.Init();
        AudioManager.instance.Prewarm(5);
        currentAmbientMusic = AudioManager.instance.Play(desertMusicAmbient, true);

        playerCharacter = Instantiate(PlayerPrefab, PlayerStartPos.position, Quaternion.identity);
    }
}
