using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Shovel : Weapon
{
    public override void Init()
    {
        Managers.PoolManager.CreatePool(Managers.ResourceManager.Load<GameObject>("Objects/Projectile_Shovel"), 8);

        // ������ ����� Json�� ����� data�� �ҷ����� ���.
        // TextAsset asset = Resources.Load<TextAsset>("Datas/Survivor/Survivor_Weapon_ShovelData");
        // _data = JsonUtility.FromJson<ShovelData>(asset.text);

        gameObject.SetActive(true);
        Weapon_Setting();
    }

    public override void LevelUp(Define.AbilityType type)
    {
        switch(type)
        {
            case Define.AbilityType.Speed:
                _speedLevel += 1;
                break;
            case Define.AbilityType.Amount:
                _amountLevel += 1;
                break;
            case Define.AbilityType.Attack:
                _attackLevel += 1;
                break;
        }

        Weapon_Setting();
    }

    void Update()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }
        transform.Rotate(Vector3.back * _itemData.speeds[_speedLevel] * Time.deltaTime);
    }

    void Weapon_Setting()
    {
        for(int i = 0; i < _itemData.amounts[_amountLevel]; ++i)
        {
            GameObject shovel = null;
            // ���ʿ��� ���� or Pop�� �����ϱ� ����
            if(i < transform.childCount)
            {
                shovel = transform.GetChild(i).gameObject;
            }
            else
            {
                shovel = Managers.ResourceManager.Instantiate("Objects/Projectile_Shovel");
            }

            shovel.transform.parent = transform;

            // ���� ���� �������� ��ġ�� �ʱ�ȭ ��. 
            // ���� ���� �ʱ�ȭ�� ���� ������ ���� ������ �������� ��ġ �ʴ� ����� ��Ÿ��.
            shovel.transform.localPosition = Vector3.zero;
            shovel.transform.localRotation = Quaternion.identity;

            Vector3 rot = Vector3.forward * 360 * i / (_amountLevel + 1); // _amountLevel�� ���� 0�̶� + 1
            shovel.transform.Rotate(rot);
            shovel.transform.Translate(shovel.transform.up * 1.25f, Space.World);

            float attackRatio = Managers.GameManagerEx.Player.GetComponent<Player>().AttackRatio;
            shovel.GetComponent<Projectile_Shovel>().Init(attackRatio * _itemData.attacks[_attackLevel]);
        }
    }
}
