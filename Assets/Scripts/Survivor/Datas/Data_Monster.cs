using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Scriptable Object/MonsterData")]
public class Data_Monster : ScriptableObject
{
    [Header("# Base Info")]
    public new string name;
    public Define.MonsterType type;
    public RuntimeAnimatorController Animator;

    [Header("# Stat Info")]
    public float hp;
    public float maxHp;
    public float attack;
    public float speed;
}
