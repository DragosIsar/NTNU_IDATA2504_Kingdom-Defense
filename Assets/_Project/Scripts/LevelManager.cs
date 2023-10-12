using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelSettings levelSettings;

    public float baseHealth = 100f;

    public int inLevelCurrency = 0;
    private float _levelScore = 0f;

    private Tower[] _towers;

    public GameObject gameManager;

    public LayerMask towerPlacementLayerMask;

    private void Start()
    {
        baseHealth = levelSettings.maxBaseHealth;

        // _towers = GameManager.instance.towers;
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
        _levelScore = (baseHealth / levelSettings.maxBaseHealth) * 100f;
    }

    public bool TryPlaceTower(Vector3 pos, Tower tower)
    {
        if (inLevelCurrency >= tower.cost) return false;
        if (IsTowerLocationValid(pos, tower)) return false;
        
        Instantiate(tower, pos, Quaternion.identity);
        return true;
    }

    private bool IsTowerLocationValid(Vector3 pos, Tower tower)
    {
        return Physics.OverlapSphere(pos, tower.placeRadius, towerPlacementLayerMask).Length > 0;
    }
}