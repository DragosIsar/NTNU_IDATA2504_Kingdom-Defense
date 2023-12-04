using static GameManager.Tags;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BuffAura : MonoBehaviour
{
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private float buffMultiplier;
    [SerializeField] private float buffInterval;
    [SerializeField] private float radius;
    [SerializeField] private VisualEffect vfx;
    
    private float _timeToBuff;
    private List<Enemy> _targets = new();
    
    private void Awake()
    {
        sphereCollider.radius = radius;
    }
    
    private void OnEnable()
    {
        _timeToBuff = buffInterval;
    }
    
    private void Update()
    {
        _timeToBuff -= Time.deltaTime;
        if (_timeToBuff <= 0)
        {
            _timeToBuff = buffInterval;
            BuffEnemies();
        }
    }
    
    private void BuffEnemies()
    {
        foreach (Enemy enemy in _targets)
        {
            enemy.Buff(buffMultiplier);
            Instantiate(vfx, enemy.transform.position + (Vector3.down * .2f), Quaternion.identity, enemy.transform);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(ENEMY)) return;
        if (!other.TryGetComponent(out Enemy enemy)) return;
        
        if (!_targets.Contains(enemy))
        {
            _targets.Add(enemy);
            enemy.OnDeath += () =>
            {
                _targets.Remove(enemy);
            };
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(ENEMY)) return;
        if (!other.TryGetComponent(out Enemy enemy)) return;
        
        if (_targets.Contains(enemy))
        {
            _targets.Remove(enemy);
        }
    }
}
