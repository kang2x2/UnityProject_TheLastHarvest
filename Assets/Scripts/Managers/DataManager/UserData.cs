using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class JsonDataUser
{
    public int gold;
    public float[] bonus;
}

public class UserData
{
    public JsonDataUser Data { get; set; } = new JsonDataUser();

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

    public void UserDataOverwrite()
    {
        string json;
        json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(folderPath + "User.json", json);
    }

    private void JsonDataLoad()
    {
        string filePath = folderPath + "User.json";
        if (File.Exists(filePath) == false)
        {
            Data.gold = 40;
            Data.bonus = new float[(int)Define.UserUpgradType.End];
            Data.bonus[(int)Define.UserUpgradType.Attack] = 0.0f;
            Data.bonus[(int)Define.UserUpgradType.MoveSpeed] = 0.0f;
            Data.bonus[(int)Define.UserUpgradType.Exp] = 0.0f;
            Data.bonus[(int)Define.UserUpgradType.MaxHP] = 0.0f;

            string json = JsonUtility.ToJson(Data, true);
            File.WriteAllText(filePath, json);
        }
        else
        {
            string json = File.ReadAllText(filePath);
            Data = JsonUtility.FromJson<JsonDataUser>(json);
        }
    }

    public void DataReset()
    {
        string filePath = folderPath + "User.json";

        Data.gold = 40;
        Data.bonus = new float[(int)Define.UserUpgradType.End];
        Data.bonus[(int)Define.UserUpgradType.Attack] = 0.0f;
        Data.bonus[(int)Define.UserUpgradType.MoveSpeed] = 0.0f;
        Data.bonus[(int)Define.UserUpgradType.Exp] = 0.0f;
        Data.bonus[(int)Define.UserUpgradType.MaxHP] = 0.0f;

        string json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(filePath, json);

        json = File.ReadAllText(filePath);
        Data = JsonUtility.FromJson<JsonDataUser>(json);
    }
}
