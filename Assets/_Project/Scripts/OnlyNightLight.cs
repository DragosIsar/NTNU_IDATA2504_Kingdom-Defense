using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyNightLight : MonoBehaviour
{
 
    private Light _light;
    private void Awake()
    {
        GameManager.DayNightCycle.OnDayChanged += OnDayChangedHandler;
        _light = GetComponent<Light>();
    }
    
    private void OnDayChangedHandler(bool isDay)
    {
        _light.enabled = !isDay;
    }
}
