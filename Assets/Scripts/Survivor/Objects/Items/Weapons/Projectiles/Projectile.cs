using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Survivor_Item
{
    public enum EffectType
    {
        Slash,
        Bullet,
        BigBullet,
        Blow,
        End
    }

    public float Attack { get; protected set; }
    public int AttackCount { get; protected set; } // Amount
    public float KnockBackPower { get; protected set; } // Amount
    public EffectType Effect { get; protected set; }
}
