using UnityEngine;


[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]
public class Level : ScriptableObject
{
    [Header("Scene References")]
    public string sceneAssetName;
    public int sceneIndexInBuildSettings;
    
    [Header("Level Settings")]
    public int maxBaseHealth = 100;
    public int startingCurrency = 100;
    public Enemy[] enemyPrefabs;
    
    public bool isUnlocked;
    public bool isCompleted;
}
