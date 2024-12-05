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

    // 플레이어의 아이템 슬롯을 참조하기 위함
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

        // 슬롯 카운트보다 남아있는 아이템이 더 적으면 바로 삽입.
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
            // 1. 중복되지 않는 랜덤 인덱스들 뽑기
            while (true)
            {
                for (int i = 0; i < player.SelectBoxCount; ++i)
                {
                    int ranIndex = Random.Range(0, _itemButtons.Count);
                    UI_ItemButton button = _itemButtons[ranIndex];

                    // 이미 획득한 GetButton이거나 만렙까지 도달한 Button이면
                    if (button.IsLive == false)
                    {
                        continue;
                    }

                    // 선택한 아이템이 획득하지 않은 아이템이라면(능력치 업그레이드가 아닌 최초 획득)
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
                    // 이미 가지고 있는 아이템이라면(능력치 업그레이드 가능)
                    else
                    {
                        selectButtons.Add(button);
                    }
                }

                // 뽑은 개수가 slotCount가 아니라면
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

        // 2. 선택창 활성화
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
