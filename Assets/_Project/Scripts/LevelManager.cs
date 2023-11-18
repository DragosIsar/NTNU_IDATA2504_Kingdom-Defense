using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static GameManager;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    public event Action<int> OnCurrencyChanged;
    public Action<int> onHealthChanged;
    
    [SerializeField] private Level level;
    [SerializeField] private List<Transform> pathPos;
    
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

    protected override void Awake()
    {
        base.Awake();
        baseHealth = level.maxBaseHealth;
        inLevelCurrency = level.startingCurrency;
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
            _enemySpawnTimer = enemySpawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(level.enemyPrefabs[Random.Range(0, level.enemyPrefabs.Length)], enemySpawnPoint.position, Quaternion.identity);
        enemy.InitPath(pathPos);
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

    public bool TryPlaceTower(Tower tower, Vector3 pos)
    {
        if (inLevelCurrency < tower.settings.cost) return false;
        if (IsTowerLocationValid(pos, tower)) return false;
        
        Instantiate(tower, pos, Quaternion.identity);
        SpendCurrency(tower.settings.cost);
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
        GameManager.Instance.Player.SetPlayerState(PlayerState.TowerPlacement);
    }
}