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
    public bool IsClear { get; private set; }

    // Item
    public int HealpackCount { get; set; }
    public int BoxCount { get; set; }

    // Data
    public Data_Spawn SpawnData { get; private set; }
    public Data_Character PlayerData { get; set; }

    public GameObject Player { get; private set; }
    public BossMonster Boss { get; private set; }
    public Define.MapType MapType { get; set; }
    public Define.GameOverType GameOverType { get; set; }
    public int GameSpeedIndex { get; set; }
    float[] _gameSpeeds = new float[3] { 1.0f, 1.5f, 2.0f };
    public void Init()
    {
        Player = Managers.ResourceManager.Instantiate("Objects/Player");
        Managers.ItemCardManager.Init();

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
        IsBossBattle = false;
        IsClear = false;
        GameSpeedIndex = 0;
    }

    public void Update()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }

        if(ProgressTime >= 600.0f && IsBossBattle == false)
        {
            // 카메라 고정
            // if (CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera != null)
            // {
            //     CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.Follow = null;
            // }
        
            // 보스 생성
            switch(MapType)
            {
                case Define.MapType.Field:
                    Boss = Managers.ResourceManager.Instantiate("Objects/Boss_Rino").GetComponent<BossMonster>();
                    break;
                case Define.MapType.Cave:
                    Boss = Managers.ResourceManager.Instantiate("Objects/Boss_Skull").GetComponent<BossMonster>();
                    break;

            }
            IsBossBattle = true;
        }

        if(IsClear == false)
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

            if(IsClear == false)
            {
                Managers.UIManager.ShowPopUpUI("PopUpUI_LevelUp");
            }
        }

        if(IsBossBattle == true && Boss != null)
        {
            if(Boss.IsLive == false && IsClear == false)
            {
                IsClear = true;
                Managers.CoroutineManager.StartCoroutine(Survived());
            }
        }
    }

    IEnumerator Survived()
    {
        yield return new WaitForSeconds(2.0f);

        GameOverType = Define.GameOverType.Clear;
        // Survived UI Show
        Managers.UIManager.ShowPopUpUI("PopUpUI_GameOver");
    }

    public void GetExp(float expValue)
    {
        CurExp += expValue;
    }

    public void SetGameSpeed()
    {
        GameSpeedIndex += 1;
        if (GameSpeedIndex >= _gameSpeeds.Length)
        {
            GameSpeedIndex = 0;
        }

        Time.timeScale = _gameSpeeds[GameSpeedIndex];
    }

    public void Pause()
    {
        IsPause = true;
        Time.timeScale = 0.0f;
    }

    public void Continue()
    {
        IsPause = false;
        Time.timeScale = _gameSpeeds[GameSpeedIndex];
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
        GameSpeedIndex = 0;
        Time.timeScale = 1.0f;
    }
}
