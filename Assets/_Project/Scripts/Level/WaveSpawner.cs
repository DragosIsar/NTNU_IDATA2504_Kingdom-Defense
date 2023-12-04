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
    // [SerializeField] private GameObject spawnPoint;

    public List<Transform> spawnLocations;

    [SerializeField] private List<Transform> path1;
    [SerializeField] private List<Transform> path2;

    public Wave[] waves;
    public int currentWaveIndex = 0;
    private int _enemyCount;

    private bool readyToCountDown;
    private void Start()
    {
        readyToCountDown = true;

        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemyPrefabs.Length;
        }
    }
    private void Update()
    {
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("You survived every wave!");
            // after adding textmeshpro with wave number, update it with "Level complete"
            // show the menu UI with quit to main menu or button to load next level
            return;
        }

        if (readyToCountDown == true)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0)
        {
            readyToCountDown = false;

            countdown = waves[currentWaveIndex].timeToNextWave;

            StartCoroutine(SpawnWave());
        }

        if (waves[currentWaveIndex].enemiesLeft == 0)
        {
            readyToCountDown = true;

            currentWaveIndex++;
        }
    }
    private IEnumerator SpawnWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            for (int i = 0; i < waves[currentWaveIndex].enemyPrefabs.Length; i++)
            {
                int randomIndex = Random.Range(0, spawnLocations.Count);

                Transform spawnPoint = spawnLocations[randomIndex];

                Enemy enemy = Instantiate(waves[currentWaveIndex].enemyPrefabs[i], spawnPoint.position, Quaternion.identity);

                enemy.transform.SetParent(spawnPoint.transform);

                enemy.InitPath(Random.Range(0, 2) < 1 ? path2 : path1);

                _enemyCount++;

                yield return new WaitForSeconds(waves[currentWaveIndex].timeToNextEnemy);
            }
        }
    }
}

[System.Serializable]
public class Wave
{
    public Enemy[] enemyPrefabs;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemiesLeft;
}