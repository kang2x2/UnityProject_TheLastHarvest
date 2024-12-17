using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Scythe : Weapon
{
    float _accTime;

    public bool IsSpawn { get; set; } = true;

    public override void Init()
    {
        base.Init();
        gameObject.SetActive(true);

        _accTime = 0.0f;
    }

    public override void LevelUp(Define.AbilityType type)
    {
        base.LevelUp(type);
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
            if (_accTime >= (float)_stats[(int)Define.AbilityType.CoolTime])
            {
                GameObject scythe = null;
                scythe = Managers.ResourceManager.Instantiate("Objects/Projectile_Scythe");

                // scythe.transform.parent = transform;

                scythe.transform.position = transform.position;
                scythe.transform.rotation = Quaternion.identity;

                float attackRatio = Managers.GameManagerEx.Player.GetComponent<Player>().AttackRatio;
                scythe.GetComponent<Projectile_Scythe>().Init(
                    (float)_stats[(int)Define.AbilityType.Attack] * attackRatio,
                    (int)_stats[(int)Define.AbilityType.Amount], _itemData.stat.knockbackPower, this);

                _accTime = 0.0f;
                IsSpawn = false;
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    private void LateUpdate()
    {
        Transform player = Managers.GameManagerEx.Player.transform;

        if (player != null)
        {
            if (player.GetComponent<SpriteRenderer>().flipX == true)
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 30.0f);
                transform.position = new Vector2(player.position.x - 0.1f, player.position.y - 0.2f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 30.0f);
                transform.position = new Vector2(player.position.x + 0.1f, player.position.y - 0.2f);
            }
        }
    }
}
