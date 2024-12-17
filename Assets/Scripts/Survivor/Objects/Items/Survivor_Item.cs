using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor_Item : MonoBehaviour
{
    public Data_Item _itemData;
    protected int[] _levels = new int[(int)Define.AbilityType.End];
    protected object[] _stats = new object[(int)Define.AbilityType.End];

    public virtual void Init() 
    {
        if (_itemData.stat.amounts.Length > 0)
            _stats[(int)Define.AbilityType.Amount] = _itemData.stat.amounts[0];

        if (_itemData.stat.attacks.Length > 0)
            _stats[(int)Define.AbilityType.Attack] = _itemData.stat.attacks[0];

        if (_itemData.stat.coolTimes.Length > 0)
            _stats[(int)Define.AbilityType.CoolTime] = _itemData.stat.coolTimes[0];

        if (_itemData.stat.fens.Length > 0)
            _stats[(int)Define.AbilityType.Fen] = _itemData.stat.fens[0];

        if (_itemData.stat.sizes.Length > 0)
            _stats[(int)Define.AbilityType.Size] = _itemData.stat.sizes[0];

        if (_itemData.stat.speeds.Length > 0)
            _stats[(int)Define.AbilityType.Speed] = _itemData.stat.speeds[0];

        if (_itemData.stat.floatUtils.Length > 0)
            _stats[(int)Define.AbilityType.FloatUtil] = _itemData.stat.floatUtils[0];

        if (_itemData.stat.intUtils.Length > 0)
            _stats[(int)Define.AbilityType.IntUtil] = _itemData.stat.intUtils[0];
    }

    public virtual void LevelUp(Define.AbilityType type)
    {
        _levels[(int)type] += 1;

        switch(type)
        {
            case Define.AbilityType.Amount:
                int amount = (int)_stats[(int)type];
                _stats[(int)type] = amount + _itemData.stat.amounts[_levels[(int)type]];
                break;

            case Define.AbilityType.Attack:
                float attack = (float)_stats[(int)type];
                _stats[(int)type] = attack + _itemData.stat.attacks[_levels[(int)type]];
                break;

            case Define.AbilityType.CoolTime:
                float coolTime = (float)_stats[(int)type];
                _stats[(int)type] = coolTime - _itemData.stat.coolTimes[_levels[(int)type]];
                break;

            case Define.AbilityType.Fen:
                float fen = (float)_stats[(int)type];
                _stats[(int)type] = fen + _itemData.stat.fens[_levels[(int)type]];
                break;

            case Define.AbilityType.Size:
                float size = (float)_stats[(int)type];
                _stats[(int)type] = size + _itemData.stat.sizes[_levels[(int)type]];
                break;

            case Define.AbilityType.Speed:
                float speed = (float)_stats[(int)type];
                _stats[(int)type] = speed + _itemData.stat.speeds[_levels[(int)type]];
                break;

            case Define.AbilityType.FloatUtil:
                float floatUtil = (float)_stats[(int)type];
                _stats[(int)type] = floatUtil + _itemData.stat.floatUtils[_levels[(int)type]];
                break;

            case Define.AbilityType.IntUtil:
                int intUtil = (int)_stats[(int)type];
                _stats[(int)type] = intUtil + _itemData.stat.intUtils[_levels[(int)type]];
                break;
        }
    }
}
