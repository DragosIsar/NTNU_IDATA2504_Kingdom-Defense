using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private int _damage = 1;
    private Rigidbody _rigidbody;
    
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        _rigidbody.velocity = transform.forward * speed;
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
    
    public void SetDirection(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }
    
    public float GetSpeed()
    {
        return speed;
    }
}
