using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static GameManager;
using Random = UnityEngine.Random;
using LRS;

public class LevelManager : Singleton<LevelManager>
{
    public event Action<int> OnCurrencyChanged;
    public Action<int> onHealthChanged;
    
    [SerializeField] private Level level;
    [SerializeField] private List<Transform> path1;
    [SerializeField] private List<Transform> path2;

    [SerializeField] private int baseHealth = 100;

    public int inLevelCurrency;
    private int _levelScore = -1;

    private List<Tower> _towers;
    private Tower _towerToPlace;

    public LayerMask antiTowerPlacementLayerMask;
    public LayerMask proTowerPlacementLayerMask;
    
    public Transform enemySpawnPoint;
    public float enemySpawnInterval = 1f;
    private float _enemySpawnTimer;
    private int _enemyCount;

    private float _currentLevelTime;
    
    private bool _gameEnded;

    protected override void Awake()
    {
        base.Awake();
        useDontDestroyOnLoad = false;
        baseHealth = level.maxBaseHealth;
        inLevelCurrency = level.startingCurrency;
        enemySpawnInterval = level.spawnInterval;
        _currentLevelTime = level.levelDurationInSec;
    }

    private void Update()
    {
        if (_enemySpawnTimer > 0)
        {
            _enemySpawnTimer -= Time.deltaTime;
        }
        else
        {
            SpawnEnemy();
            IncreaseSpawnInterval();    
            _enemySpawnTimer = enemySpawnInterval;
        }
        
        if (_currentLevelTime > 0)
        {
            _currentLevelTime -= Time.deltaTime;
        }
        else
        {
            GameWin();
        }
    }

    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(level.enemyPrefabs[Random.Range(0, level.enemyPrefabs.Length)], enemySpawnPoint.position, Quaternion.identity);
        
        if(Random.Range(0,2) < 1)
        {
            enemy.InitPath(path2);
            //Debug.Log("Path 2");
        }
        else
        {
            enemy.InitPath(path1);
            //Debug.Log("Path 1");
        }
        
        _enemyCount++;
    }
    
    private void IncreaseSpawnInterval()
    {
        if (_enemyCount % 12 == 0)
        {
            enemySpawnInterval *= level.spawnIntervalMultiplier;
        }
    }
    
    public void DamageBase(int damage)
    {
        if (baseHealth - damage <= 0)
        {
            baseHealth = 0;
            GameOver();
        }
        else
        {
            baseHealth -= damage;
        }
        onHealthChanged?.Invoke(baseHealth);
    }

    public void CollectCurrency(int amount)
    {
        inLevelCurrency += amount;
        OnCurrencyChanged?.Invoke(inLevelCurrency);
    }
    
    public void SpendCurrency(int amount)
    {
        inLevelCurrency -= amount;
        OnCurrencyChanged?.Invoke(inLevelCurrency);
    }

    private void GameOver()
    {
        if (_gameEnded) return;
        _gameEnded = true;
        PauseGame();
        GameManager.Player.SetPlayerState(PlayerState.None);
        GameManager.HUD.ShowGameOverScreen();
    }

    private void GameWin()
    {
        if (_gameEnded) return;
        _gameEnded = true;
        level.levelToUnlock.isUnlocked = true;
        PauseGame();
        GameManager.Player.SetPlayerState(PlayerState.None);
        CalculateScore();
        GrantCurrencyBasedOnScore();
        GameManager.HUD.ShowGameWinScreen();
    }

    private void GrantCurrencyBasedOnScore()
    {
        int currency = _levelScore / 10;
        AddGlobalCurrency(currency);
    }

    private void CalculateScore()
    {
        _levelScore = baseHealth / level.maxBaseHealth * 100;
    }

    public bool TryPlaceTower(Vector3 pos)
    {
        if (inLevelCurrency < _towerToPlace.settings.placementCost)
        {
            SetStatusText("Not enough currency!");
            return false;
        }

        if (IsTowerLocationValid(pos, _towerToPlace) ||
            pos == Vector3.zero)
        {
            SetStatusText("Not enough space!");
            return false;
        }
        
        Instantiate(_towerToPlace, pos, Quaternion.identity);
        SpendCurrency(_towerToPlace.settings.placementCost);
        return true;
    }

    private bool IsTowerLocationValid(Vector3 pos, Tower tower)
    {
        return Physics.CheckSphere(pos, tower.settings.placeRadius, antiTowerPlacementLayerMask);
    }

    public int GetBaseHealth()
    {
        return baseHealth;
    }

    public void SetTowerToPlace(Tower tower)
    {
        _towerToPlace = tower;
        GameManager.Player.SetPlayerState(PlayerState.TowerPlacement);
    }
    
    public static void SetStatusText(string text, float duration = 2f)
    {
        GameManager.HUD.SetStatusText(text, duration);
    }

    public void ReloadLevel()
    {
        LoadLevel(level);
    }

    public void LoadMainMenu()
    {
        GameManager.LoadMainMenu();
    }

    public float GetBaseMaxHealth()
    {
        return level.maxBaseHealth;
    }
    
    public string GetCurrentLevelTimeFormatted()
    {
        return TimeSpan.FromSeconds(_currentLevelTime).ToString(@"mm\:ss");
    }

    public string GetLevelScore()
    {
        if (_levelScore == -1)
        {
            CalculateScore();
        }
        
        return _levelScore.ToString("F0");
    }

    public void LoadNextLevel()
    {
        if (level.levelToUnlock != null)
        {
            LoadLevel(level.levelToUnlock);
        }
    }
}