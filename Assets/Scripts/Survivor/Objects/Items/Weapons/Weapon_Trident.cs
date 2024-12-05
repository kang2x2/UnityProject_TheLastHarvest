using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Trident : Weapon
{
    public LayerMask _targetLayer;
    Transform _nearTarget = null;
    RaycastHit2D[] _targets;

    float _shootTime = 5.0f;
    float _accTime = 0.0f;
    public bool IsSpawn { get; set; } = true;

    public override void Init()
    {
        gameObject.SetActive(true);
    }

    public override void LevelUp(Define.AbilityType type)
    {
        switch (type)
        {
            case Define.AbilityType.Attack:
                _attackLevel += 1;
                break;
            case Define.AbilityType.IntUtil:
                _intUtilLevel += 1;
                break;
        }
    }

    void FixedUpdate()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }
        _targets = Physics2D.CircleCastAll(transform.position, 2.5f, Vector3.zero, 0, _targetLayer);
        _nearTarget = GetTarget();
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


    void Update()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }

        if (IsSpawn == true)
        {
            _accTime += Time.deltaTime;
            if(_nearTarget != null && _accTime > _shootTime)
            {
                _accTime = 0.0f;
                IsSpawn = false;
                Fire();
            }
        }
    }

    private void LateUpdate()
    {
        if(Managers.GameManagerEx.Player.GetComponent<SpriteRenderer>().flipX == true)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 10.0f);
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, -10.0f);
        }
    }

    void Fire()
    {
        Transform trident = null;
        trident = Managers.ResourceManager.Instantiate("Objects/Projectile_Trident").transform;

        Vector3 dir = _nearTarget.position - transform.position;
        dir = dir.normalized;

        trident.position = transform.position;
        trident.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        float attackRatio = Managers.GameManagerEx.Player.GetComponent<Player>().AttackRatio;
        trident.GetComponent<Projectile_Trident>().Init(_itemData.intUtils[_intUtilLevel],
            attackRatio * _itemData.attacks[_attackLevel], dir, transform);
    }
}
