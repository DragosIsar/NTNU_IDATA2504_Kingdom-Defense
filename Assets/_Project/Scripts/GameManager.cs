using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private const string _GLOBAL_CURRENCY_KEY = "GlobalCurrency";

    protected override void Awake()
    {
        base.Awake();
        useDontDestroyOnLoad = true;
    }
    
    private static int GetGlobalCurrency()
    {
        if (PlayerPrefs.HasKey(_GLOBAL_CURRENCY_KEY))
        {
            return PlayerPrefs.GetInt(_GLOBAL_CURRENCY_KEY);
        }
        else
        {
            PlayerPrefs.SetInt(_GLOBAL_CURRENCY_KEY, 0);
            return 0;
        }
    }
    
    private void SetGlobalCurrency(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Cannot set negative currency");
            return;
        }
        PlayerPrefs.SetInt(_GLOBAL_CURRENCY_KEY, amount);
    }
    
    private void AddGlobalCurrency(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Cannot add negative currency");
            return;
        }
        int currentCurrency = GetGlobalCurrency();
        PlayerPrefs.SetInt(_GLOBAL_CURRENCY_KEY, currentCurrency + amount);
    }
}
