using UnityEngine;
using static LRS.Tools.PhysicCalculations;

public class GravityProjectileTower : ShootingTower
{
    [SerializeField] private float shootingAngle = 45f;
    
    protected override void Attack(Enemy enemy)
    {
        if (settings.attackSound) SoundManager.Instance.PlaySFX(settings.attackSound);
        
        Vector3 velocity = VelocityTowardsTargetWithAngle(shootingAngle, projectileSpawnPoint.position, enemy.GetHitPoint());
        
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        
        if (projectile.TryGetComponent(out ExplodeOnImpact explodeOnImpact))
        {
            explodeOnImpact.SetDamage(damage);
        }
        
        rb.AddForce(velocity, ForceMode.VelocityChange);
    }
}
