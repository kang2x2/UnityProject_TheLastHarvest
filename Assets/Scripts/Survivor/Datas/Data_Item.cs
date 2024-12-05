using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("# Weapon Info")]
    public int maxLevel;
    public float[] speeds;
    public float[] coolTimes;
    public float[] fens;
    public int[] amounts;
    public float[] attacks;
    public float[] sizes;

    [Header("# Util Info")]
    public float[] floatUtils; // �нú� ������ �� �� ���⸶���� ���� �ɷ�
    public int[] intUtils; // �нú� ������ �� �� ���⸶���� ���� �ɷ�
}
