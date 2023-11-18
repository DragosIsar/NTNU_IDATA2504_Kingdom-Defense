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
    
    private void Update()
    {
        if (_attackRateTimer > 0)
        {
            _attackRateTimer -= Time.deltaTime;
        }
        else
        {
            Attack();
            _attackRateTimer = 1 / _attackRate;
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        
    }

    protected virtual void Attack()
    {
        
    }
}