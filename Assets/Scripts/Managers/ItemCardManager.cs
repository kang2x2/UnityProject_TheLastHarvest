using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ItemCardManager
{
    public enum SelectType
    {
        LevelUp,
        Box,
        End
    }

    GameObject _itemCardRoot;

    List<Data_Item> _itemDatas = new List<Data_Item>();

    List<UI_ItemButton> _itemButtons = new List<UI_ItemButton>();
    HashSet<UI_ItemButton> selectButtons = new HashSet<UI_ItemButton>();

    // 플레이어의 아이템 슬롯을 참조하기 위함
    Survivor_Item[] _items = new Survivor_Item[(int)Define.ItemName.End];
    UI_ItemButton _healPackButton;
    public void Init()
    {
        _itemDatas.Clear();
        _itemButtons.Clear();

        Object[] assets = Resources.LoadAll("Scriptables/Items/Loads", typeof(Object));

        foreach (Object asset in assets)
        {
            if (asset as Data_Item != null)
            {
                _itemDatas.Add(asset as Data_Item);
            }
        }

        Transform player = Managers.GameManagerEx.Player.transform;

        _items[(int)Define.ItemName.Gun] = player.Find("Weapon_Gun").GetComponent<Weapon_Gun>();
        _items[(int)Define.ItemName.Thompson] = player.Find("Weapon_Thompson").GetComponent<Weapon_Thompson>();
        _items[(int)Define.ItemName.Shotgun] = player.Find("Weapon_Shotgun").GetComponent<Weapon_Shotgun>();
        _items[(int)Define.ItemName.Shovel] = player.Find("Weapon_Shovel").GetComponent<Weapon_Shovel>();
        _items[(int)Define.ItemName.Scythe] = player.Find("Weapon_Scythe").GetComponent<Weapon_Scythe>();
        _items[(int)Define.ItemName.Trident] = player.Find("Weapon_Trident").GetComponent<Weapon_Trident>();
        _items[(int)Define.ItemName.Shoose] = player.Find("Passive_Shoose").GetComponent<Passive_Shoose>();
        _items[(int)Define.ItemName.Margent] = player.Find("Passive_Margnet").GetComponent<Passive_Margent>();
        _items[(int)Define.ItemName.PowerCore] = player.Find("Passive_PowerCore").GetComponent<Passive_PowerCore>();
        _items[(int)Define.ItemName.ExpBoost] = player.Find("Passive_ExpBoost").GetComponent<Passive_ExpBoost>();
        _items[(int)Define.ItemName.MaxHp] = player.Find("Passive_MaxHp").GetComponent<Passive_MaxHp>();
        _items[(int)Define.ItemName.Recovery] = player.Find("Passive_Recovery").GetComponent<Passive_Recovery>();
        _items[(int)Define.ItemName.CriticalUp] = player.Find("Passive_Critical").GetComponent<Passive_Critical>();

        _itemCardRoot = GameObject.Find("@ItemCardRoot");
        if (_itemCardRoot == null)
        {
            _itemCardRoot = new GameObject { name = "@ItemCardRoot" };
        }

        GameObject.DontDestroyOnLoad(_itemCardRoot);

        for (int i = 0; i < _itemDatas.Count; ++i)
        {
            UI_ItemButton btn = Managers.ResourceManager.
                Instantiate("UI/PopUps/UI_ItemCard", _itemCardRoot.transform).GetComponent<UI_ItemButton>();

            switch (_itemDatas[i].itemName)
            {
                case Define.ItemName.Gun:
                    btn.Item = _items[(int)Define.ItemName.Gun];
                    break;
                case Define.ItemName.Thompson:
                    btn.Item = _items[(int)Define.ItemName.Thompson];
                    break;
                case Define.ItemName.Shotgun:
                    btn.Item = _items[(int)Define.ItemName.Shotgun];
                    break;
                case Define.ItemName.Shovel:
                    btn.Item = _items[(int)Define.ItemName.Shovel];
                    break;
                case Define.ItemName.Scythe:
                    btn.Item = _items[(int)Define.ItemName.Scythe];
                    break;
                case Define.ItemName.Trident:
                    btn.Item = _items[(int)Define.ItemName.Trident];
                    break;
                case Define.ItemName.Shoose:
                    btn.Item = _items[(int)Define.ItemName.Shoose];
                    break;
                case Define.ItemName.Margent:
                    btn.Item = _items[(int)Define.ItemName.Margent];
                    break;
                case Define.ItemName.PowerCore:
                    btn.Item = _items[(int)Define.ItemName.PowerCore];
                    break;
                case Define.ItemName.ExpBoost:
                    btn.Item = _items[(int)Define.ItemName.ExpBoost];
                    break;
                case Define.ItemName.MaxHp:
                    btn.Item = _items[(int)Define.ItemName.MaxHp];
                    break;
                case Define.ItemName.Recovery:
                    btn.Item = _items[(int)Define.ItemName.Recovery];
                    break;
                case Define.ItemName.CriticalUp:
                    btn.Item = _items[(int)Define.ItemName.CriticalUp];
                    break;
            }

            btn.ItemData = _itemDatas[i];
            btn.Init();

            _itemButtons.Add(btn);

            if(btn.ItemData.itemName == Define.ItemName.HealthPack)
            {
                _healPackButton = btn;
            }
        }
    }

    public void ItemCardSuffle(int SelectCount, Transform parent, SelectType type = SelectType.LevelUp)
    {
        selectButtons.Clear();

        Player player = Managers.GameManagerEx.Player.GetComponent<Player>();

        // 1. 유효 아이템 뽑기.
        List<UI_ItemButton> liveButtons = new List<UI_ItemButton>();
        foreach (UI_ItemButton button in _itemButtons)
        {
            button.transform.SetParent(_itemCardRoot.transform);
            button.gameObject.SetActive(false);

            if(type == SelectType.Box && button.ItemData.itemName == Define.ItemName.HealthPack)
            {
                continue;
            }

            if (button.IsLive == true)
            {
                // 소비 아이템들은 Item이 null.
                // Weapon의 경우엔 획득하지도 않은 무기의 스탯 카드를 선택하면 안되기에 검사가 필요하다.
                if (button.Item != null && button.Item.ItemType == Define.ItemType.Weapon)
                {
                    // 최초 획득한 Weapon인가?
                    if (button.ItemData.abilityType == Define.AbilityType.Init)
                    {
                        if (player.HasItem[(int)button.ItemData.itemName] == false)
                        {
                            liveButtons.Add(button);
                        }
                    }
                    else
                    {
                        if (player.HasItem[(int)button.ItemData.itemName] == true)
                        {
                            liveButtons.Add(button);
                        }
                    }
                }
                else
                {
                    liveButtons.Add(button);
                }
            }
        }

        if (liveButtons.Count > SelectCount)
        {
            while (true)
            {
                for (int i = 0; i < SelectCount; ++i)
                {
                    int ranIndex = Random.Range(0, liveButtons.Count);
                    selectButtons.Add(liveButtons[ranIndex]);
                }

                if (selectButtons.Count >= SelectCount)
                {
                    break;
                }

                selectButtons.Clear();
            }

        }
        else
        {
            foreach (UI_ItemButton button in liveButtons)
            {
                selectButtons.Add(button);
            }
        }

        if(type == SelectType.Box)
        {
            selectButtons.Add(_healPackButton);
        }

        // 2. 선택창 활성화
        foreach (UI_ItemButton button in selectButtons)
        {
            button.gameObject.SetActive(true);
            button.transform.SetParent(parent);
            button.transform.localScale = Vector3.one;
        }
    }

    public void CompletedSelect()
    {
        foreach (UI_ItemButton button in selectButtons)
        {
            button.gameObject.SetActive(false);
            button.transform.SetParent(_itemCardRoot.transform);
        }
    }
}
