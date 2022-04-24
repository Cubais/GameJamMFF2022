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
}
