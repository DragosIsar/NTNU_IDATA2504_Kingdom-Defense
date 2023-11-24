using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour
{
    public event Action OnDeath;
    
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform hitPointTransform;
    
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hitSound;

    private int _currentHealth;
    private Rigidbody _rigidbody;
    private int _targetIndex;
    private List<Transform> _pathPositions;
    
    private static readonly int _GET_HIT = Animator.StringToHash("getHit");

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
        if (_targetIndex < _pathPositions.Count-1) _targetIndex++;
        Debug.Log(_targetIndex);
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
            
        animator.SetTrigger(_GET_HIT);
        if (hitSound) SoundManager.Instance.PlaySFX(hitSound);
    }

    public void InitPath (List<Transform> path)
    {
        _pathPositions = path;
    }
    
    private void Die()
    {
        if (deathSound) SoundManager.Instance.PlaySFX(deathSound);
        OnDeath?.Invoke();
        LevelManager.Instance.CollectCurrency(enemyType.reward);
        if (enemyType.deathEffect != null)
        {
            VisualEffect deathEffect = Instantiate(enemyType.deathEffect, transform.position + Vector3.up, Quaternion.identity);
            deathEffect.Play();
            Destroy(deathEffect.gameObject, 3f);
        }
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

    public Vector3 GetHitPoint()
    {
        return hitPointTransform ? hitPointTransform.position : transform.position + Vector3.up;
    }

    public Vector3 GetVelocity()
    {
        return _rigidbody.velocity;
    }

    public int GetHealth()
    {
        return _currentHealth;
    }
}