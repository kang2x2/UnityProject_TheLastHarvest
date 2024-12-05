using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn", menuName = "Scriptable Object/SpawnData")]
public class Data_Spawn : ScriptableObject
{
    [Header("# Exp Info")]
    public int[] destExpChangeLevels;
    public float[] destExps;

    [Header("# SpawnTime Info")]
    public int[] spawnTimeChangeLevels;
    public float[] spawnTimes;

    [Header("# SpawnType Info")]
    public int[] spawnTypeChangeLevels;
    public Define.MonsterType[] minTypes;
    public Define.MonsterType[] maxTypes;
}
