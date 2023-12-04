using UnityEngine;
using UnityEngine.UI;


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
    public Wave[] waves;
    [Tooltip("in seconds")]
    public float timeBetweenWaves = 10f;
    
    [Header("UI")]
    public Sprite thumbnail;
    
    [Header("Other")]
    public bool isUnlocked;
    public bool isCompleted;
    public int globalCurrencyGained;
    public int globalCurrencyToUnlock;
    
    public Level levelToUnlock;
    
    [ContextMenu("Unlock Level and Save")]
    public void UnlockLevelAndSave()
    {
        isUnlocked = true;
        GameManager.UnlockLevel(this);
    }
    
    [ContextMenu("Lock Level and Save")]
    public void LockLevelAndSave()
    {
        isUnlocked = false;
        GameManager.LockLevel(this);
    }
}
