using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarDisplay : MonoBehaviour
{
    private Slider _slider;

    private void OnEnable()
    {
        LevelManager.Instance.onHealthChanged += OnHealthChangedHandler;
    }
    
    private void OnDisable()
    {
        LevelManager.Instance.onHealthChanged -= OnHealthChangedHandler;
    }

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        _slider.value = 1;
    }

    private void OnHealthChangedHandler(int health)
    {
        _slider.value = health / LevelManager.Instance.GetBaseMaxHealth();
    }
}
