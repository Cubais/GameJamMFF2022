using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Level settings")]
    [SerializeField] private LevelSettings LevelSetup;

    [Header("Player info")]
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private Transform PlayerStartPos;

    [Header("Ambient sounds")]
    [SerializeField] private AudioClip desertMusicAmbient;
    
    public CharacterControler playerCharacter { get; private set; }
    public bool GamePaused => gamePaused;

    private AudioUnit currentAmbientMusic;
    private bool gamePaused = false;
    private int currentScreenEdge = 0;

    private void Awake()
    {
        playerCharacter = Instantiate(PlayerPrefab, PlayerStartPos.position, Quaternion.identity).GetComponent<CharacterControler>();
        playerCharacter.gameObject.SetActive(false);

        currentScreenEdge = LevelSetup.DesertCount + 1;
        BackgroundManager.instance.Init(LevelSetup);
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

        CheckLevelPass();        
    }

    private void CheckLevelPass()
    {
        if (playerCharacter.transform.position.x / BackgroundManager.instance.screenWidth > currentScreenEdge)
        {
            CameraMovement.instance.SetCameraEdge(currentScreenEdge * BackgroundManager.instance.screenWidth);
            //MoveBorders();
            switch (BackgroundManager.instance.CurrentLevel)
            {
                case BackgroundType.Desert:
                    BackgroundManager.instance.ChangebackgroundType(BackgroundType.Hangar);
                    currentScreenEdge += LevelSetup.HangarCount;
                    break;
                case BackgroundType.Hangar:
                    BackgroundManager.instance.ChangebackgroundType(BackgroundType.Lab);
                    currentScreenEdge += LevelSetup.LabCount;
                    break;
                case BackgroundType.Lab:
                    break;
                default:
                    break;
            }
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

        CameraMovement.instance.ResetCamera();
    }
}
