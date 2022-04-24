using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Player info")]
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private Transform PlayerStartPos;

    [Header("Ambient sounds")]
    [SerializeField] private AudioClip desertMusicAmbient;
    
    public CharacterControler playerCharacter { get; private set; }
    public bool GamePaused => gamePaused;

    private AudioUnit currentAmbientMusic;
    private bool gamePaused = false;

    private void Awake()
    {
        playerCharacter = Instantiate(PlayerPrefab, PlayerStartPos.position, Quaternion.identity).GetComponent<CharacterControler>();
        playerCharacter.gameObject.SetActive(false);

        BackgroundManager.instance.Init();
        AudioManager.instance.Prewarm(5);
        currentAmbientMusic = AudioManager.instance.Play(desertMusicAmbient, true);        
    }

    private void Start()
    {
        ScreenManager.instance.SetScreen(ScreenType.Loading, ScreenType.Menu);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
                OnContinue();
            else
                OnPauseGame();
        }
    }

    public void StartGame()
    {
        playerCharacter.gameObject.SetActive(true);
    }

    public void OnContinue()
    {
        Time.timeScale = 1f;
        ScreenManager.instance.SetScreen(ScreenType.World);
    }

    public void OnPauseGame()
    {
        Time.timeScale = 0f;
        ScreenManager.instance.SetScreen(ScreenType.Pause);
    }

    public void OnBackToMenu()
    {
        Time.timeScale = 1f;
        ScreenManager.instance.SetScreen(ScreenType.Loading, ScreenType.Menu);
        ResetGame();
    }

    private void ResetGame()
    {
        // Reset player
        Destroy(playerCharacter.gameObject);
        playerCharacter = Instantiate(PlayerPrefab, PlayerStartPos.position, Quaternion.identity).GetComponent<CharacterControler>();
        playerCharacter.gameObject.SetActive(false);
    }
}
