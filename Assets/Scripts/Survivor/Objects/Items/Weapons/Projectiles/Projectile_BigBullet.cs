using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_BigBullet : Projectile
{
    SpriteRenderer _sprite;
    TrailRenderer _trail;
    Collider2D _collider;
    Vector3 _destPos;
    float _dist;
    float _speed;
    bool _isDead;

    // 삭제
    float _destroyTime;
    float _accTime;

    public void Init(Vector3 dir, float attack)
    {
        _sprite = GetComponent<SpriteRenderer>();
        _trail = GetComponent<TrailRenderer>();
        _collider = GetComponent<Collider2D>();

        Attack = attack;
        AttackCount = 1;
        Effect = EffectType.BigBullet;

        _dist = 3.5f;
        _speed = 15.0f;
        _destPos = transform.position + (dir * _dist);
        
        _isDead = false;
        _sprite.enabled = true;
        _trail.emitting = true;
        _collider.enabled = true;

        // 삭제
        _destroyTime = _trail.time;
        _accTime = 0.0f;
    }

    private void Update()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, _destPos, _speed * Time.deltaTime);

        float dist = Vector2.Distance(transform.position, _destPos);

        if(dist < 0.02f)
        {
            _isDead = true;
            _collider.enabled = false;
            _sprite.enabled = false;
            _trail.emitting = false;
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
