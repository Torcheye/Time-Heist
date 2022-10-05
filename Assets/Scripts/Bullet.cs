using System;
using UnityEngine;

public class Bullet : TimeActor
{
    public float speed;
    private void Start()
    {
        PastStates.Push(new RecordState()
        {
            Position = Rigidbody2D.position,
            Show = false,
            Collide = false,
            Alive = false
        });
        
        Rigidbody2D.AddForce(speed * transform.up, ForceMode2D.Impulse);
    }
    
    protected override void ReadRecordedState(out RecordState result)
    {
        if (PastStates.TryPop(out result))
        {
            if (!result.Alive)
            {
                GameManager.Instance.sfxPlayer.PlayShoot(true);
                Destroy(gameObject);
            }
            
            if (!GameManager.Instance.InInsertedBoxRange(result.Position))
                Rigidbody2D.MovePosition(result.Position);
            
            Rigidbody2D.MoveRotation(result.Rotation);
            SpriteRenderer.enabled = result.Show;
            Collider2D.enabled = result.Collide;
        }
    }
}