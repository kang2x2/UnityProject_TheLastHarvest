using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Survivor_Item
{
    protected GameObject _originalProjectile = null;

    public override void Init()
    {
        base.Init();
        ItemType = Define.ItemType.Weapon;
    }

    protected float DamageCalculator()
    {
        float playerAtkRatio = Managers.GameManagerEx.Player.GetComponent<Player>().AttackRatio;
        float weaponAtkRatio = Random.Range(0.9f, 1.1f);

        float weaponDamage = weaponAtkRatio * (float)_stats[(int)Define.AbilityType.Attack];
        float resultDamage = playerAtkRatio * weaponDamage;

        return resultDamage;
    }
}


