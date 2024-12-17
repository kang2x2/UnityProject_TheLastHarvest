using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[CreateAssetMenu(fileName = "ItemStatInfo", menuName = "Scriptable Object/ItemStatInfo")]
public class Data_ItemStatInfo : ScriptableObject
{
    // 모든 능력치 배열의 0번 인덱스는 Default Value.
    // 1번 인덱스부턴 value 증가량.

    [Header("# Default Info")]
    public float[] speeds;
    public float[] coolTimes;
    public float[] fens;
    public int[] amounts;
    public float[] attacks;
    public float[] sizes;
    public float knockbackPower;

    [Header("# Util Info")]
    public float[] floatUtils; // 패시브 아이템 및 각 무기마다의 고유 능력
    public int[] intUtils; // 패시브 아이템 및 각 무기마다의 고유 능력
}
