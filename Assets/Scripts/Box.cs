using UnityEngine;

public class Box : TimeActor
{
    public Sprite normal;
    public Sprite inserted;

    private bool _inserted;

    public void ToggleInserted()
    {
        _inserted = !_inserted;
        SpriteRenderer.sprite = _inserted ? inserted : normal;
    }

    public bool GetInserted()
    {
        return _inserted;
    }
    
    protected override void FixedUpdate()
    {
        if (_inserted)
        {
            PastStates.Clear();
            return;
        }

        base.FixedUpdate();
    }
}