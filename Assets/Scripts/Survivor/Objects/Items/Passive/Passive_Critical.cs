using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Critical : Passive
{
    Player _player;
    float defaultValue;
    public override void Init()
    {
        base.Init();
        _player = transform.parent.GetComponent<Player>();
        defaultValue = _player.CriticalRatio;
        _player.CriticalRatio = defaultValue + (float)_stats[(int)Define.AbilityType.FloatUtil];
    }
    public override void LevelUp(Define.AbilityType type)
    {
        base.LevelUp(type);
        _player.CriticalRatio = defaultValue + (float)_stats[(int)Define.AbilityType.FloatUtil];
    }
}
