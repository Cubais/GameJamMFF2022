using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : ScreenOverlay
{

    public override void RunEffect()
    {
        base.RunEffect();
    }

    public void OnContinue()
    {
        GameManager.instance.OnContinue();
    }

    public void OnBackToMenu()
    {
        GameManager.instance.OnBackToMenu();
    }
}
