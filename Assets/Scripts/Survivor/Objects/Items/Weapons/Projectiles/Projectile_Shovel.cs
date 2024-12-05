using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Shovel : Projectile
{
    public void Init(float attack)
    {
        Attack = attack;
        Effect = EffectType.Slash;
    }
}
