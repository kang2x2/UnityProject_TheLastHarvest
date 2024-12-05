using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Object/CharacterData")]
public class Data_Character : ScriptableObject
{
    [Header("# Select Info")]
    public new string name;
    public Define.CharacterType type;
    public RuntimeAnimatorController selectAnimator;

    [TextArea]
    public string desc;

    [Header("# Game Info")]
    public RuntimeAnimatorController gameAnimator;
    public float speedBonus;
    public float attackBonus;
    public float expBonus;
}
