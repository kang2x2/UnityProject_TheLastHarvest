using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class JsonDataSound
{
    public float bgmVolum;
    public float sfxVolum;
}

public class SoundData
{
    public JsonDataSound Data { get; set; } = new JsonDataSound();

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

    public void SoundDataOverwrite(float bgmVolum, float sfxVolum)
    {
        Data.bgmVolum = bgmVolum;
        Data.sfxVolum = sfxVolum;

        string json;
        json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(folderPath + "Sound.json", json);
    }

    private void JsonDataLoad()
    {
        string filePath = folderPath + "Sound.json";
        if (File.Exists(filePath) == false)
        {
            Data.bgmVolum = 100.0f;
            Data.sfxVolum = 100.0f;
            string json = JsonUtility.ToJson(Data, true);
            File.WriteAllText(filePath, json);
        }
        else
        {
            string json = File.ReadAllText(filePath);
            Data = JsonUtility.FromJson<JsonDataSound>(json);
        }
    }
}
