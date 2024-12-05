using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Thompson : Weapon
{
    public LayerMask _targetLayer;

    float _accTime;
    float _fireTime;
    Transform _nearTarget = null;
    RaycastHit2D[] _targets;

    Vector2 _offset;

    public override void Init()
    {
        gameObject.SetActive(true);

        _accTime = 0.0f;
        _fireTime = 3.5f;
        _offset = new Vector2(0.75f, 0.75f);
    }

    public override void LevelUp(Define.AbilityType type)
    {
        switch (type)
        {
            case Define.AbilityType.Amount:
                _amountLevel += 1;
                break;
            case Define.AbilityType.Fen:
                _fenLevel += 1;
                break;
            case Define.AbilityType.Attack:
                _attackLevel += 1;
                break;
        }
    }

    void FixedUpdate()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }

        _targets = Physics2D.CircleCastAll(transform.position, 4.5f, Vector3.zero, 0, _targetLayer);
        _nearTarget = GetTarget();

        Vector3 playerPos = Managers.GameManagerEx.Player.transform.position;
        transform.position = new Vector2(playerPos.x + _offset.x, playerPos.y + _offset.y);

        SetDir();
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

    void SetDir()
    {
        if (_nearTarget != null)
        {
            float angle = Mathf.Rad2Deg * (Mathf.Atan2(_nearTarget.position.y - transform.position.y,
            _nearTarget.position.x - transform.position.x));

            Vector3 playerDir = (Managers.GameManagerEx.Player.transform.right).normalized;
            Vector3 toTargetDir = (_nearTarget.position - transform.position).normalized;

            if (Vector3.Dot(playerDir, toTargetDir) < 0)
            {
                transform.rotation = Quaternion.Euler(180.0f, 0.0f, -angle);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
            }

        }
        //else
        //{
        //    if (Managers.GameManagerEx.Player.GetComponent<SpriteRenderer>().flipX == true)
        //    {
        //        transform.rotation = Quaternion.Euler(180.0f, 0.0f, 180.0f);
        //    }
        //    else
        //    {
        //        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        //    }
        //}
    }


    void Update()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }

        _accTime += Time.deltaTime;
        if (_nearTarget != null && _accTime > _fireTime)
        {
            IEnumerator coFire = Fire();
            StartCoroutine(coFire);
            _accTime = 0.0f;
        }
    }

    IEnumerator Fire()
    {
        for(int i = 0; i < _itemData.amounts[_amountLevel]; ++i)
        {
            if(_nearTarget == null)
            {
                break;
            }

            Transform fireBullet = Managers.ResourceManager.Instantiate("Objects/Projectile_Bullet").transform;

            Vector3 dir = _nearTarget.position - transform.position;
            dir = dir.normalized;

            float randomAngle = Random.Range(-15.0f, 15.0f);
            dir = Quaternion.Euler(0, 0, randomAngle) * dir;

            fireBullet.position = transform.Find("Muzzle").gameObject.transform.position;
            fireBullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

            float attackRatio = Managers.GameManagerEx.Player.GetComponent<Player>().AttackRatio;
            fireBullet.GetComponent<Projectile_Bullet>().
                Init(dir, attackRatio * _itemData.attacks[_attackLevel], _itemData.fens[_fenLevel]);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
