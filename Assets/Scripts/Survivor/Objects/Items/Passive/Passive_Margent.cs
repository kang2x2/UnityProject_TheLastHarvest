using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Margent : Survivor_Item
{
    CircleCollider2D _collider;
    public override void Init()
    {
        base.Init();
        _collider = GetComponent<CircleCollider2D>();
        _collider.radius = (float)_stats[(int)Define.AbilityType.FloatUtil];
    }

    public override void LevelUp(Define.AbilityType type)
    {
        base.LevelUp(type);
        _collider.radius = (float)_stats[(int)Define.AbilityType.FloatUtil];
    }
}
