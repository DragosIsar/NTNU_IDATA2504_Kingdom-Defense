using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager.Tags;

public class ShootingTower : Tower
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    

    protected override void Attack(Enemy enemy)
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Projectile p = projectile.GetComponent<Projectile>();
        
        // calculate the projectile velocity needed to lead and hit the target
        Vector3 dir = enemy.GetHitPoint() - projectile.transform.position;
        float dist = dir.magnitude;
        float projectileTravelTime = dist / p.GetSpeed();
        Vector3 targetVelocity = enemy.GetVelocity();
        Vector3 targetDisplacement = targetVelocity * projectileTravelTime;
        dir += targetDisplacement;
        
        // set projectile direction
        p.SetDirection(dir.normalized);
        p.SetDamage(settings.damage);
    }
}
