using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ExplodeOnImpact : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionForce = 5f;
    [SerializeField] private int explosionDamage = 5;
    [SerializeField] private VisualEffect explosionVFX;
    [SerializeField] private float vfxDuration = 5f;
    [SerializeField] private AudioClip explosionSound;
    
    private void OnCollisionEnter(Collision other)
    {
        Explode();
    }
    
    private void Explode()
    {
        if (explosionVFX)
        {
            VisualEffect vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            vfx.Play();
            Destroy(vfx, 5f);
        }
        
        if (explosionSound) SoundManager.Instance.PlaySFX(explosionSound, 0.5f);
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
            if (collider.TryGetComponent(out Enemy enemy))
            {
                enemy.Damage(explosionDamage);
            }
        }
        Destroy(gameObject);
    }
    
    public void SetDamage(int damage) => explosionDamage = damage;
}
