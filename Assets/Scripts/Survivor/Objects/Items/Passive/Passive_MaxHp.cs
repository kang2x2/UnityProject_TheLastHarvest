using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_MaxHp : Survivor_Item
{
    Player _player;
    public override void Init()
    {
        _player = transform.parent.GetComponent<Player>();
    }
    public override void LevelUp(Define.AbilityType type)
    {
        _floatUtilLevel += 1;
        _player.MaxHp += 5.0f;
    }
}
