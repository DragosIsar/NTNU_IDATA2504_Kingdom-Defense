using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    [SerializeField] private float speed = 1f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float health = 1f;
    
    private void Start()
    {
        target = GameObject.FindWithTag("Base").transform;
    }
    
    private void Update()
    {
        if (target == null) return;
        
        Vector3 dir = target.position - transform.position;
        dir.y = 0;
        GetComponent<Rigidbody>().velocity = dir.normalized * speed;
    }
    
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    
    public void Damage(float damage)
    {
        if (health - damage <= 0)
        {
            health = 0;
            Die();
        }
        else
        {
            health -= damage;
        }
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Base"))
        {
            LevelManager.Instance.DamageBase(damage);
            Die();
        }
    }
}
