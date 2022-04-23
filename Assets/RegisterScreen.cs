using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterScreen : MonoBehaviour
{
    public ScreenType screenType;

    // Start is called before the first frame update
    void Start()
    {
        ScreenManager.instance.RegisterScreen(screenType, gameObject);
    }
}
