using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class JsonDataCharacter
{
    public bool[] unLocks;
}

public class CharacterData
{
    public JsonDataCharacter Data { get; set; } = new JsonDataCharacter();

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

    public void CharacterDataOverwrite()
    {
        string json;
        json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(folderPath + "Character.json", json);
    }

    private void JsonDataLoad()
    {
        string filePath = folderPath + "Character.json";
        if (File.Exists(filePath) == false)
        {
            Data.unLocks = new bool[] { true, false, false, false };
            string json = JsonUtility.ToJson(Data, true);
            File.WriteAllText(filePath, json);
        }
        else
        {
            string json = File.ReadAllText(filePath);
            Data = JsonUtility.FromJson<JsonDataCharacter>(json);
        }
    }

    public IEnumerator UnLockCharacter(Action action)
    {
        UI_PopUp popUp;

        void PopUpSet(int index)
        {
            Data.unLocks[index] = true;
            Managers.UIManager.ShowPopUpUI("PopUpUI_Get", index);
            popUp = Managers.UIManager.CurPopUp;
        }

        if (Managers.GameManagerEx.ProgressTime >= 300.0f &&
            Data.unLocks[(int)Define.CharacterType.Character2] == false)
        {
            PopUpSet((int)Define.CharacterType.Character2);
            while (true)
            {
                if (popUp.IsShow == false)
                {
                    break;
                }
                yield return null;
            }
        }
        if (Managers.GameManagerEx.MapType == Define.MapType.Field &&
            Managers.GameManagerEx.IsClear == true &&
            Data.unLocks[(int)Define.CharacterType.Character3] == false)
        {
            PopUpSet((int)Define.CharacterType.Character3);
            while (true)
            {
                if (popUp.IsShow == false)
                {
                    break;
                }
                yield return null;
            }
        }
        if (Managers.GameManagerEx.MapType == Define.MapType.Cave &&
            Managers.GameManagerEx.IsClear == true &&
            Data.unLocks[(int)Define.CharacterType.Character4] == false)
        {
            PopUpSet((int)Define.CharacterType.Character4);
            while (true)
            {
                if (popUp.IsShow == false)
                {
                    break;
                }
                yield return null;
            }
        }

        CharacterDataOverwrite();

        action?.Invoke();
        yield return null;
    }

    public void DataReset()
    {
        string filePath = folderPath + "Character.json";

        Data.unLocks = new bool[] { true, false, false, false };
        string json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(filePath, json);

        json = File.ReadAllText(filePath);
        Data = JsonUtility.FromJson<JsonDataCharacter>(json);
    }
}
