using UnityEngine;

[CreateAssetMenu(fileName = "TowerSettings", menuName = "ScriptableObjects/TowerSettings")]
public class TowerSettings : ScriptableObject
{
    public bool isUnlocked = true;
    public bool alwaysUnlocked = false;
    
    [Header("Tower Settings")] 
    public int unlockCost = 10;
    public int placementCost = 10;
    public float range = 5f;
    public float attackRate = 1f;
    public int damage = 1;
    public float placeRadius = 1f;

    [Header("Upgrades")] 
    public int upgradeCostIncrease = 5;
    public float rangeIncrease = 1f;
    public float attackRateIncrease = 0.1f;
    public int damageIncrease = 1;

    [Header("UI")]
    public string towerName;
    public Sprite icon;
    
    [Header("Sounds")]
    public AudioClip attackSound;
    
    [Header("Other")]
    public Material validGhostMaterial;
    public Material invalidGhostMaterial;
    
    [ContextMenu("Unlock Tower and Save")]
    public void UnlockTowerAndSave()
    {
        isUnlocked = true;
        GameManager.UnlockTower(this);
    }
    
    [ContextMenu("Lock Tower and Save")]
    public void LockTowerAndSave()
    {
        isUnlocked = false;
        GameManager.LockTower(this);
    }
}
