using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : ScreenOverlay
{
    protected override void Init()
    {
        
    }

    public override void RunEffect()
    {
        base.RunEffect();
    }

    public void OnPlay()
    {
        OnScreenFinished(screenType);        
    }

    public void OnQuitApp()
    {
        Application.Quit();
    }

    public void OnControls()
    {
        ScreenManager.instance.SetScreen(ScreenType.Controls, ScreenType.Menu);
    }
}
