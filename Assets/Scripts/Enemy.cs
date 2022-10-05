using System;
using UnityEngine;

public class Enemy : TimeActor
{
    public GameObject bulletPrefab;
    public Transform shootPos;
    public const float SHOOT_CD = 3.5f;

    private float _shootTimer;
    
    private void Start()
    {
        _shootTimer = 2;
    }

    public void Shoot()
    {
        _shootTimer = 0;
        Instantiate(bulletPrefab, shootPos.position, transform.rotation);
        GameManager.Instance.sfxPlayer.PlayShoot();
    }

    private void Update()
    {
        if (!Timer.Instance.forward)
            return;
        
        var target = GameManager.Instance.player;
        var dir = (target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        
        _shootTimer += Time.deltaTime;
        if (_shootTimer >= SHOOT_CD)
            Shoot();
    }
    
    protected override void WriteRecordedState()
    {
        PastStates.Push(new RecordState()
        {
            Position = Rigidbody2D.position,
            Rotation = Rigidbody2D.rotation,
            Show = SpriteRenderer.enabled,
            Collide = Collider2D.enabled,
            Alive = Alive,
            ShootTimer_Enemy = _shootTimer
        });
    }

    protected override void ReadRecordedState(out RecordState result)
    {
        base.ReadRecordedState(out result);
        _shootTimer = result.ShootTimer_Enemy;
    }
}
