using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData" )]
public class Data_Item : ScriptableObject
{
    [Header("# Type Info")]
    public Define.ItemType itemType;
    public Define.AbilityType abilityType;

    [Header("# Text Info")]
    public new string name;
    public string itemDesc;
    public string abilityDesc;

    [Header("# Image Info")]
    public Sprite icon;

    [Header("# Stat Info")]
    public int maxLevel;
    public Data_ItemStatInfo stat;

}
