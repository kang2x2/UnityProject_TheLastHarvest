using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Shoose : Survivor_Item
{
    Player _player;
    float _defaultMoveSpeed;
    private void Start()
    {
        _player = transform.parent.GetComponent<Player>();
        _defaultMoveSpeed = _player.MoveSpeed;
    }
    public override void LevelUp(Define.AbilityType type)
    {
        _floatUtilLevel += 1;
        _player.MoveSpeed = _defaultMoveSpeed * _itemData.floatUtils[_floatUtilLevel];
    }
}
