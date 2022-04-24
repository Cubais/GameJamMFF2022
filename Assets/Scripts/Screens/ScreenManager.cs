using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ScreenType
{
    Flash,
    Menu,
    Win,
    Dead,
    Pause,
    Loading,
    World,
    Controls,
    None
}

public class ScreenManager : Singleton<ScreenManager>
{
    Dictionary<ScreenType, ScreenOverlay> screenUIs = new Dictionary<ScreenType, ScreenOverlay>();

    private ScreenType activeScreen;
    private ScreenType nextScreen;

    private void Update()
    {
      
    }

    private void ScreenFinished(ScreenType screenType)
    {
        CloseAllScreens();
        if (nextScreen != ScreenType.None)
            SetScreen(nextScreen);
        else
            activeScreen = ScreenType.None;

        switch (screenType)
        {            
            case ScreenType.Menu:
                GameManager.instance.StartGame();
                SetScreen(ScreenType.World);
                break;
            case ScreenType.Win:
                GameManager.instance.OnBackToMenu();
                break;
            case ScreenType.Dead:
                GameManager.instance.OnBackToMenu();
                break;
            case ScreenType.Pause:
                break;
            case ScreenType.Loading:
                break;
            case ScreenType.None:
                break;
            default:
                break;
        }
    }

    private void CloseAllScreens()
    {
        foreach (var screen in screenUIs.Values)
        {
            screen.gameObject.SetActive(false);
        }
    }

    public void RegisterScreen(ScreenType screenType, ScreenOverlay screen)
    {
        if (!screenUIs.ContainsKey(screenType))
        {
            screenUIs.Add(screenType, screen);
            screen.OnScreenFinished += ScreenFinished;            
        }
    }    

    public void SetScreen(ScreenType currentScreen, ScreenType nextScreen = ScreenType.None)
    {
        if (activeScreen != ScreenType.None)
            screenUIs[activeScreen].gameObject.SetActive(false);

        this.nextScreen = nextScreen;
        activeScreen = currentScreen;

        screenUIs[currentScreen].gameObject.SetActive(true);
        screenUIs[currentScreen].RunEffect();
    }

    public void SetCharacterHealth(float currentHealth, float radio)
    {
        var worldScreen = screenUIs[ScreenType.World] as WorldScreen;
        worldScreen.CharacterHealth.SetSliderValue(currentHealth);
        worldScreen.RadioHealth.SetSliderValue(radio);
    }

    public void SetBossHealth(float currentHealth)
    {
        var worldScreen = screenUIs[ScreenType.World] as WorldScreen;
        worldScreen.BossHealth.SetSliderValue(currentHealth);        
    }

    public void ShowBossHealth(bool show)
    {
        var worldScreen = screenUIs[ScreenType.World] as WorldScreen;
        worldScreen.BossUI.SetActive(show);
    }
}
