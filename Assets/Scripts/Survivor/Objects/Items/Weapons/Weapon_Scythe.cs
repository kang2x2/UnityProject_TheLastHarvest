using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Scythe : Weapon
{
    float _shootTime;
    float _accTime;

    Vector2 _offset;

    public bool IsSpawn { get; set; } = true;

    public override void Init()
    {
        gameObject.SetActive(true);

        _accTime = 0.0f;
        _shootTime = 2.0f;

        _offset = new Vector2(0.1f, -0.2f);
    }

    public override void LevelUp(Define.AbilityType type)
    {
        switch (type)
        {
            case Define.AbilityType.Size:
                _sizeLevel += 1;
                break;
            case Define.AbilityType.Speed:
                _speedLevel += 1;
                break;
            case Define.AbilityType.Attack:
                _attackLevel += 1;
                break;
        }
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
            if (_accTime >= _shootTime)
            {
                GameObject scythe = null;
                scythe = Managers.ResourceManager.Instantiate("Objects/Projectile_Scythe");

                scythe.transform.parent = transform;

                scythe.transform.localPosition = Vector3.zero;
                scythe.transform.localRotation = Quaternion.identity;
                scythe.transform.localScale = new Vector3(_itemData.sizes[_sizeLevel], _itemData.sizes[_sizeLevel], _itemData.sizes[_sizeLevel]);

                float attackRatio = Managers.GameManagerEx.Player.GetComponent<Player>().AttackRatio;
                scythe.GetComponent<Projectile_Scythe>().Init(2.5f, 
                    _itemData.speeds[_speedLevel], _itemData.attacks[_attackLevel] * attackRatio);

                _accTime = 0.0f;
                IsSpawn = false;
            }
        }
    }
}
