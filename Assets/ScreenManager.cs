using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ScreenType
{
    Flash
}

public class ScreenManager : Singleton<ScreenManager>
{
    Dictionary<ScreenType, GameObject> screenUIs = new Dictionary<ScreenType, GameObject>();

    private void Update()
    {
        
    }

    public void RegisterScreen(ScreenType screenType, GameObject go)
    {
        if (!screenUIs.ContainsKey(screenType))
            screenUIs.Add(screenType, go);
    }

    private IEnumerator FadeOutScreen(ScreenType screenType)
    {
        var imageFlash = screenUIs[screenType].GetComponent<Image>();
        var color = imageFlash.color;

        color.a = 1f;
        imageFlash.color = color;

        var alpha = 1f;      
        
        while (alpha > 0f)
        {
            color.a = alpha;
            imageFlash.color = color;

            alpha -= Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
    }

    public void FlashScreen()
    {
        StartCoroutine(FadeOutScreen(ScreenType.Flash));
    }
}
