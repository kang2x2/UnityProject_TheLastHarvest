using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Scythe : Projectile
{
    float _offsetY;
    float _atkOffset;
    float _speed;
    float _angle;

    bool _isFlip;

    Collider2D _collider;

    public void Init(float attack)
    {
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;

        Attack = attack;
        Effect = EffectType.Slash;

        _offsetY = 1.75f;
        _speed = 10.0f;
        _isFlip = false;

        StartCoroutine(AttackReady(() => {
            StartCoroutine(WeaponAttack(() => {
                StartCoroutine(ReturnWeapon()); 
            }));
        }));
    }

    private void Update()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }
    }

    IEnumerator AttackReady(Action action)
    {
        float moveSpeed = 3.0f;
        Transform player = Managers.GameManagerEx.Player.transform;

        while (true)
        {
            Vector3 destPos = new Vector3(player.position.x, player.position.y + _offsetY, player.position.z);
            transform.position = Vector3.MoveTowards(transform.position, destPos, moveSpeed * Time.deltaTime);
            transform.Rotate(Vector3.back * 2160.0f * Time.deltaTime);

            if (Vector2.Distance(transform.position, destPos) >= 0.02f)
            {
                yield return null;
            }
            else
            {
                break;
            }
        }

        _isFlip = true;
        yield return new WaitForSeconds(0.1f);

        action?.Invoke();
    }

    IEnumerator WeaponAttack(Action action)
    {
        _collider.enabled = true;

        // 초기 회전 각도 계산, 시작 각도는 -90도이지만 언제 바뀔 지 모르기에 수식으로.
        Vector3 dirToPlayer = Managers.GameManagerEx.Player.transform.position - transform.position;
        float initialAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

        _angle = initialAngle;
        float destX = 0.0f;
        float destY = 0.0f;

        bool _isRight = transform.eulerAngles.y > 0 ? true : false;
        _isFlip = false;

        while (true)
        {
            Vector3 center = Managers.GameManagerEx.Player.transform.position;

            _angle += _speed / 4;
            
            if (_isRight == true) // Right
            {
                destX = center.x - (_offsetY * Mathf.Cos(_angle * (Mathf.PI / 180.0f)));
                destY = center.y - (_offsetY * Mathf.Sin(_angle * (Mathf.PI / 180.0f)));
            }
            else // Left
            {
                destX = center.x + (_offsetY * Mathf.Cos(_angle * (Mathf.PI / 180.0f)));
                destY = center.y - (_offsetY * Mathf.Sin(_angle * (Mathf.PI / 180.0f)));
            }

            transform.position = new Vector2(destX, destY);
            // transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + _angle);

            if (_angle < 90.0f)
            {
                yield return null;
            }
            else
            {
                break;
            }
        }

        _collider.enabled = false;
        action?.Invoke();
    }

    IEnumerator ReturnWeapon()
    {
        yield return new WaitForSeconds(0.1f);

        float moveSpeed = 3.0f;
        Transform player = Managers.GameManagerEx.Player.transform;

        while (true)
        {
            transform.Rotate(Vector3.back * 2160.0f * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.position) >= 0.02f)
            {
                yield return null;
            }
            else
            {
                transform.parent.GetComponent<Weapon_Scythe>().IsSpawn = true;
                Managers.ResourceManager.Destroy(gameObject);
                break;
            }
        }
    }

    private void LateUpdate()
    {
        Transform player = Managers.GameManagerEx.Player.transform;
        if(player != null && _isFlip == true)
        {
            if (player.GetComponent<SpriteRenderer>().flipX == true)
            {
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 45.0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 45.0f);
            }
        }
    }
}
