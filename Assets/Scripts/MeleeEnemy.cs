using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyController
{
    public override void Move()
    {
        _distance = Vector2.Distance(transform.position, _player.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * _direction, _rayLength, _wallLayer);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.right * _direction - Vector2.up , _rayLength, _wallLayer);

        if (hit.collider != null)
            _direction *= -1;
        if (hitDown.collider == null)
            _direction *= -1;

        Debug.DrawRay(transform.position, Vector2.right * _direction * _rayLength);

        if (_distance <= _chaseRange)
            _currentState = _enemyStates.chase;
        _enemyRigidbody.AddForce(Vector2.right * _direction * _speed * Time.deltaTime);
    }

    public override void Chase()
    {
        _distance = Vector2.Distance(transform.position, _player.transform.position);

        if (transform.position.x > _player.transform.position.x)
            _direction = -1;
        else  
            _direction = 1;

        if ( _distance >= _chaseRange)
            _currentState = _enemyStates.move;
        if ( _distance <= _attackRange)
            _currentState = _enemyStates.attack;
        
        _enemyRigidbody.AddForce(Vector2.right * _direction * _runSpeed * Time.deltaTime);
    }

    public override void Attack()
    {
        if (_attackCools <= 0)
        {
            _anim.SetBool("attack", true);
            Invoke("ResetAtack", 0.1f);
            _attackCools = _timeBetweenAttacks;
            _src.Play();
        }
        else 
            _currentState = _enemyStates.chase;
    }

    void ResetAtack()
    {
        _anim.SetBool("attack", false);
    }
    public override void Damage(float amt)
    {
        _health -= amt;
        if (_health <= 0)
            Die();
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
}
