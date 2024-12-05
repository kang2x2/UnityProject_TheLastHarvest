using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "Scriptable Object/MapData")]
public class Data_Map : ScriptableObject
{
    [Header("# Image Info")]
    public Sprite icon;

    [Header("# Base Info")]
    public new string name;
    public Define.MapType mapType;
}
