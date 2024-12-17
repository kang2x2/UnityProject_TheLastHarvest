using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[CreateAssetMenu(fileName = "ItemStatInfo", menuName = "Scriptable Object/ItemStatInfo")]
public class Data_ItemStatInfo : ScriptableObject
{
    // ��� �ɷ�ġ �迭�� 0�� �ε����� Default Value.
    // 1�� �ε������� value ������.

    [Header("# Default Info")]
    public float[] speeds;
    public float[] coolTimes;
    public float[] fens;
    public int[] amounts;
    public float[] attacks;
    public float[] sizes;
    public float knockbackPower;

    [Header("# Util Info")]
    public float[] floatUtils; // �нú� ������ �� �� ���⸶���� ���� �ɷ�
    public int[] intUtils; // �нú� ������ �� �� ���⸶���� ���� �ɷ�
}
