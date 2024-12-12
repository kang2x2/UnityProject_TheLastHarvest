using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Scythe : Projectile
{
    Collider2D _collider;
    TrailRenderer _trail;
    Weapon_Scythe _parent;

    Vector2 _offset;
    Vector2 _attackScale;

    public void Init(float attack, int attackCount, Weapon_Scythe parent)
    {
        _collider = GetComponent<Collider2D>();
        _trail = GetComponent<TrailRenderer>();
        _collider.enabled = false;
        _trail.enabled = false;

        Attack = attack;
        AttackCount = attackCount;
        Effect = EffectType.Slash;

        _offset = new Vector2(-0.5f, 2.0f);
        _attackScale = new Vector3(2.0f, 2.0f, 2.0f);

        _parent = parent;

        StartCoroutine(AttackReady(() => {
            StartCoroutine(WeaponAttack(() => {
                StartCoroutine(ReturnWeapon()); 
            }));
        }));
    }

    IEnumerator AttackReady(Action action)
    {
        Transform player = Managers.GameManagerEx.Player.transform;

        float accTime = 0.0f;
        float moveTime = 0.5f;
        while (true)
        {
            float distX = Mathf.Abs(player.position.x - transform.position.x);

            if (player.GetComponent<SpriteRenderer>().flipX == true)
            {
                _offset.x = 0.5f;
                transform.position = new Vector2(player.position.x + distX, transform.position.y);
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }
            else
            {
                _offset.x = -0.5f;
                transform.position = new Vector2(player.position.x - distX, transform.position.y);
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }

            float t = accTime / moveTime;
            Vector2 destPos = new Vector2(player.position.x + _offset.x, player.position.y + _offset.y);
            transform.position = Vector2.Lerp(transform.position, destPos, t);
            transform.localScale = Vector2.Lerp(new Vector2(0.5f, 0.5f), _attackScale, t);

            accTime += Time.deltaTime;

            if (accTime < moveTime)
            {
                yield return null;
            }
            else
            {
                break;
            }
        }

        accTime = 0.0f;
        float destAngle = 60.0f;
        float curAngle = 0.0f;
        float rotTime = 0.5f;
        while (true)
        {
            transform.SetParent(_parent.transform);
            float t = accTime / rotTime;
            curAngle = Mathf.Lerp(curAngle, destAngle, t);
            transform.rotation = Quaternion.Euler(0.0f, transform.eulerAngles.y, curAngle);

            accTime += Time.deltaTime;
            if (accTime < rotTime)
            {
                yield return null;
            }
            else
            {
                transform.SetParent(null);
                break;
            }
        }

        action?.Invoke();
    }

    IEnumerator WeaponAttack(Action action)
    {
        Transform player = Managers.GameManagerEx.Player.transform;
        _collider.enabled = true;
        _trail.enabled = true;

        // 초기 시작 위치 계산.
        Vector2 dirToPlayer = player.position - transform.position;
        float curPosValue = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

        float destPosValue = 90.0f;

        // 각도 
        float curAngle = transform.eulerAngles.z;
        float destAngle = -180.0f;

        // 경과 시간
        float accTime = 0.0f;
        float attackTime = 0.15f;

        // 실시간 위치
        float curPosX;
        float curPosY;

        bool isRight = player.position.x > transform.position.x ? true : false;

        while (true)
        {
            float t = accTime / attackTime;
            curPosValue = Mathf.Lerp(curPosValue, destPosValue, t);
            curAngle = Mathf.Lerp(curAngle, destAngle, t);

            if (isRight == true) // Right
            {
                curPosX = player.position.x + (_offset.y * Mathf.Cos(curPosValue * (Mathf.PI / 180.0f)));
                curPosY = player.position.y - (_offset.y * Mathf.Sin(curPosValue * (Mathf.PI / 180.0f)));
            }
            else // Left
            {
                curPosX = player.position.x - (_offset.y * Mathf.Cos(curPosValue * (Mathf.PI / 180.0f)));
                curPosY = player.position.y - (_offset.y * Mathf.Sin(curPosValue * (Mathf.PI / 180.0f)));
            }

            transform.position = new Vector2(curPosX, curPosY);
            transform.rotation = Quaternion.Euler(0.0f, transform.eulerAngles.y, curAngle);

            accTime += Time.deltaTime;

            if (curPosValue < destPosValue)
            {
                yield return null;
            }
            else
            {
                break;
            }
        }

        _collider.enabled = false;
        _trail.emitting = false;
        action?.Invoke();
    }

    IEnumerator ReturnWeapon()
    {
        yield return new WaitForSeconds(0.1f);

        Transform player = Managers.GameManagerEx.Player.transform;

        float accTime = 0.0f;
        float moveTime = 0.5f;

        while (true)
        {
            float t = accTime / moveTime;
            transform.Rotate(Vector3.back * 2160.0f * Time.deltaTime);
            transform.position = Vector2.Lerp(transform.position, player.position, t);
            transform.localScale = Vector2.Lerp(_attackScale, Vector2.zero, t);

            accTime += Time.deltaTime;
            if (accTime < moveTime)
            {
                yield return null;
            }
            else
            {
                _trail.enabled = false;
                _parent.IsSpawn = true;
                _parent.GetComponent<SpriteRenderer>().enabled = true;
                Managers.ResourceManager.Destroy(gameObject);
                break;
            }
        }
    }
}
