using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_ExpBoost : Survivor_Item
{
    Player _player;
    float _defaultExpRatio;
    private void Start()
    {
        _player = transform.parent.GetComponent<Player>();
        _defaultExpRatio = _player.GetExpRatio;
    }
    public override void LevelUp(Define.AbilityType type)
    {
        _floatUtilLevel += 1;
        _player.GetExpRatio = _defaultExpRatio * _itemData.floatUtils[_floatUtilLevel];
    }
}
