using System;
using System.Collections;
using UnityEngine;

public class Code : TimeActor
{
    public GameObject door;
    public Sprite normal;
    public Sprite active;

    public bool Active
    {
        get => _active;
        private set => OnActiveChanged(value);
    }

    private Collider2D _doorCol;
    private Animator _doorAnim;
    private bool _active;
    private bool _notResponsive;

    protected override void Awake()
    {
        base.Awake();
        _doorCol = door.GetComponent<Collider2D>();
        _doorAnim = door.GetComponent<Animator>();
    }

    private void Update()
    {
        if (WithinDistance() && !_notResponsive &&
            (GameManager.Instance.playerComponent.Equipped || Timer.Instance.forward))
        {
            GetComponent<Instruction>().ToggleInstruction(true, "E");
            if (Input.GetKeyDown(KeyCode.E) && !Active)
            {
                Active = true;
            }
        }
        else
        {
            GetComponent<Instruction>().ToggleInstruction(false, "E");
        }
    }

    public override void OnChangeTimeDirection(bool to)
    {
        base.OnChangeTimeDirection(to);
        StopAllCoroutines();
    }

    private bool WithinDistance()
    {
        return Vector3.Distance(GameManager.Instance.player.transform.position,
            transform.position) < 1;
    }
    
    private void OnActiveChanged(bool value)
    {
        if (!_active && value)
        {
            SpriteRenderer.sprite = active;
            StartCoroutine(DoorOpen());
        }
        else if (_active && !value)
        {
            SpriteRenderer.sprite = normal;
            StartCoroutine(DoorClose());
        }
        _active = value;
    }

    private IEnumerator DoorOpen()
    {
        GameManager.Instance.sfxPlayer.PlayDoorOpen();
        _notResponsive = true;
        _doorAnim.SetBool("Opened", true);
        yield return new WaitForSeconds(0.3f);
        _doorCol.enabled = false;

        yield return new WaitForSeconds(1.8f);
        Active = false;
    }
    
    private IEnumerator DoorClose()
    {
        GameManager.Instance.sfxPlayer.PlayDoorClose();
        _doorAnim.SetBool("Opened", false);
        yield return new WaitForSeconds(0.3f);
        _doorCol.enabled = true;
        _notResponsive = false;
    }

    protected override void WriteRecordedState()
    {
        PastStates.Push(new RecordState()
        {
            Active_Code = Active
        });
    }

    protected override void ReadRecordedState(out RecordState result)
    {
        if (PastStates.TryPop(out result))
        {
            Active = result.Active_Code;
        }
    }
}