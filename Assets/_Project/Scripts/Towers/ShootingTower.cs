using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager.Tags;

public class ShootingTower : Tower
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    
    private Enemy _target;
    
    protected override void OnTriggerStay(Collider other)
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

    protected override void Attack()
    {
        if (_target == null) return;
        
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Projectile p = projectile.GetComponent<Projectile>();
        
        // calculate the projectile velocity needed to lead and hit the target
        Vector3 dir = _target.GetHitPoint() - projectile.transform.position;
        float dist = dir.magnitude;
        float projectileTravelTime = dist / p.GetSpeed();
        Vector3 targetVelocity = _target.GetVelocity();
        Vector3 targetDisplacement = targetVelocity * projectileTravelTime;
        dir += targetDisplacement;
        
        // set projectile direction
        p.SetVelocityWithDirection(dir.normalized);
        p.SetDamage(settings.damage);
    }
}
