using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScreen : ScreenOverlay
{
    public HealthBar CharacterHealth;
    public HealthBar RadioHealth;
    public HealthBar BossHealth;

    protected override void Init()
    {
        CharacterHealth.SetMaxSliderValue(100f);
        RadioHealth.SetMaxSliderValue(100f);
        BossHealth.SetMaxSliderValue(1000f);
    }
}
