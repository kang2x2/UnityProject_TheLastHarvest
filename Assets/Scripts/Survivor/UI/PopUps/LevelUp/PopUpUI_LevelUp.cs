using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopUpUI_LevelUp : UI_PopUp
{
    enum Scrollbars
    {
        ScrollbarVertical,
    }
    enum GameObjects
    {
        Content,
    }
    enum Buttons
    {
        ReRollItemButton,
        ReRollGoldButton,
    }
    enum Texts
    {
        HasReRollText,
        HasGoldText,
        NeedGoldText,
    }

    public Data_Item[] _itemDatas;
    public int LiveButtonCount { get; set; } = 0;

    List<UI_ItemButton> _initItemButtons = new List<UI_ItemButton>();
    List<UI_ItemButton> _itemButtons = new List<UI_ItemButton>();

    int _reRollPrice;

    // 플레이어의 아이템 슬롯을 참조하기 위함
    Survivor_Item[] _items = new Survivor_Item[(int)Define.ItemType.End];

    public override void Init()
    {
        UI_Bind<Scrollbar>(typeof(Scrollbars));
        UI_Bind<GameObject>(typeof(GameObjects));
        UI_Bind<Button>(typeof(Buttons));
        UI_Bind<Text>(typeof(Texts));

        UI_BindEvent(UI_Get<Button>((int)Buttons.ReRollItemButton).gameObject, ClickReRollButton_Item);
        UI_BindEvent(UI_Get<Button>((int)Buttons.ReRollGoldButton).gameObject, ClickReRollButton_Gold);

        _reRollPrice = 2;

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
        _items[(int)Define.ItemType.ExpBoost] = player.Find("Passive_ExpBoost").GetComponent<Passive_ExpBoost>();
        _items[(int)Define.ItemType.MaxHp] = player.Find("Passive_MaxHp").GetComponent<Passive_MaxHp>();
        _items[(int)Define.ItemType.Recovery] = player.Find("Passive_Recovery").GetComponent<Passive_Recovery>();

        for (int i = 0; i<_itemDatas.Length; ++i)
        {
            //UI_ItemButton btn = Managers.ResourceManager.Instantiate
            //    ("UI/PopUps/UI_ItemCard", UI_Get<Image>((int)Images.SelectAreaImage).transform).GetComponent<UI_ItemButton>();
            UI_ItemButton btn = Managers.ResourceManager.Instantiate
                  ("UI/PopUps/UI_ItemCard", UI_Get<GameObject>((int)GameObjects.Content).transform).GetComponent<UI_ItemButton>();
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
                case Define.ItemType.ExpBoost:
                    btn.Item = _items[(int)Define.ItemType.ExpBoost];
                    break;
                case Define.ItemType.MaxHp:
                    btn.Item = _items[(int)Define.ItemType.MaxHp];
                    break;
                case Define.ItemType.Recovery:
                    btn.Item = _items[(int)Define.ItemType.Recovery];
                    break;
            }

            btn.ItemData = _itemDatas[i];
            btn.Init();

            if (btn.ItemData.abilityType == Define.AbilityType.Init)
            {
                _initItemButtons.Add(btn);
            }

            _itemButtons.Add(btn);
        }

        LiveButtonCount = _itemButtons.Count;
    }

    public override void Show(object param = null)
    {
        Managers.SoundManager.PlaySFX("UISounds/LevelUp");
        Managers.GameManagerEx.Pause();

        GetRandomItems();
    }
    void GetRandomItems()
    {
        UI_Get<Scrollbar>((int)Scrollbars.ScrollbarVertical).value = 1;

        Vector2 downPos = new Vector2(0.0f, -1000.0f);
        UI_Get<GameObject>((int)GameObjects.Content).GetComponent<RectTransform>().localPosition = downPos;

        LiveButtonCount = 0;

        Player player = Managers.GameManagerEx.Player.GetComponent<Player>();


        foreach (UI_ItemButton button in _itemButtons)
        {
            button.gameObject.SetActive(false);
            if (button.IsLive == true)
            {
                LiveButtonCount += 1;
            }
        }

        HashSet<UI_ItemButton> selectButtons = new HashSet<UI_ItemButton>();

        // 슬롯 카운트보다 남아있는 아이템이 더 적으면 바로 삽입.
        if (LiveButtonCount <= player.SelectItemCount)
        {
            foreach (UI_ItemButton button in _itemButtons)
            {
                if (button.IsLive == true)
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
                for (int i = 0; i < player.SelectItemCount; ++i)
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
                if (selectButtons.Count < player.SelectItemCount)
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

    public void ClickReRollButton_Item(PointerEventData data)
    {
        Player player = Managers.GameManagerEx.Player.GetComponent<Player>();

        if (player.ReRollCount > 0)
        {
            Managers.SoundManager.PlaySFX("UISounds/SelectionComplete");

            player.ReRollCount -= 1;
            GetRandomItems();
        }
    }

    public void ClickReRollButton_Gold(PointerEventData data)
    {
        int gold = Managers.DataManager.User.Data.gold;
        if(gold >= _reRollPrice)
        {
            Managers.SoundManager.PlaySFX("UISounds/SelectionComplete");

            Managers.DataManager.User.Data.gold -= _reRollPrice;
            Managers.DataManager.User.UserDataOverwrite();
            _reRollPrice *= 2;
            GetRandomItems();
        }
    }

    private void LateUpdate()
    {
        Player player = Managers.GameManagerEx.Player.GetComponent<Player>();
        UI_Get<Text>((int)Texts.HasReRollText).text = player.ReRollCount.ToString();

        if (player.ReRollCount <= 0)
        {
            UI_Get<Button>((int)Buttons.ReRollItemButton).interactable = false;
            UI_Get<Button>((int)Buttons.ReRollItemButton).image.color = Color.gray;
        }
        else
        {
            UI_Get<Button>((int)Buttons.ReRollItemButton).interactable = true;
            UI_Get<Button>((int)Buttons.ReRollItemButton).image.color = Color.white;
        }


        int gold = Managers.DataManager.User.Data.gold;
        UI_Get<Text>((int)Texts.HasGoldText).text = gold.ToString();
        UI_Get<Text>((int)Texts.NeedGoldText).text = _reRollPrice.ToString();

        if(gold < _reRollPrice)
        {
            UI_Get<Button>((int)Buttons.ReRollGoldButton).interactable = false;
            UI_Get<Button>((int)Buttons.ReRollGoldButton).image.color = Color.gray;
        }
        else
        {
            UI_Get<Button>((int)Buttons.ReRollGoldButton).interactable = true;
            UI_Get<Button>((int)Buttons.ReRollGoldButton).image.color = Color.white;
        }
    }
}
