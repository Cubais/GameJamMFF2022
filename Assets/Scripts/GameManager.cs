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
    private bool inMenu = false;
    private int currentScreenEdge = 0;

    private void Awake()
    {
        playerCharacter = Instantiate(PlayerPrefab, PlayerStartPos.position, Quaternion.identity).GetComponent<CharacterControler>();
        playerCharacter.gameObject.SetActive(false);

        currentScreenEdge = LevelSetup.DesertLevel.Count + 1;
        BackgroundManager.instance.Init(LevelSetup);
        AudioManager.instance.Prewarm(5);
        currentAmbientMusic = AudioManager.instance.Play(desertMusicAmbient, true);        
    }

    private void Start()
    {
        CameraMovement.instance.SetCameraEdge(0, currentScreenEdge * BackgroundManager.instance.screenWidth);
        ScreenManager.instance.SetScreen(ScreenType.Loading, ScreenType.Menu);
        NPCManager.instance.SpawnMenuNPCs();
        inMenu = true;
    }

    private void Update()
    {
        if (inMenu)
            return;

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
        if (playerCharacter.transform.position.x / BackgroundManager.instance.screenWidth > currentScreenEdge && NPCManager.instance.LevelNPCCount == 0)
        {
            print("Switch " + BackgroundManager.instance.CurrentLevel);
            switch (BackgroundManager.instance.CurrentLevel)
            {
                case BackgroundType.Desert:
                    BackgroundManager.instance.ChangebackgroundType(BackgroundType.Hangar);
                    CameraMovement.instance.SetCameraEdge(currentScreenEdge * BackgroundManager.instance.screenWidth, 
                                                          (currentScreenEdge + LevelSetup.HangarLevel.Count + 1) * BackgroundManager.instance.screenWidth);

                    BackgroundManager.instance.MoveSideBorders(currentScreenEdge - 1, currentScreenEdge + LevelSetup.HangarLevel.Count);

                    currentScreenEdge += LevelSetup.HangarLevel.Count + 1;                    
                    break;
                case BackgroundType.Hangar:
                    BackgroundManager.instance.ChangebackgroundType(BackgroundType.Lab);
                    CameraMovement.instance.SetCameraEdge(currentScreenEdge * BackgroundManager.instance.screenWidth,
                                                         (currentScreenEdge + LevelSetup.LabLevel.Count) * BackgroundManager.instance.screenWidth);

                    BackgroundManager.instance.MoveSideBorders(currentScreenEdge - 1, currentScreenEdge - 1 + LevelSetup.LabLevel.Count);

                    currentScreenEdge += LevelSetup.LabLevel.Count;
                    break;
                case BackgroundType.Lab:
                    CameraMovement.instance.SetCameraEdge((currentScreenEdge) * BackgroundManager.instance.screenWidth,
                                                         (currentScreenEdge) * BackgroundManager.instance.screenWidth);

                    BackgroundManager.instance.MoveSideBorders(currentScreenEdge - 1, currentScreenEdge - 1);
                    currentScreenEdge += 100;
                    break;
                default:
                    break;
            }
        }
    }

    public void StartGame()
    {
        playerCharacter.gameObject.SetActive(true);
        NPCManager.instance.StartGame(LevelSetup);
        inMenu = false;
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
        inMenu = true;
        ResetGame();
    }

    private void ResetGame()
    {
        // Reset player
        Destroy(playerCharacter.gameObject);
        playerCharacter = Instantiate(PlayerPrefab, PlayerStartPos.position, Quaternion.identity).GetComponent<CharacterControler>();
        playerCharacter.gameObject.SetActive(false);

        CameraMovement.instance.ResetCamera();

        NPCManager.instance.SpawnMenuNPCs();
    }
}
