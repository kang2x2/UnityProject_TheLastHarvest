using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardManager
{
    GameObject _itemCardRoot;

    List<Data_Item> _itemDatas = new List<Data_Item>();
    public int LiveButtonCount { get; set; } = 0;

    List<UI_ItemButton> _itemButtons = new List<UI_ItemButton>();

    // �÷��̾��� ������ ������ �����ϱ� ����
    Survivor_Item[] _items = new Survivor_Item[(int)Define.ItemName.End];

    public void Init()
    {
        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets/Resources/SCriptables/Items" });

        foreach(string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            if (scriptableObject == null)
            {
                Debug.Log("Fail Found ScriptableObject: " + scriptableObject.name + " at " + path);
                return;
            }

            if(scriptableObject as Data_Item != null)
            {
                _itemDatas.Add(scriptableObject as Data_Item);
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

        _itemCardRoot = GameObject.Find("@ItemCardRoot");
        if (_itemCardRoot == null)
        {
            _itemCardRoot = new GameObject { name = "@ItemCardRoot" };
        }

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
            }

            btn.ItemData = _itemDatas[i];
            btn.Init();

            _itemButtons.Add(btn);
        }
    }

    public void ItemCardSuffle(int SelectCount, Transform parent, string parentUIName)
    {
        LiveButtonCount = 0;
        Player player = Managers.GameManagerEx.Player.GetComponent<Player>();

        // 1. ��ȿ ������ �̱�.
        List<UI_ItemButton> liveButtons = new List<UI_ItemButton>();
        foreach (UI_ItemButton button in _itemButtons)
        {
            button.transform.SetParent(_itemCardRoot.transform);
            button.gameObject.SetActive(false);
            if (button.IsLive == true)
            {
                // �Һ� �����۵��� Item�� null.
                // Weapon�� ��쿣 ȹ�������� ���� ������ ���� ī�带 �����ϸ� �ȵǱ⿡ �˻簡 �ʿ��ϴ�.
                if(button.Item != null && button.Item.ItemType == Define.ItemType.Weapon)
                {
                    // ���� ȹ���� Weapon�ΰ�?
                    if (button.ItemData.abilityType == Define.AbilityType.Init)
                    {
                        if(player.HasItem[(int)button.ItemData.itemName] == false)
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

        HashSet<UI_ItemButton> selectButtons = new HashSet<UI_ItemButton>();
        if (liveButtons.Count > SelectCount)
        {
            while(true)
            {
                for (int i = 0; i < SelectCount; ++i)
                {
                    int ranIndex = Random.Range(0, liveButtons.Count);
                    selectButtons.Add(liveButtons[ranIndex]);
                }

                if(selectButtons.Count >= SelectCount)
                {
                    break;
                }

                selectButtons.Clear();
            }

        }
        else
        {
            foreach(UI_ItemButton button in liveButtons)
            {
                selectButtons.Add(button);
            }
        }

        // 2. ����â Ȱ��ȭ
        foreach (UI_ItemButton button in selectButtons)
        {
            button.gameObject.SetActive(true);
            button.transform.SetParent(parent);
            button.ParentUIName = parentUIName;
            button.transform.localScale = Vector3.one;
        }
    }
}