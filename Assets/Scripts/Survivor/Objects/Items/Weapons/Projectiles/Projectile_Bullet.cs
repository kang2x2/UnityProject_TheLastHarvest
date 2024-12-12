using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Bullet : Projectile
{
    SpriteRenderer _sprite;
    Rigidbody2D _rigid;
    TrailRenderer _trail;
    Vector3 _dir;
    float _fen;
    float _speed;
    bool _isDead;

    // 삭제
    float _destroyTime;
    float _accTime;

    public void Init(Vector3 dir, float attack, float fen)
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        _trail = GetComponent<TrailRenderer>();

        Attack = attack;
        AttackCount = 1;
        Effect = EffectType.Bullet;

        _dir = dir;
        _fen = fen;
        _speed = 15.0f;
        _isDead = false;

        _rigid.simulated = true;
        _rigid.velocity = _dir * _speed;
        _sprite.enabled = true;
        _trail.emitting = true;

        // 삭제
        _destroyTime = _trail.time;
        _accTime = 0.0f;
    }

    private void FixedUpdate()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            _rigid.velocity = Vector2.zero;
            return;
        }

        if(_rigid.velocity.magnitude < _speed)
        {
            _rigid.velocity = _dir * _speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_isDead == false)
        {
            if (collision.CompareTag("Enemy"))
            {
                _fen -= 1;
                if (_fen <= 0)
                {
                    _rigid.simulated = false;
                    _sprite.enabled = false;
                    _trail.emitting = false;
                    _isDead = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_isDead == false)
        {
            if (collision.CompareTag("PlayerArea"))
            {
                _trail.emitting = false;
                _isDead = true;
            }
        }
    }

    private void LateUpdate()
    {
        if (Managers.GameManagerEx.IsPause == true)
        {
            return;
        }

        if (_isDead == true)
        {
            _accTime += Time.deltaTime;
            if (_accTime >= _destroyTime)
            {
                Managers.ResourceManager.Destroy(gameObject);
            }
        }
    }
}
