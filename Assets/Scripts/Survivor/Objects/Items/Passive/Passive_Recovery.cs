using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Recovery : Survivor_Item
{
    Player _player;
    float _defaultAttackRatio;
    public override void Init()
    {
        _player = transform.parent.GetComponent<Player>();
        _defaultAttackRatio = _player.RecoveryRatio;
    }
    public override void LevelUp(Define.AbilityType type)
    {
        _floatUtilLevel += 1;
        _player.RecoveryRatio = _defaultAttackRatio + 0.2f;
    }
}
