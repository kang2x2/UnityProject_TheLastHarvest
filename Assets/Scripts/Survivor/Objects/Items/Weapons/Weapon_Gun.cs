using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Gun : Weapon
{
    public LayerMask _targetLayer;

    float _accTime = 0.0f;
    Transform _nearTarget = null;
    RaycastHit2D[] _targets;

    Vector2 _offset;

    public override void Init()
    {
        base.Init();
        _offset = new Vector2(0.3f, -0.15f);
    }

    public override void LevelUp(Define.AbilityType type)
    {
        base.LevelUp(type);
    }

    // 몬스터 탐색은 Physics를 사용하기에 FixedUpdate에서
    void FixedUpdate()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }

        Vector3 playerPos = Managers.GameManagerEx.Player.transform.position;
        _targets = Physics2D.CircleCastAll(playerPos, 3.0f, Vector3.zero, 0, _targetLayer);
        _nearTarget = GetTarget(playerPos);

        float dir = Managers.GameManagerEx.Player.GetComponent<SpriteRenderer>().flipX == false ? 1.0f : -1.0f;

        transform.position = new Vector2(playerPos.x + (_offset.x * dir), playerPos.y + _offset.y);

        SetDir();
    }

    Transform GetTarget(Vector3 playerPos)
    {
        Transform target = null;
        float nearDiff = float.MaxValue;

        foreach (RaycastHit2D enemy in _targets)
        {
            float diff = Vector3.Distance(playerPos, enemy.transform.position);
            if (diff < nearDiff)
            {
                nearDiff = diff;
                target = enemy.transform;
            }
        }

        return target;
    }

    void SetDir()
    {
        if (_nearTarget != null)
        {
            float angle = Mathf.Rad2Deg * (Mathf.Atan2(_nearTarget.position.y - transform.position.y,
            _nearTarget.position.x - transform.position.x));

            Vector3 playerDir = (Managers.GameManagerEx.Player.transform.right).normalized;
            Vector3 toTargetDir = (_nearTarget.position - transform.position).normalized;

            Debug.DrawRay(transform.position, playerDir * 4.0f, Color.green);
            Debug.DrawRay(transform.position, toTargetDir * 4.0f, Color.red);

            if (Vector3.Dot(playerDir, toTargetDir) < 0)
            {
                transform.rotation = Quaternion.Euler(180.0f, 0.0f, -angle);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
            }

        }
        else
        {
            if(Managers.GameManagerEx.Player.GetComponent<SpriteRenderer>().flipX == true)
            {
                transform.rotation = Quaternion.Euler(180.0f, 0.0f, 180.0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
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
        if(_nearTarget != null)
        {
            if (_accTime > (float)_stats[(int)Define.AbilityType.CoolTime])
            {
                Fire();
                _accTime = 0.0f;
            }
        }
    }

    void Fire()
    {
        Transform fireBullet = Managers.ResourceManager.Instantiate("Objects/Projectile_Bullet").transform;

        Vector3 dir = _nearTarget.position - transform.position;
        dir = dir.normalized;

        fireBullet.position = transform.Find("Muzzle").gameObject.transform.position;
        fireBullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        float attackRatio = Managers.GameManagerEx.Player.GetComponent<Player>().AttackRatio;
        fireBullet.GetComponent<Projectile_Bullet>().Init(dir, DamageCalculator(),
            (float)_stats[(int)Define.AbilityType.Fen], _itemData.stat.knockbackPower);

        Managers.SoundManager.PlaySFX("weaponSounds/Rifle");
    }
}
