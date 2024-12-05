using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_BoxUpgrade : Survivor_Item
{
    Player _player;
    private void Start()
    {
        _player = transform.parent.GetComponent<Player>();
    }
    public override void LevelUp(Define.AbilityType type)
    {
        _intUtilLevel += 1;
        _player.SelectBoxCount += 1;
    }
}
