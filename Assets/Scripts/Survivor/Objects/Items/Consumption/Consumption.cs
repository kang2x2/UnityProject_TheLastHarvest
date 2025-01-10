using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumption : Survivor_Item
{
    public override void Init()
    {
        base.Init();
        ItemType = Define.ItemType.Consumption;
    }
}


