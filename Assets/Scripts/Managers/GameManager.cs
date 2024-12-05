using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    // GamePlay
    public bool IsPause { get; set; } = false;
    public float ProgressTime { get; private set; } = 0.0f;
    public int Kill { get; set; } = 0;
    public int GameLevel { get; private set; } = 0;
    public int ExpLevel { get; private set; } = 0;
    public int SpawnTypeLevel { get; private set; } = 0;
    public int SpawnTimeLevel { get; private set; } = 0;
    public float CurExp { get; private set; } = 0.0f;
    public float DestExp { get; private set; } = 5.0f;

    public bool IsBossBattle { get; private set; } = false;

    // Data
    public Data_Spawn SpawnData { get; private set; }
    public Data_Character PlayerData { get; set; }

    // Player And Map
    public GameObject Player { get; private set; }
    public Define.MapType MapType { get; set; }

    public void Init()
    {
        Player = Managers.ResourceManager.Instantiate("Objects/Player");

        switch (MapType)
        {
            case Define.MapType.Field:
                SpawnData = Resources.Load<Data_Spawn>("Scriptables/Spawn/Spawn_Field");
                Managers.ResourceManager.Instantiate("Tiles/FieldPal");
                break;
            case Define.MapType.Cave:
                SpawnData = Resources.Load<Data_Spawn>("Scriptables/Spawn/Spawn_Cave");
                Managers.ResourceManager.Instantiate("Tiles/CavePal");
                break;
        }

        Player.GetComponent<Player>().CharacterSetting(PlayerData);
    }

    public void Update()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }

        //if(ProgressTime >= 3.0f && IsBossBattle == false)
        //{
        //    // 카메라 고정
        //    if (CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera != null)
        //    {
        //        CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.Follow = null;
        //    }
        //
        //    // 보스 생성
        //    Managers.ResourceManager.Instantiate("Objects/Boss_Rino");
        //
        //    IsBossBattle = true;
        //}
        //else
        {
            ProgressTime += Time.deltaTime;
        }
    }

    public void LateUpdate()
    {
        if (CurExp >= DestExp)
        {
            GameLevel += 1;
            CurExp = 0;

            if (ExpLevel < SpawnData.destExpChangeLevels.Length)
            {
                if (GameLevel == SpawnData.destExpChangeLevels[ExpLevel])
                {
                    ExpLevel += 1;
                }
            }

            if (SpawnTypeLevel < SpawnData.spawnTypeChangeLevels.Length)
            {
                if (GameLevel == SpawnData.spawnTypeChangeLevels[SpawnTypeLevel])
                {
                    SpawnTypeLevel += 1;
                }
            }

            if (SpawnTimeLevel < SpawnData.spawnTimeChangeLevels.Length)
            {
                if (GameLevel == SpawnData.spawnTimeChangeLevels[SpawnTimeLevel])
                {
                    SpawnTimeLevel += 1;
                }
            }

            DestExp *= SpawnData.destExps[ExpLevel];

            Managers.UIManager.ShowPopUpUI("PopUpUI_LevelUp");
        }
    }

    public void GetExp(float expValue)
    {
        CurExp += expValue;
    }

    public void Pause()
    {
        IsPause = true;
        Time.timeScale = 0;
    }

    public void Continue()
    {
        IsPause = false;
        Time.timeScale = 1;
    }

    public void Clear()
    {
        ProgressTime = 0.0f;
        Kill = 0;
        GameLevel = 0;
        ExpLevel = 0;
        SpawnTypeLevel = 0;
        SpawnTimeLevel = 0;
        CurExp = 0.0f;
        DestExp = 5.0f;
    }
}
