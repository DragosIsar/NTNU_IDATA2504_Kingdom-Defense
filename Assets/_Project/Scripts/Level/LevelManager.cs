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
    private float _levelScore;

    private List<Tower> _towers;
    private Tower _towerToPlace;

    public LayerMask antiTowerPlacementLayerMask;
    public LayerMask proTowerPlacementLayerMask;
    
    public Transform enemySpawnPoint;
    public float enemySpawnInterval = 1f;
    private float _enemySpawnTimer;
    private int _enemyCount;


    protected override void Awake()
    {
        base.Awake();
        baseHealth = level.maxBaseHealth;
        inLevelCurrency = level.startingCurrency;
        enemySpawnInterval = level.spawnInterval;
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
            EndLevel();
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

    private void EndLevel()
    {
        CalculateScore();
    }

    private void CalculateScore()
    {
        _levelScore = (baseHealth / level.maxBaseHealth) * 100f;
    }

    public bool TryPlaceTower(Vector3 pos)
    {
        if (inLevelCurrency < _towerToPlace.settings.cost)
        {
            SetStatusText("Not enough currency!");
            return false;
        }

        if (IsTowerLocationValid(pos, _towerToPlace))
        {
            SetStatusText("Not enough space!");
            return false;
        }
        
        Instantiate(_towerToPlace, pos, Quaternion.identity);
        SpendCurrency(_towerToPlace.settings.cost);
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
}