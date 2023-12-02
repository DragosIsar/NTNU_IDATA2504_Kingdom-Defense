using System.Collections.Generic;
using UnityEngine;
using static GameManager.Tags;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(CapsuleCollider))]
public class Tower : MonoBehaviour
{
    public TowerSettings settings;
    
    private float _attackRate = 1f;
    private float _attackRateTimer;
    private float _range = 5f;

    protected List<Enemy> _targets = new();
    
    private SphereCollider _sphereCollider; 

    private void Start()
    {
        if (settings == null)
        {
            Debug.LogError("TowerController: No settings found!");
            return;
        }
        
        _attackRate = settings.attackRate;
        _range = settings.range;
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = _range;
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
            _attackRateTimer = 1 / _attackRate;
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

    private Material _originalMaterial;
    public void SetGhostMaterial(bool placeable)
    {
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshRenderer == null) return;
        if (_originalMaterial == null) _originalMaterial = meshRenderer.material;
        meshRenderer.material = placeable ? settings.validGhostMaterial : settings.invalidGhostMaterial;
    }

    public override bool Equals(object other)
    {
        return other switch
        {
            null => false,
            Tower tower => tower.settings == settings,
            _ => base.Equals(other)
        };
    }
}