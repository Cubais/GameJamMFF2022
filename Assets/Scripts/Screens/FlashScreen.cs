using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashScreen : ScreenOverlay
{
    private Image image;

    protected override void Init()
    {
        image = GetComponent<Image>();    
    }

    public override void RunEffect()
    {
        StartCoroutine(FadeOutScreen());
    }

    private IEnumerator FadeOutScreen()
    {        
        var color = image.color;

        color.a = 1f;
        image.color = color;

        var alpha = 1f;

        while (alpha > 0f)
        {
            color.a = alpha;
            image.color = color;

            alpha -= Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        OnScreenFinished(screenType);
    }
}
