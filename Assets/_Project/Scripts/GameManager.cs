using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager.Keys;
using LRS;

public class GameManager : Singleton<GameManager>
{
    public List<Level> levels = new ();
    
    public List<Tower> towers = new ();

    public static Player Player => Instance._player ? Instance._player : FindFirstObjectByType<Player>();
    private Player _player;
    
    public static HUD HUD => Instance._hud ? Instance._hud : FindFirstObjectByType<HUD>();
    private HUD _hud;
    
    public static bool CursorAboveUI
    {
        get => IsInitialized && Instance._cursorAboveUI;
        set
        {
            if (IsInitialized)
            {
                return;
            }
            Instance._cursorAboveUI = value;
        }
    }
    
    private bool _cursorAboveUI;

    public static class Keys
    {
        public const string GLOBAL_CURRENCY_KEY = "GlobalCurrency";
    }

    public static class Tags
    {
        public const string TOWER = "Tower";
        public const string ENEMY = "Enemy";
        public const string GROUND = "Ground";
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
        
        //LoadLevelData();
    }

    private void Update()
    {
        // check if the cursor is above the UI
        CursorAboveUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }

    public static void LoadLevel(Level level)
    {
        if (SceneManager.GetSceneByName(level.sceneAssetName).name !=
            SceneManager.GetSceneByBuildIndex(level.sceneIndexInBuildSettings).name)
        {
            Debug.LogError("Scene name and index do not match");
            return;
        }
        SceneManager.LoadScene(level.sceneAssetName);
    }
    
    public static void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(levels[sceneIndex].sceneAssetName);
    }
    
    private void SafeLevelData ()
    {
        foreach (Level level in levels)
        {
            if (level.isCompleted)
            {
                PlayerPrefs.SetInt(level.name, 2);
            }
            else if (level.isUnlocked)
            {
                PlayerPrefs.SetInt(level.name, 1);
            }
            else
            {
                PlayerPrefs.SetInt(level.name, 0);
            }
        }
    }
    
    private void LoadLevelData ()
    {
        foreach (Level level in levels)
        {
            if (PlayerPrefs.HasKey(level.name))
            {
                int levelState = PlayerPrefs.GetInt(level.name);
                switch (levelState)
                {
                    case 0:
                        level.isUnlocked = false;
                        level.isCompleted = false;
                        break;
                    case 1:
                        level.isUnlocked = true;
                        level.isCompleted = false;
                        break;
                    case 2:
                        level.isUnlocked = true;
                        level.isCompleted = true;
                        break;
                }
            }
            else
            {
                level.isUnlocked = false;
                level.isCompleted = false;
            }
        }
    }
    
    private static int GetGlobalCurrency()
    {
        if (PlayerPrefs.HasKey(GLOBAL_CURRENCY_KEY))
        {
            return PlayerPrefs.GetInt(GLOBAL_CURRENCY_KEY);
        }

        PlayerPrefs.SetInt(GLOBAL_CURRENCY_KEY, 0);
        return 0;
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
    
    public void PlayNextUnlockedLevel()
    {
        foreach (Level level in levels.Where(level => level.isUnlocked && !level.isCompleted))
        {
            LoadLevel(level);
            return;
        }
    }
    
    public void UnlockLevel(Level level)
    {
        level.isUnlocked = true;
        SafeLevelData();
    }
    
    public void CompleteLevel(Level level)
    {
        level.isCompleted = true;
        SafeLevelData();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
