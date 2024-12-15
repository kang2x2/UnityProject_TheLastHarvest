using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Margent : Survivor_Item
{
    CircleCollider2D _collider;
    public override void Init()
    {
        _collider = GetComponent<CircleCollider2D>();

        // Player player = Managers.GameManagerEx.Player.GetComponent<Player>();
        // player.HasItem[(int)_itemData.itemType] = true;
    }

    public override void LevelUp(Define.AbilityType type)
    {
        _floatUtilLevel += 1;
        _collider.radius = _itemData.floatUtils[_floatUtilLevel];
    }
}
