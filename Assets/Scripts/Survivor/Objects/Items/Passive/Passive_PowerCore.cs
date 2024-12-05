using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_PowerCore : Survivor_Item
{
    Player _player;
    float _defaultAttackRatio;
    private void Start()
    {
        _player = transform.parent.GetComponent<Player>();
        _defaultAttackRatio = _player.AttackRatio;
    }
    public override void LevelUp(Define.AbilityType type)
    {
        _floatUtilLevel += 1;
        _player.AttackRatio = _defaultAttackRatio * _itemData.floatUtils[_floatUtilLevel];
    }
}
