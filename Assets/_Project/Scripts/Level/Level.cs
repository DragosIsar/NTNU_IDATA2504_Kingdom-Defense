using UnityEngine;


[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]
public class Level : ScriptableObject
{
    [Header("Scene References")]
    public string sceneAssetName;
    public int sceneIndexInBuildSettings;
    
    [Header("Level Settings")]
    public float levelDurationInSec = 300f;
    public int maxBaseHealth = 100;
    public int startingCurrency = 100;
    public Enemy[] enemyPrefabs;
    public float spawnInterval = 5f;
    public float spawnIntervalMultiplier = 0.9f;
    
    public bool isUnlocked;
    public bool isCompleted;
    
    public Level levelToUnlock;
}
