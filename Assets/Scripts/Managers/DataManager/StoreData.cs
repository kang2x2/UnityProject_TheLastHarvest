using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public struct PassiveItem
{
    public PassiveItem(string _spriteSheetPath, string _spriteName, string _desc,
        int _upgradIndex, int _level, float _addValue, int _price)
    {
        spriteSheetPath = _spriteSheetPath;
        spriteName = _spriteName;
        desc = _desc;
        upgradIndex = _upgradIndex;
        level = _level;
        addValue = _addValue;
        price = _price;
    }

    public string spriteSheetPath;
    public string spriteName;
    public string desc;
    public int upgradIndex;
    public int level;
    public float addValue;
    public int price;
}

[Serializable]
public class JsonDataStore
{
    public PassiveItem[] passiveItems;
}

public class StoreData
{
    public JsonDataStore Data { get; set; } = new JsonDataStore();

    string folderPath;
    public void Init()
    {
        folderPath = Application.persistentDataPath;
        if (File.Exists(folderPath) == false)
        {
            Directory.CreateDirectory(folderPath);
        }
        folderPath += "/";
        JsonDataLoad();
    }

    public void StoreDataOverwrite()
    {
        string json;
        json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(folderPath + "Store.json", json);
    }

    private void JsonDataLoad()
    {
        string filePath = folderPath + "Store.json";
        if (File.Exists(filePath) == false)
        {
            ItemDataInit();
            string json = JsonUtility.ToJson(Data, true);
            File.WriteAllText(filePath, json);
        }
        else
        {
            string json = File.ReadAllText(filePath);
            Data = JsonUtility.FromJson<JsonDataStore>(json);
        }
    }

    public void DataReset()
    {
        string filePath = folderPath + "Store.json";

        ItemDataInit();
        string json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(filePath, json);

        json = File.ReadAllText(filePath);
        Data = JsonUtility.FromJson<JsonDataStore>(json);
    }

    private void ItemDataInit()
    {
        Data.passiveItems = new PassiveItem[(int)Define.UserStatType.End];

        Data.passiveItems[(int)Define.UserStatType.Attack] =
            new PassiveItem("Sprites/UserStatUI", "UserStatUI_6", "���ݷ��� 2% �����մϴ�.",
            (int)Define.UserStatType.Attack, 0, 0.02f, 10);

        Data.passiveItems[(int)Define.UserStatType.MoveSpeed] =
            new PassiveItem("Sprites/UserStatUI", "UserStatUI_12", "�̵� �ӵ��� 5% �����մϴ�.",
            (int)Define.UserStatType.MoveSpeed, 0, 0.05f, 10);

        Data.passiveItems[(int)Define.UserStatType.Exp] =
            new PassiveItem("Sprites/UserStatUI", "UserStatUI_0", "����ġ ȹ�淮�� 5% �����մϴ�.",
            (int)Define.UserStatType.Exp, 0, 0.05f, 10);

        Data.passiveItems[(int)Define.UserStatType.MaxHP] =
            new PassiveItem("Sprites/UserStatUI", "UserStatUI_1", "�ִ� ü���� 5 �����մϴ�.",
            (int)Define.UserStatType.MaxHP, 0, 5.0f, 10);

        Data.passiveItems[(int)Define.UserStatType.Recovery] =
            new PassiveItem("Sprites/UserStatUI", "UserStatUI_7", "�ʴ� ȸ������ 0.2% �����մϴ�.",
            (int)Define.UserStatType.Recovery, 0, 0.2f, 10);

        Data.passiveItems[(int)Define.UserStatType.SelectCount] =
            new PassiveItem("Sprites/UserStatUI", "UserStatUI_11", "Level Up �� ���� ������ ������ ī�尡 1�� �߰��˴ϴ�.",
            (int)Define.UserStatType.SelectCount, 0, 1.0f, 50);

        Data.passiveItems[(int)Define.UserStatType.Critical] =
            new PassiveItem("Sprites/UserStatUI", "UserStatUI_16", "ġ��Ÿ Ȯ���� 0.25% �����մϴ�.",
            (int)Define.UserStatType.Recovery, 0, 0.25f, 10);

    }
}
