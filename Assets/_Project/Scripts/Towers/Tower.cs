using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager.Tags;
using static GameManager;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(CapsuleCollider))]
public class Tower : MonoBehaviour
{
    public TowerSettings settings;
    
    private float _attackRate = 1f;
    private float _attackRateTimer;
    private float _range = 5f;
    
    private List<Enemy> _targets = new();
    
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
                OrderListByHealth();
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

    private void Update()
    {
        if (_attackRateTimer > 0)
        {
            _attackRateTimer -= Time.deltaTime;
        }
        else
        {
            if (_targets.Count != 0) Attack(_targets[0]);
            _attackRateTimer = 1 / _attackRate;
        }
    }

    protected virtual void Attack(Enemy enemy)
    {
        if (settings.attackSound) SoundManager.Instance.PlaySFX(settings.attackSound, .5f);
    }
    
    protected void OrderListByHealth()
    {
        _targets.Sort((enemy1, enemy2) => enemy1.GetHealth().CompareTo(enemy2.GetHealth()));
        _targets.Reverse();
    }
}