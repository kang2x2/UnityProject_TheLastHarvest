using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopUpUI_LevelUp : UI_PopUp
{
    enum Images
    {
        BackGroundImage,
        SelectAreaImage
    }

    public Data_Item[] _itemDatas;

    List<UI_ItemButton> _initItemButtons = new List<UI_ItemButton>();
    List<UI_ItemButton> _itemButtons = new List<UI_ItemButton>();

    public int LiveButtonCount { get; set; } = 0;

    // �÷��̾��� ������ ������ �����ϱ� ����
    Survivor_Item[] _items = new Survivor_Item[(int)Define.ItemType.End];

    public override void Init()
    {
        UI_Bind<Image>(typeof(Images));

        Transform player = Managers.GameManagerEx.Player.transform;

        _items[(int)Define.ItemType.Gun] = player.Find("Weapon_Gun").GetComponent<Weapon_Gun>();
        _items[(int)Define.ItemType.Thompson] = player.Find("Weapon_Thompson").GetComponent<Weapon_Thompson>();
        _items[(int)Define.ItemType.Shotgun] = player.Find("Weapon_Shotgun").GetComponent<Weapon_Shotgun>();
        _items[(int)Define.ItemType.Shovel] = player.Find("Weapon_Shovel").GetComponent<Weapon_Shovel>();
        _items[(int)Define.ItemType.Scythe] = player.Find("Weapon_Scythe").GetComponent<Weapon_Scythe>();
        _items[(int)Define.ItemType.Trident] = player.Find("Weapon_Trident").GetComponent<Weapon_Trident>();
        _items[(int)Define.ItemType.Shoose] = player.Find("Passive_Shoose").GetComponent<Passive_Shoose>();
        _items[(int)Define.ItemType.Margent] = player.Find("Passive_Margnet").GetComponent<Passive_Margent>();
        _items[(int)Define.ItemType.PowerCore] = player.Find("Passive_PowerCore").GetComponent<Passive_PowerCore>();
        _items[(int)Define.ItemType.BoxUpgrade] = player.Find("Passive_BoxUpgrade").GetComponent<Passive_BoxUpgrade>();
        _items[(int)Define.ItemType.ExpBoost] = player.Find("Passive_ExpBoost").GetComponent<Passive_ExpBoost>();

        for (int i = 0; i<_itemDatas.Length; ++i)
        {
            UI_ItemButton btn = Managers.ResourceManager.Instantiate
                ("UI/PopUps/UI_ItemCard", UI_Get<Image>((int)Images.SelectAreaImage).transform).GetComponent<UI_ItemButton>();

            switch (_itemDatas[i].itemType)
            {
                case Define.ItemType.Gun:
                    btn.Item = _items[(int)Define.ItemType.Gun];
                    break;
                case Define.ItemType.Thompson:
                    btn.Item = _items[(int)Define.ItemType.Thompson];
                    break;
                case Define.ItemType.Shotgun:
                    btn.Item = _items[(int)Define.ItemType.Shotgun];
                    break;
                case Define.ItemType.Shovel:
                    btn.Item = _items[(int)Define.ItemType.Shovel];
                    break;
                case Define.ItemType.Scythe:
                    btn.Item = _items[(int)Define.ItemType.Scythe];
                    break;
                case Define.ItemType.Trident:
                    btn.Item = _items[(int)Define.ItemType.Trident];
                    break;
                case Define.ItemType.Shoose:
                    btn.Item = _items[(int)Define.ItemType.Shoose];
                    break;
                case Define.ItemType.Margent:
                    btn.Item = _items[(int)Define.ItemType.Margent];
                    break;
                case Define.ItemType.PowerCore:
                    btn.Item = _items[(int)Define.ItemType.PowerCore];
                    break;
                case Define.ItemType.BoxUpgrade:
                    btn.Item = _items[(int)Define.ItemType.BoxUpgrade];
                    break;
                case Define.ItemType.ExpBoost:
                    btn.Item = _items[(int)Define.ItemType.ExpBoost];
                    break;
            }

            btn.ItemData = _itemDatas[i];
            btn.Init();
            UI_BindEvent(btn.gameObject, Close);

            if (btn.ItemData.abilityType == Define.AbilityType.Init)
            {
                _initItemButtons.Add(btn);
            }

            _itemButtons.Add(btn);
        }

        LiveButtonCount = _itemButtons.Count;
    }

    public override void Show()
    {
        Managers.GameManagerEx.Pause();
        Player player = Managers.GameManagerEx.Player.GetComponent<Player>();

        LiveButtonCount = 0;

        foreach (UI_ItemButton button in _itemButtons)
        {
            button.gameObject.SetActive(false);
            if(button.IsLive == true)
            {
                LiveButtonCount += 1;
            }
        }

        HashSet<UI_ItemButton> selectButtons = new HashSet<UI_ItemButton>();

        // ���� ī��Ʈ���� �����ִ� �������� �� ������ �ٷ� ����.
        if (LiveButtonCount <= player.SelectBoxCount)
        {
            foreach(UI_ItemButton button in _itemButtons)
            {
                if(button.IsLive == true)
                {
                    selectButtons.Add(button);
                }
            }
        }
        else
        {
            // 1. �ߺ����� �ʴ� ���� �ε����� �̱�
            while (true)
            {
                for (int i = 0; i < player.SelectBoxCount; ++i)
                {
                    int ranIndex = Random.Range(0, _itemButtons.Count);
                    UI_ItemButton button = _itemButtons[ranIndex];

                    // �̹� ȹ���� GetButton�̰ų� �������� ������ Button�̸�
                    if (button.IsLive == false)
                    {
                        continue;
                    }

                    // ������ �������� ȹ������ ���� �������̶��(�ɷ�ġ ���׷��̵尡 �ƴ� ���� ȹ��)
                    if (player.HasItem[(int)button.ItemData.itemType] == false)
                    {
                        for (int j = 0; j < _initItemButtons.Count; ++j)
                        {
                            if (_initItemButtons[j].ItemData.itemType == button.ItemData.itemType)
                            {
                                selectButtons.Add(_initItemButtons[j]);
                            }
                        }
                    }
                    // �̹� ������ �ִ� �������̶��(�ɷ�ġ ���׷��̵� ����)
                    else
                    {
                        selectButtons.Add(button);
                    }
                }

                // ���� ������ slotCount�� �ƴ϶��
                if (selectButtons.Count < player.SelectBoxCount)
                {
                    selectButtons.Clear();
                }
                else
                {
                    break;
                }
            }
        }

        // 2. ����â Ȱ��ȭ
        foreach (UI_ItemButton button in selectButtons)
        {
            button.gameObject.SetActive(true);
        }
    }

    public void Close(PointerEventData data)
    {
        Managers.UIManager.ClosePopUpUI("PopUpUI_LevelUp");
        Managers.GameManagerEx.Continue();
    }
}
