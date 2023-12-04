using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarDisplay : MonoBehaviour
{
    private TMP_Text _text;

    private void OnEnable()
    {
        LevelManager.Instance.onHealthChanged += OnHealthChangedHandler;
    }

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        _text.text = $"{LevelManager.Instance.GetBaseHealth()}/{LevelManager.Instance.GetBaseMaxHealth()}";
    }

    private void OnHealthChangedHandler(int health)
    {
        _text.text = $"{health}/{LevelManager.Instance.GetBaseMaxHealth()}";
    }
}
