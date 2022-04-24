using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScreenOverlay : MonoBehaviour
{
    public System.Action<ScreenType> OnScreenFinished;
    public ScreenType screenType;

    // Start is called before the first frame update
    void Awake()
    {
        ScreenManager.instance.RegisterScreen(screenType, this);
        gameObject.SetActive(false);
        Init();
    }
    protected virtual void Init() { }
    public virtual void RunEffect() { }
}
