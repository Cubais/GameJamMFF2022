using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fillColor;

    public void SetMaxSliderValue(float val)
    {
        slider.maxValue = val;
        slider.value = val;

        fillColor.color = gradient.Evaluate(1f);
    }

    public void SetSliderValue(float health)
    {
        if(health >=0 )
            slider.value = health;

        fillColor.color = gradient.Evaluate(slider.normalizedValue);
    }
}
