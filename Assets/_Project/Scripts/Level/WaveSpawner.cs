using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static GameManager;
using Random = UnityEngine.Random;
using LRS;
public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private float countdown;

    [SerializeField] private List<GameObject> pathContainers;
    
    [SerializeField] private Transform enemyContainer;
    
    private List<List<Transform>> _paths = new();

    private Wave[] _waves;
    public int currentWaveIndex = 0;
    private int _totalEnemyCount;

    private bool _readyToCountDown;
    private void Start()
    {
        _readyToCountDown = true;
        
        GeneratePaths();
    }
    private void Update()
    {
        if (_readyToCountDown)
        {
            countdown -= Time.deltaTime;
            GameManager.HUD.SetShowCountdown(true);
        }

        if (countdown <= 0)
        {
            _readyToCountDown = false;
            GameManager.HUD.SetShowCountdown(false);

            countdown = LevelManager.Instance.GetLevel().timeBetweenWaves;

            StartCoroutine(SpawnWave());
        }
    }
    private IEnumerator SpawnWave()
    {
        if (currentWaveIndex < _waves.Length)
        {
            SetEnemiesLeft();
            foreach (Enemy enemyPrefab in _waves[currentWaveIndex].enemyPrefabs)
            {
                List<Transform> path = _paths[Random.Range(0, _paths.Count)];

                Transform spawnPoint = path[0];

                Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                if (enemyContainer) enemy.transform.SetParent(enemyContainer);
                enemy.InitPath(path);
                enemy.OnDeath += () =>
                {
                    if (currentWaveIndex < _waves.Length)_waves[currentWaveIndex].enemiesLeft--;
                    if (_waves[currentWaveIndex].enemiesLeft == 0)
                    {
                        _readyToCountDown = true;
                        currentWaveIndex++;
                        Time.timeScale = 1;
                    }

                    if (currentWaveIndex >= _waves.Length)
                    {
                        LevelManager.Instance.GameWin();
                    }
                };

                _totalEnemyCount++;
                yield return new WaitForSeconds(_waves[currentWaveIndex].timeToNextEnemy);
            }
        }
    }

    private void GeneratePaths()
    {
        foreach (GameObject pathContainer in pathContainers)
        {
            List<Transform> path = new List<Transform>();

            foreach (Transform child in pathContainer.transform)
            {
                path.Add(child);
            }

            _paths.Add(path);
        }
    }
    
    public void SetWaves(Wave[] waves)
    {
        _waves = waves;
    }

    public string GetWaveStatusText()
    {
        if (currentWaveIndex >= _waves.Length) return "Win!";
        return "Wave: " + (currentWaveIndex + 1) + "/" + _waves.Length;
    }

    public string GetCountdownText()
    {
        return "Next wave in: " + Mathf.Round(countdown);
    }

    public void SetEnemiesLeft()
    {
        foreach (Wave wave in _waves)
        {
            wave.enemiesLeft = wave.enemyPrefabs.Length;
        }
    }
}
