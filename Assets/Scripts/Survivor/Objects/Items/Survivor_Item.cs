using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor_Item : MonoBehaviour
{
    public Data_Item _itemData;
    protected int _amountLevel = 0;
    protected int _speedLevel = 0;
    protected int _attackLevel = 0;
    protected int _coolTimeLevel = 0;
    protected int _fenLevel = 0;
    protected int _sizeLevel = 0;
    protected int _floatUtilLevel = 0;
    protected int _intUtilLevel = 0;

    public virtual void Init()
    {

    }

    public virtual void LevelUp(Define.AbilityType type)
    {

    }

}
