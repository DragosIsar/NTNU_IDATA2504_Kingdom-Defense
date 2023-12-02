using System.Collections.Generic;
using UnityEngine;
using static GameManager.Tags;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(CapsuleCollider))]
public class Tower : MonoBehaviour
{
    public TowerSettings settings;
    
    public int towerTier = 1;
    public List<GameObject> upgrades = new();
    [SerializeField] private Transform rangeIndicator;

    public int damage = 1;
    public float attackRate = 1f;
    private float _attackRateTimer;
    public float range = 5f;
    public int upgradeCost;
    public int sellValue;
    
    protected List<Enemy> _targets = new();
    
    private SphereCollider _sphereCollider;

    private void Start()
    {
        if (settings == null)
        {
            Debug.LogError("TowerController: No settings found!");
            return;
        }
        
        damage = settings.damage;
        attackRate = settings.attackRate;
        range = settings.range;
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = range;
        upgradeCost = settings.upgradeCost;
        sellValue = settings.sellValue;
    }

    protected void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(ENEMY)) return;
        if (!other.TryGetComponent(out Enemy enemy)) return;
        
        if (!_targets.Contains(enemy))
        {
            _targets.Add(enemy);
            enemy.OnDeath += () =>
            {
                _targets.Remove(enemy);
                OrderList();
            };
        }
    }
    
    protected void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(ENEMY)) return;
        if (!other.TryGetComponent(out Enemy enemy)) return;
        
        if (_targets.Contains(enemy))
        {
            _targets.Remove(enemy);
        }
    }

    protected virtual void Update()
    {
        if (_attackRateTimer > 0)
        {
            _attackRateTimer -= Time.deltaTime;
        }
        else if (_targets.Count > 0)
        {
            Attack(_targets[0]);
            _attackRateTimer = 1 / attackRate;
        }
    }

    protected virtual void Attack(Enemy enemy)
    {
        if (settings.attackSound) SoundManager.Instance.PlaySFX(settings.attackSound, .5f);
    }
    
    protected void OrderList()
    {
        _targets.Sort((enemy1, enemy2) => enemy2.GetHealth().CompareTo(enemy1.GetHealth()));
    }
    
    public void SetGhostMaterial(bool placeable)
    {
        MeshRenderer meshRenderer = upgrades[towerTier - 1].GetComponent<MeshRenderer>();
        if (meshRenderer == null) return;
        meshRenderer.material = placeable ? settings.validGhostMaterial : settings.invalidGhostMaterial;
    }

    public virtual void Upgrade()
    {
        if (towerTier >= settings.maxTier) return;
        towerTier++;
        damage += settings.damageIncrease;
        attackRate += settings.attackRateIncrease;
        range += settings.rangeIncrease;
        _sphereCollider.radius = range;
        upgradeCost += settings.upgradeCostIncrease;
        sellValue += settings.sellValueIncrease;
        
        foreach (GameObject o in upgrades)
        {
            o.SetActive(false);
        }
        upgrades[towerTier - 1].SetActive(true);
    }
    
    public void ShowRangeIndicator(bool show)
    {
        rangeIndicator.gameObject.SetActive(show);
        range = range > settings.range ? range : settings.range;
        rangeIndicator.localScale = Vector3.one * (range * 2);
    }
}