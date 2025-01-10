using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Survivor_Item
{
    protected GameObject _originalProjectile = null;

    public override void Init()
    {
        base.Init();
        ItemType = Define.ItemType.Weapon;
    }
}


