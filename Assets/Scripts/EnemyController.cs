using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float _maxHealth;
    protected float _health;
    public Image _healthImage;

    public float _speed;
    public float _runSpeed;
    public float _chaseRange;
    public float _attackRange;
    public enum _enemyStates { move, chase, attack }
    public _enemyStates _currentState = _enemyStates.move;

    protected Rigidbody2D _enemyRigidbody;
    public LayerMask _wallLayer;
    public float _rayLength;
    public int _direction;

    protected SpriteRenderer _rend;

    protected float _distance;
    protected PlayerController _player;

    public float _timeBetweenAttacks;
    protected float _attackCools;

    protected Animator _anim;
    protected AudioSource _src;
    void OnEnable()
    {
        _direction = (Random.value >= 0.5f) ? 1: -1;
        _health = _maxHealth;
        _attackCools = _timeBetweenAttacks;
    }
    void Awake()
    {
        _rend = GetComponent<SpriteRenderer>();
        _enemyRigidbody = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<PlayerController>();
        _anim = GetComponent<Animator>();
        _src = GetComponent<AudioSource>();
    }

    public virtual void Move(){ }
    public virtual void Chase(){ }
    public virtual void Attack(){ }
    public virtual void Damage(float amount){ }
    public virtual void Die(){ }

    // Update is called once per frame
    void Update()
    {
        _rend.flipX = (_direction == -1); 
        switch (_currentState)
        {
            case _enemyStates.move:
                Move();
                break;
            case _enemyStates.chase:
                Chase();
                break;
           case _enemyStates.attack:
                Attack();
                break;
        }

        if (_attackCools > 0)
            _attackCools -= Time.deltaTime;

        _healthImage.fillAmount = _health/_maxHealth;
    }
}
