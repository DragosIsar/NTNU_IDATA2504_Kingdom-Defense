using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts;
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
    
    public static DayNightCycle DayNightCycle => Instance._dayNightCycle ? Instance._dayNightCycle : FindFirstObjectByType<DayNightCycle>();
    private DayNightCycle _dayNightCycle;
    
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
    
    public static event Action<int> OnGlobalCurrencyChanged;

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
        
        LoadTowerData();
        LoadLevelData();
    }

    private void Update()
    {
        // check if the cursor is above the UI
        CursorAboveUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResumeGame();
    }

    public static void LoadLevel(Level level)
    {
        if (SceneManager.GetSceneByName(level.sceneAssetName).name !=
            SceneManager.GetSceneByBuildIndex(level.sceneIndexInBuildSettings).name)
        {
            Debug.LogError("Scene name and index do not match");
        }
        ResumeGame();
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
    
    public void SaveTowerData ()
    {
        foreach (Tower tower in towers)
        {
            PlayerPrefs.SetInt(tower.settings.towerName, tower.settings.isUnlocked ? 1 : 0);
        }
    }
    
    public static void UnlockTower (TowerSettings towerSettings)
    {
        towerSettings.isUnlocked = true;
        PlayerPrefs.SetInt(towerSettings.towerName, 1);
    }
    
    public static void LockTower (TowerSettings towerSettings)
    {
        towerSettings.isUnlocked = towerSettings.alwaysUnlocked;
        PlayerPrefs.SetInt(towerSettings.towerName, towerSettings.alwaysUnlocked ? 1 : 0);
    }
    
    [ContextMenu("Unlock All Towers")]
    public void UnlockAllTowers ()
    {
        foreach (Tower tower in towers)
        {
            UnlockTower(tower.settings);
        }
    }
    
    [ContextMenu("Lock All Towers except always unlocked")]
    public void LockAllTowersExceptAlwaysUnlocked ()
    {
        foreach (Tower tower in towers)
        {
            LockTower(tower.settings);
        }
    }
    
    [ContextMenu("Give 200 Global Currency")]
    public void Give200GlobalCurrency ()
    {
        AddGlobalCurrency(200);
    }
    
    [ContextMenu("Remove all Global Currency")]
    public void RemoveAllGlobalCurrency ()
    {
        GlobalCurrency = 0;
    }
    
    public void LoadTowerData ()
    {
        foreach (Tower tower in towers)
        {
            if (PlayerPrefs.HasKey(tower.settings.towerName))
            {
                tower.settings.isUnlocked = PlayerPrefs.GetInt(tower.settings.towerName) == 1;
            }
            else
            {
                tower.settings.isUnlocked = false;
            }
        }
    }
    
    public void SaveLevelData ()
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
            
            PlayerPrefs.SetInt(level.name + "_gcg", level.globalCurrencyGained);
        }
    }

    public void LoadLevelData ()
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
            
            if (PlayerPrefs.HasKey(level.name + "_gcg"))
            {
                level.globalCurrencyGained = PlayerPrefs.GetInt(level.name + "_gcg");
            }
            else
            {
                level.globalCurrencyGained = 0;
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
        OnGlobalCurrencyChanged?.Invoke(amount);
        PlayerPrefs.SetInt(GLOBAL_CURRENCY_KEY, amount);
    }

    public static void AddGlobalCurrency(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Cannot add negative currency");
            return;
        }

        GlobalCurrency += amount;
    }
    
    public static void RemoveGlobalCurrency(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Cannot remove negative currency");
            return;
        }
        GlobalCurrency -= amount;
    }
    
    public void PlayNextUnlockedLevel()
    {
        foreach (Level level in levels.Where(level => level.isUnlocked && !level.isCompleted))
        {
            LoadLevel(level);
            return;
        }
    }
    
    [ContextMenu("Unlock All Levels")]
    public void UnlockAllLevels()
    {
        foreach (Level level in levels)
        {
            level.isUnlocked = true;
        }
        SaveLevelData();
    }
    
    [ContextMenu("Lock All Levels except first")]
    public void LockAllLevelsExceptFirst()
    {
        foreach (Level level in levels)
        {
            level.isUnlocked = level.name == levels[0].name;
        }
        SaveLevelData();
    }
    
    [ContextMenu("Complete All Levels")]
    public void CompleteAllLevels()
    {
        foreach (Level level in levels)
        {
            level.isCompleted = true;
        }
        SaveLevelData();
    }

    public static void UnlockAndCompleteLevel(Level level)
    {
        PlayerPrefs.SetInt(level.name, 2);
    }

    public static void UnlockLevel(Level level)
    {
        PlayerPrefs.SetInt(level.name, 1);
    }
    
    public static void LockLevel(Level level)
    {
        PlayerPrefs.SetInt(level.name, 0);
    }
    
    public void CompleteLevel(Level level)
    {
        level.isCompleted = true;
        SaveLevelData();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void PauseGame()
    {
        Time.timeScale = 0f;
    }
    
    public static void ResumeGame()
    {
        Time.timeScale = 1f;
    }
    
    public static void TogglePauseGame()
    {
        if (Time.timeScale == 0f)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public static void SetPauseGame(bool mode)      
    {
        if (mode)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public bool UnlockTower(Tower tower)
    {
        if (!towers.Contains(tower)) return false;
        
        if (GlobalCurrency < tower.settings.unlockCost) return false;
        
        RemoveGlobalCurrency(tower.settings.unlockCost);
        tower.settings.isUnlocked = true;
        
        SaveTowerData();
        
        return true;
    }
}
