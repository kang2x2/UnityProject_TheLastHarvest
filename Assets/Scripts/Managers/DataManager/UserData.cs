using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class JsonDataUser
{
    public int gold;
    public int selectCount;
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

    // Float -> RoundFloat
    public float GetUserStat_Float(Define.UserStatType type, int digit = 2)
    {
        return (float)Math.Round(Data.bonus[(int)type], digit);
    }
    // Float -> RoundInt
    public int GetUserStat_Int(Define.UserStatType type, int digit = 2)
    {
        float stat = (float)Math.Round(Data.bonus[(int)type], digit);

        if(digit != 0)
        {
            // return (int)(stat * 100.0f); 캐스팅 과정에서 부동 소수점 문제가 발생.
            return Mathf.RoundToInt(stat * 100.0f);
        }
        else
        {
            return Mathf.RoundToInt(stat);
        }
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
            Data.gold = 0;
            Data.selectCount = 3;
            Data.bonus = new float[(int)Define.UserStatType.End];
            Data.bonus[(int)Define.UserStatType.Attack] = 0.0f;
            Data.bonus[(int)Define.UserStatType.MoveSpeed] = 0.0f;
            Data.bonus[(int)Define.UserStatType.Exp] = 0.0f;
            Data.bonus[(int)Define.UserStatType.MaxHP] = 0.0f;
            Data.bonus[(int)Define.UserStatType.Recovery] = 0.0f;
            Data.bonus[(int)Define.UserStatType.Critical] = 0.0f;

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

        Data.gold = 0;
        Data.selectCount = 3;
        Data.bonus = new float[(int)Define.UserStatType.End];
        Data.bonus[(int)Define.UserStatType.Attack] = 0.0f;
        Data.bonus[(int)Define.UserStatType.MoveSpeed] = 0.0f;
        Data.bonus[(int)Define.UserStatType.Exp] = 0.0f;
        Data.bonus[(int)Define.UserStatType.MaxHP] = 0.0f;
        Data.bonus[(int)Define.UserStatType.Recovery] = 0.0f;
        Data.bonus[(int)Define.UserStatType.Critical] = 0.0f;

        string json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(filePath, json);

        json = File.ReadAllText(filePath);
        Data = JsonUtility.FromJson<JsonDataUser>(json);
    }
}
