using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager.Tags;
using static GameManager;

public class Tower : MonoBehaviour
{
    public TowerSettings settings;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    private float _fireRate = 1f;
    private float _fireRateTimer;
    private int _damage = 1;
    private int _cost = 10;
    private float _range = 5f;
    
    private Enemy _target;
    
    private SphereCollider _sphereCollider; 

    private void Start()
    {
        if (settings == null)
        {
            Debug.LogError("TowerController: No settings found!");
            return;
        }
        
        _fireRate = settings.fireRate;
        _damage = settings.damage;
        _cost = settings.cost;
        
        _range = settings.range;
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = _range;
    }
    
    private void Update()
    {
        if (_target == null) return;
        
        if (_fireRateTimer > 0)
        {
            _fireRateTimer -= Time.deltaTime;
        }
        else
        {
            Fire();
            _fireRateTimer = 1 / _fireRate;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_target != null) return;
        
        if (other.gameObject.CompareTag(ENEMY))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                _target = enemy;
            }
        }
    }

    private void Fire()
    {
        if (_target == null) return;
        
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        
        // calculate the projectile velocity needed to lead and hit the target
        Vector3 dir = _target.transform.position - projectile.transform.position;
        float dist = dir.magnitude;
        float projectileSpeed = 10f;
        float projectileTravelTime = dist / projectileSpeed;
        Vector3 targetVelocity = _target.GetComponent<Rigidbody>().velocity;
        Vector3 targetDisplacement = targetVelocity * projectileTravelTime;
        dir += targetDisplacement;
        
        // set projectile direction
        Projectile p = projectile.GetComponent<Projectile>();
        p.SetVelocity(dir.normalized * projectileSpeed);
        p.SetDamage(_damage);
    }
    
    public void Upgrade()
    {
        _cost += settings.upgradeCostIncrease;
        
        _range += settings.rangeIncrease;
        _sphereCollider.radius = _range;
        
        _fireRate += settings.fireRateIncrease;
        _damage += settings.damageIncrease;
    }
}