using System;
using System.Collections.Generic;
using UnityEngine;

public struct RecordState
{
    public Vector3 Position;
    public float Rotation;
    public bool Show;
    public bool Collide;
    public bool Alive;
    public float ShootTimer_Enemy;
    public bool Dead_Player;
    public float Blood_Player;
    public bool Active_Code;
}

public class TimeActor : MonoBehaviour
{
    protected Stack<RecordState> PastStates;
    protected bool Alive = true;
    protected Rigidbody2D Rigidbody2D;
    protected SpriteRenderer SpriteRenderer;
    protected Collider2D Collider2D;

    protected virtual void Awake()
    {
        PastStates = new Stack<RecordState>();
        TryGetComponent(out Rigidbody2D);
        TryGetComponent(out SpriteRenderer);
        TryGetComponent(out Collider2D);
    }

    protected virtual void FixedUpdate()
    {
        if (!Alive)
            Destroy(gameObject);

        if (Timer.Instance.forward)
            WriteRecordedState();
        else
            ReadRecordedState(out _);
    }

    private void PushAndReadRemaining()
    {
        while (PastStates.Count != 0)
        {
            ReadRecordedState(out _);
        }
    }

    public virtual void OnChangeTimeDirection(bool to)
    {
        if (to)
            PushAndReadRemaining();
    }

    public virtual void ToggleInstruction(string text)
    {
        
    }
    
    protected virtual void WriteRecordedState()
    {
        PastStates.Push(new RecordState()
        {
            Position = Rigidbody2D.position,
            Rotation = Rigidbody2D.rotation,
            Show = SpriteRenderer.enabled,
            Collide = Collider2D.enabled,
            Alive = Alive
        });
    }

    protected virtual void ReadRecordedState(out RecordState result)
    {
        if (PastStates.TryPop(out result))
        {
            if (!result.Alive)
                Destroy(gameObject);

            Rigidbody2D.MovePosition(result.Position);
            Rigidbody2D.MoveRotation(result.Rotation);
            SpriteRenderer.enabled = result.Show;
            Collider2D.enabled = result.Collide;
        }
    }
}