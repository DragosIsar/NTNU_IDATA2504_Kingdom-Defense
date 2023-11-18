using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private Animator animator;

    private int _currentHealth;
    
    private Rigidbody _rigidbody;
    private int _targetIndex;
    private List<Transform> _pathPositions;
    
    private void Start()
    {
        _currentHealth = enemyType.health;
        animator ??= GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (IsAtTarget())
        {
            SwitchToNextTarget();
        }
        else
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {   
        Vector3 dir = _pathPositions[_targetIndex].position - transform.position;
        dir.y = 0;

        transform.rotation = Quaternion.LookRotation(dir);

        _rigidbody.velocity = transform.forward * enemyType.speed;
    }

    private bool IsAtTarget ()
    {
        return Vector3.Distance(transform.position, _pathPositions[_targetIndex].position) < 1f;
    }

    private void SwitchToNextTarget ()
    {
        if (_targetIndex >= _pathPositions.Count) return;
        _targetIndex++;
    }
    
    public void Damage(int damage)
    {
        if (_currentHealth - damage <= 0)
        {
            _currentHealth = 0;
            Die();
        }
        else
        {
            _currentHealth -= damage;
        }
    }

    public void InitPath (List<Transform> path)
    {
        _pathPositions = path;
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Base"))
        {
            LevelManager.Instance.DamageBase(enemyType.damage);
            Die();
        }
    }
}
