using UnityEngine;

[CreateAssetMenu(fileName = "TowerSettings", menuName = "ScriptableObjects/TowerSettings")]
public class TowerSettings : ScriptableObject
{
    [Header("Tower Settings")] public int cost = 10;
    public float range = 5f;
    public float attackRate = 1f;
    public int damage = 1;
    public float placeRadius = 1f;

    [Header("Upgrades")] public int upgradeCostIncrease = 5;
    public float rangeIncrease = 1f;
    public float attackRateIncrease = 0.1f;
    public int damageIncrease = 1;

    [Header("UI")]
    public Sprite icon;
}
