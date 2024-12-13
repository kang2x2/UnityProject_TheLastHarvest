using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Get", menuName = "Scriptable Object/GetCharacterData")]
public class Data_GetCharacter : ScriptableObject
{
    [Header("# Info")]
    public new string name;
    public string desc;
    public Sprite image;
}
