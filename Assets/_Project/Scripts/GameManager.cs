using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager.Keys;

public class GameManager : Singleton<GameManager>
{
    private List<Level> _levels = new ();
    
    private List<Tower> _towers = new ();

    public Player Player => _player ? _player : FindObjectOfType<Player>();
    private Player _player;

    public static class Keys
    {
        public const string GLOBAL_CURRENCY_KEY = "GlobalCurrency";
    }

    public static class Tags
    {
        public const string TOWER = "Tower";
        public const string ENEMY = "Enemy";
    }
    
    public static int GlobalCurrency
    {
        get => GetGlobalCurrency();
        set => SetGlobalCurrency(value);
    }

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    protected override void Awake()
    {
        base.Awake();
        useDontDestroyOnLoad = true;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }

    public static void LoadLevel(Level level)
    {
        if (SceneManager.GetSceneByName(level.sceneName).name !=
            SceneManager.GetSceneByBuildIndex(level.sceneIndex).name)
        {
            Debug.LogError("Scene name and index do not match");
            return;
        }
        SceneManager.LoadScene(level.sceneName);
    }
    
    public static void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public static void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    
    private static int GetGlobalCurrency()
    {
        if (PlayerPrefs.HasKey(GLOBAL_CURRENCY_KEY))
        {
            return PlayerPrefs.GetInt(GLOBAL_CURRENCY_KEY);
        }
        else
        {
            PlayerPrefs.SetInt(GLOBAL_CURRENCY_KEY, 0);
            return 0;
        }
    }
    
    private static void SetGlobalCurrency(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Cannot set negative currency");
            return;
        }
        PlayerPrefs.SetInt(GLOBAL_CURRENCY_KEY, amount);
    }
    
    private void AddGlobalCurrency(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Cannot add negative currency");
            return;
        }
        int currentCurrency = GetGlobalCurrency();
        PlayerPrefs.SetInt(GLOBAL_CURRENCY_KEY, currentCurrency + amount);
    }
}
