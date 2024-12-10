using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Shotgun : Weapon
{
    public LayerMask _targetLayer;

    float _accTime;
    Transform _nearTarget = null;
    RaycastHit2D[] _targets;

    int _bulletCount;
    float _startAngle;
    float _diffAngle;
    float _moveSpeed;
    bool _isFire;

    Vector2 _offset;

    public override void Init()
    {
        gameObject.SetActive(true);

        _accTime = 0.0f;
        _bulletCount = 5;
        _startAngle = -30.0f;
        _diffAngle = 15.0f;
        _moveSpeed = 5.0f;
        _isFire = false;

        _offset = new Vector2(-0.75f, 0.75f);
    }

    public override void LevelUp(Define.AbilityType type)
    {
        switch (type)
        {
            case Define.AbilityType.CoolTime:
                _coolTimeLevel += 1;
                break;
            case Define.AbilityType.Attack:
                _attackLevel += 1;
                break;
        }
    }

    // 몬스터 탐색은 Physics를 사용하기에 FixedUpdate에서
    void FixedUpdate()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }

        if (_isFire == false)
        {

            _targets = Physics2D.CircleCastAll(Managers.GameManagerEx.Player.transform.position, 2.5f, Vector3.zero, 0, _targetLayer);
            _nearTarget = GetTarget();

            Vector3 playerPos = Managers.GameManagerEx.Player.transform.position;
            transform.position = new Vector2(playerPos.x + _offset.x, playerPos.y + _offset.y);
        }
    }

    Transform GetTarget()
    {
        Transform target = null;
        float nearDiff = float.MaxValue;

        foreach (RaycastHit2D enemy in _targets)
        {
            float diff = Vector3.Distance(transform.position, enemy.transform.position);
            if (diff < nearDiff)
            {
                nearDiff = diff;
                target = enemy.transform;
            }
        }

        return target;
    }

    void SetDir(Transform nearTarget)
    {
        if (nearTarget != null)
        {
            float angle = Mathf.Rad2Deg * (Mathf.Atan2(nearTarget.position.y - transform.position.y,
            nearTarget.position.x - transform.position.x));

            Vector3 playerDir = (Managers.GameManagerEx.Player.transform.right).normalized;
            Vector3 toTargetDir = (nearTarget.position - transform.position).normalized;

            if (Vector3.Dot(playerDir, toTargetDir) < 0)
            {
                transform.rotation = Quaternion.Euler(180.0f, 0.0f, -angle);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
            }
        }
    }


    void Update()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }

        _accTime += Time.deltaTime;
        if (_isFire == false && _nearTarget != null && _accTime > _itemData.coolTimes[_coolTimeLevel])
        {
            _isFire = true;
            StartCoroutine(FireReady(_nearTarget, () => { Fire(_nearTarget); }));
            _accTime = 0.0f;
        }
    }

    IEnumerator ReturnPos()
    {
        yield return new WaitForSeconds(0.25f);

        // 인스펙터 창의 Rotation은 보통 -180에서 180을 표현하지만, eulerAngles는 0에서 360을 표현한다. 
        // 인스펙터 상의 Rotation z가 -62라면, eulerAngles는.z는 308이 된다.
        // 샷건의 윈체스터 효과를 구현하고 싶다면, 각도가 180보다 큰지 작은지를 판단하면 될 것 같다.
        // 180보다 작다면 360 - 현재 각도, 180보다 크다면 360 + (360 - 현재 각도)

        float accAngle = 0.0f;
        float rotValue = transform.eulerAngles.z < 180.0f ?
            360.0f - transform.eulerAngles.z : 360.0f + (360.0f - transform.eulerAngles.z);

        float initAngle = transform.eulerAngles.z;

        Managers.SoundManager.PlaySFX("weaponSounds/ReLoad");

        while (true)
        {
            accAngle = Mathf.Lerp(accAngle, rotValue, 2.0f * Time.deltaTime);

            if(accAngle < rotValue)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, initAngle + accAngle);
            }

            Vector3 playerPos = Managers.GameManagerEx.Player.transform.position;
            Vector3 destPos = new Vector2(playerPos.x + _offset.x, playerPos.y + _offset.y);

            transform.position = Vector3.Lerp(transform.position, destPos, Time.deltaTime * _moveSpeed);

            if(Vector3.Distance(transform.position, destPos) <= 0.02f && rotValue - accAngle <= 1.0f)
            {
                _isFire = false;
                break;
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator FireReady(Transform nearTarget, Action action)
    {
        while(true)
        {
            SetDir(nearTarget);

            Vector3 dir = nearTarget.position - transform.position;

            if (dir.magnitude > 1.5f)
            {
                transform.position = Vector3.Lerp(transform.position, nearTarget.position, Time.deltaTime * _moveSpeed * 0.75f);
                yield return null;
            }
            else
            {
                break;
            }
        }

        yield return new WaitForSeconds(0.1f);

        SetDir(nearTarget); // 0.25초 대기 후, 총구를 타겟에 일치시키기 위함.

        action?.Invoke();
    }

    void Fire(Transform nearTarget)
    {
        for (int i = 0; i < _bulletCount; ++i)
        {
            Transform fireBullet = Managers.ResourceManager.Instantiate("Objects/Projectile_BigBullet").transform;

            Vector3 dir = nearTarget.position - transform.position;
            dir = dir.normalized;

            dir = Quaternion.Euler(0, 0, (_startAngle + (_diffAngle * i))) * dir;

            fireBullet.position = transform.position;
            fireBullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

            float attackRatio = Managers.GameManagerEx.Player.GetComponent<Player>().AttackRatio;
            fireBullet.GetComponent<Projectile_BigBullet>().Init(dir, attackRatio * _itemData.attacks[_attackLevel]);
        }

        Managers.SoundManager.PlaySFX("weaponSounds/ShotGun");
        StartCoroutine(ReturnPos());
    }
}
