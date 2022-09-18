using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;

    [SerializeField] private Gradient gradient;

    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        SetHealth(health);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
