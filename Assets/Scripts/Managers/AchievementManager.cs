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

public class AchievementManager
{
    public JsonDataCharacter Character { get; set; } = new JsonDataCharacter();

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

    public void JsonDataOverwrite(Define.JsonDataType type)
    {
        string json;

        switch (type)
        {
            case Define.JsonDataType.Character:
                json = JsonUtility.ToJson(Character);
                File.WriteAllText(folderPath + "Character.json", json);
                break;
        }
    }

    private void JsonDataLoad()
    {
        string filePath = folderPath + "Character.json";
        if (File.Exists(filePath) == false)
        {
            Character.unLocks = new bool[] { true, false, false, false };
            string json = JsonUtility.ToJson(Character);
            File.WriteAllText(filePath, json);
        }
        else
        {
            string json = File.ReadAllText(filePath);
            Character = JsonUtility.FromJson<JsonDataCharacter>(json);
        }
    }

    public IEnumerator UnLockCharacter(Action action)
    {
        UI_PopUp popUp;

        void PopUpSet(int index)
        {
            Character.unLocks[index] = true;
            Managers.UIManager.ShowPopUpUI("PopUpUI_Get", index);
            popUp = Managers.UIManager.CurPopUp;
        }

        if (Managers.GameManagerEx.ProgressTime >= 300.0f &&
            Character.unLocks[(int)Define.CharacterType.Character2] == false)
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
            Character.unLocks[(int)Define.CharacterType.Character3] == false)
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
            Character.unLocks[(int)Define.CharacterType.Character4] == false)
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

        JsonDataOverwrite(Define.JsonDataType.Character);

        action?.Invoke();
        yield return null;
    }

    public void DataReset()
    {
        string filePath = folderPath + "Character.json";

        Character.unLocks = new bool[] { true, false, false, false };
        string json = JsonUtility.ToJson(Character);
        File.WriteAllText(filePath, json);

        json = File.ReadAllText(filePath);
        Character = JsonUtility.FromJson<JsonDataCharacter>(json);
    }
}
