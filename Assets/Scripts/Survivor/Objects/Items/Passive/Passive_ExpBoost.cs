using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_ExpBoost : Survivor_Item
{
    Player _player;
    float defaultValue;
    public override void Init()
    {
        base.Init();
        _player = transform.parent.GetComponent<Player>();
        defaultValue = _player.GetExpRatio;
        _player.GetExpRatio = defaultValue + (float)_stats[(int)Define.AbilityType.FloatUtil];
    }
    public override void LevelUp(Define.AbilityType type)
    {
        base.LevelUp(type);
        _player.GetExpRatio = defaultValue + (float)_stats[(int)Define.AbilityType.FloatUtil];
    }
}
