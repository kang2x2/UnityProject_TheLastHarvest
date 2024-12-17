using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Shovel : Projectile
{
    public void Init(float attack, float knockBackPower)
    {
        Attack = attack;
        KnockBackPower = knockBackPower;
        AttackCount = 1;
        Effect = EffectType.Blow;
    }
}
