using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsScreen : ScreenOverlay
{
    public override void RunEffect()
    {
        StartCoroutine(Randopm());
    }

    private IEnumerator Randopm()
    {
        yield return new WaitForSeconds(3f);
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        OnScreenFinished(screenType);
    }
}
