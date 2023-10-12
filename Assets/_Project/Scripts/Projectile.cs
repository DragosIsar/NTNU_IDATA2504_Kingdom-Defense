using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int _damage = 1;
    
    private Rigidbody _rigidbody;
    
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage(_damage);
            }
        }
        Destroy(gameObject);
    }
    
    public void SetDamage(int damage)
    {
        _damage = damage;
    }
    
    public void SetVelocity(Vector3 velocity)
    {
        _rigidbody.velocity = velocity;
    }
}
