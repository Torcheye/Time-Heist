using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : TimeActor
{
    public float speed;
    public Transform throwPos;
    public Box box;
    public Light2D selfLight;

    public bool Equipped
    {
        get => _equipped;
        set => OnEquippedChanged(value);
    }
    private bool _equipped;
    private bool _isDead;
    private Animator _animator;
    private float _bloodLevel;
    private bool _isMoving;
    private bool _isReverseMoving;

    protected override void Awake()
    {
        base.Awake();
        
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(PlayFootsteps());
    }

    void Update()
    {
        if (_isDead)
        {
            if (_bloodLevel < .8f && Timer.Instance.forward)
                _bloodLevel += Time.deltaTime * 0.2f;

            _isMoving = false;
            UIManager.Instance.SetBloodLevel(_bloodLevel);
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Equipped = !Equipped;
        }

        if (box != null)
        {
            if (Equipped || (Timer.Instance.forward && box.GetInserted()))
            {
                box.GetComponent<Instruction>().ToggleInstruction(true, "E");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    box.ToggleInserted();
                    Equipped = !Equipped;
                }
            }
        }

        if (Equipped || Timer.Instance.forward)
            Move();
        else
            _isMoving = false;

        if (Equipped && Input.GetMouseButtonDown(0))
            Throw();
    }
    
    protected override void FixedUpdate()
    {
        if (Equipped)
        {
            PastStates.Clear();
            return;
        }

        base.FixedUpdate();
    }

    private void OnEquippedChanged(bool value)
    {
        _equipped = value;
        UIManager.Instance.ToggleChip(Equipped);
        selfLight.color = _equipped ? Color.cyan : Color.white;
    }

    private void Throw()
    {
        Equipped = false;
        
        var chip = GameManager.Instance.chip;
        chip.transform.position = throwPos.position;
        chip.SetActive(true);
        chip.GetComponent<Rigidbody2D>().AddForce(transform.up * 100);
    }

    private IEnumerator PlayFootsteps()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            if (_isMoving)
            {
                GameManager.Instance.sfxPlayer.PlayFootstep();
            }
            else if (_isReverseMoving)
            {
                GameManager.Instance.sfxPlayer.PlayFootstep(true);
            }
        }
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        var movement = y * Vector3.up + x * Vector3.right;

        _isMoving = movement.magnitude > 0.1f;
        _animator.SetBool("Walking", _isMoving);

        var rot = Quaternion.LookRotation(
            Vector3.forward, GameManager.Instance.aim.position - transform.position);
        
        Rigidbody2D.MoveRotation(rot);
        Rigidbody2D.MovePosition(transform.position + speed * Time.fixedDeltaTime *  movement);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            if (col.relativeVelocity.magnitude < 5)
                return;
            Collider2D.enabled = false;
            _isDead = true;
            GameManager.Instance.sfxPlayer.PlayBulletHit();
            if (_equipped)
                UIManager.Instance.Loose();
        }
        else if (col.gameObject.CompareTag("Chip"))
        {
            Equipped = true;
            col.gameObject.SetActive(false);
            GameManager.Instance.sfxPlayer.PlayCollectChip();
        }
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
            Dead_Player = _isDead,
            Blood_Player = _bloodLevel
        });
    }

    protected override void ReadRecordedState(out RecordState result)
    {
        base.ReadRecordedState(out result);
        _isDead = result.Dead_Player;
        _bloodLevel = result.Blood_Player;
        _isReverseMoving = Vector3.Distance(result.Position, transform.position) > 0.01f;
    }
}
