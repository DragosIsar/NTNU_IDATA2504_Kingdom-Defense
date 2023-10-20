using System;
using UnityEngine;
using static GameManager;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Level level;

    [SerializeField] private float baseHealth = 100f;

    public int inLevelCurrency;
    private float _levelScore;

    private Tower[] _towers;

    public LayerMask towerPlacementLayerMask;
    
    public Enemy enemyPrefab;
    public Transform enemySpawnPoint;
    public float enemySpawnInterval = 1f;
    private float _enemySpawnTimer;

    private void Start()
    {
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
        Enemy enemy = Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
    }
    
    public void DamageBase(float damage)
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
    }

    public void CollectCurrency(int amount)
    {
        inLevelCurrency += amount;
    }

    private void EndLevel()
    {
        CalculateScore();
    }

    private void CalculateScore()
    {
        _levelScore = (baseHealth / level.maxBaseHealth) * 100f;
    }

    public bool TryPlaceTower(Vector3 pos, Tower tower)
    {
        if (inLevelCurrency >= tower.settings.cost) return false;
        if (IsTowerLocationValid(pos, tower)) return false;
        
        Instantiate(tower, pos, Quaternion.identity);
        return true;
    }

    private bool IsTowerLocationValid(Vector3 pos, Tower tower)
    {
        return Physics.OverlapSphere(pos, tower.settings.placeRadius, towerPlacementLayerMask).Length > 0;
    }
}