using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamTower : Tower
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform beamOrigin;
    
    private bool _hasStartedAttackSound;
    private AudioSource _audioSource;
    
    protected override void Update()
    {
        base.Update();
        lineRenderer.SetPosition(0, beamOrigin.position);
        if (_targets.Count > 0)
        {
            lineRenderer.SetPosition(1, _targets[0].GetHitPoint());
            PlayAttackSound();
        }
        else
        {
            lineRenderer.SetPosition(1, beamOrigin.position);
            StopAttackSound();
            
        }
    }

    protected override void Attack(Enemy enemy)
    {
        enemy.Damage(settings.damage);
    }
    
    public override void Upgrade()
    {
        base.Upgrade();
        beamOrigin = upgrades[towerTier - 1].transform.Find("SpawningPos");
    }
    
    private void PlayAttackSound()
    {
        if (_hasStartedAttackSound) return;
        _hasStartedAttackSound = true;
        if (!_audioSource)
        {
            _audioSource = SoundManager.Instance.PlaySFXWithNewSource(settings.attackSound);
            _audioSource.playOnAwake = false;
            _audioSource.loop = true;
        }
        else
        {
            _audioSource.Play();
        }
    }
    
    private void StopAttackSound()
    {
        if (!_hasStartedAttackSound) return;
        _hasStartedAttackSound = false;
        _audioSource.Stop();
    }
}
