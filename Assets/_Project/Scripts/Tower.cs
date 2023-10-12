using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int cost = 10;
    public float placeRadius = 10f;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float fireRate = 1f;
    private float _fireRateTimer;
    [SerializeField] private int damage = 1;
    
    private Enemy _target;
    
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
            _fireRateTimer = 1 / fireRate;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_target != null) return;
        
        if (other.gameObject.CompareTag("Enemy"))
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
        
        // calculate direction to target
        Vector3 dir = (_target.transform.position + Vector3.up * 0.5f) - projectileSpawnPoint.position;
        dir = dir.normalized;
        
        // set projectile direction
        Projectile p = projectile.GetComponent<Projectile>();
        p.SetVelocity(dir * 10f);
        p.SetDamage(damage);
    }
}