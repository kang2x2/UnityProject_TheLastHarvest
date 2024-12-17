using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Shovel : Weapon
{
    public override void Init()
    {
        base.Init();
        Managers.PoolManager.CreatePool(Managers.ResourceManager.Load<GameObject>("Objects/Projectile_Shovel"), 8);

        // 이전에 사용한 Json을 사용해 data를 불러오는 방식.
        // TextAsset asset = Resources.Load<TextAsset>("Datas/Survivor/Survivor_Weapon_ShovelData");
        // _data = JsonUtility.FromJson<ShovelData>(asset.text);

        gameObject.SetActive(true);
        Weapon_Setting();
    }

    public override void LevelUp(Define.AbilityType type)
    {
        base.LevelUp(type);

        Weapon_Setting();
    }

    void Update()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }
        transform.Rotate(Vector3.back * (float)_stats[(int)Define.AbilityType.Speed] * Time.deltaTime);
    }

    void Weapon_Setting()
    {
        for(int i = 0; i < (int)_stats[(int)Define.AbilityType.Amount]; ++i)
        {
            GameObject shovel = null;
            // 불필요한 생성 or Pop을 방지하기 위함
            if(i < transform.childCount)
            {
                shovel = transform.GetChild(i).gameObject;
            }
            else
            {
                shovel = Managers.ResourceManager.Instantiate("Objects/Projectile_Shovel");
            }

            shovel.transform.parent = transform;

            // 로컬 영역 기준으로 위치를 초기화 함. 
            // 로컬 영역 초기화를 하지 않으면 월드 공간을 기준으로 원치 않는 결과를 나타냄.
            shovel.transform.localPosition = Vector3.zero;
            shovel.transform.localRotation = Quaternion.identity;

            Vector3 rot = Vector3.forward * 360 * i / (int)_stats[(int)Define.AbilityType.Amount]; // _amountLevel은 최초 0이라 + 1
            shovel.transform.Rotate(rot);
            shovel.transform.Translate(shovel.transform.up * 1.25f, Space.World);

            float attackRatio = Managers.GameManagerEx.Player.GetComponent<Player>().AttackRatio;
            shovel.GetComponent<Projectile_Shovel>().Init(
                attackRatio * (float)_stats[(int)Define.AbilityType.Attack],
                _itemData.stat.knockbackPower);
        }
    }
}
