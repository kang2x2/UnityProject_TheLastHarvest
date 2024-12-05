using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemButton : UI_Base
{
    enum Images
    {
        ItemImage,
    }

    enum Texts
    {
        LevelText,
        NameText,
        ItemDescText,
        AbilityDescText,
    }

    enum textType
    {
        Level,
        Name,
        ItemDesc,
        AbilityDesc,
        End
    }

    public Data_Item _itemData;
    public Data_Item ItemData { get { return _itemData; } set { _itemData = value; } }

    public Survivor_Item _item;
    public Survivor_Item Item { get { return _item; } set { _item = value; } }

    public bool IsLive { get; private set; } = true;

    int _level = 0;

    public override void Init()
    {
        UI_Bind<Image>(typeof(Images));
        UI_Bind<Text>(typeof(Texts));

        UI_Get<Image>((int)Images.ItemImage).sprite = _itemData.icon;

        UI_Get<Text>((int)Texts.NameText).text = _itemData.name;
        UI_Get<Text>((int)Texts.ItemDescText).text = _itemData.itemDesc;
        UI_Get<Text>((int)Texts.AbilityDescText).text = _itemData.abilityDesc;

        UI_BindEvent(gameObject, OnClick);
    }

    private void LateUpdate()
    {
        if(ItemData.abilityType != Define.AbilityType.Init)
        {
            string nextLevel = _level + 1 >= ItemData.maxLevel ? "Max" : "Lv." + (_level + 2).ToString();
            UI_Get<Text>((int)Texts.LevelText).text = $"Lv.{_level + 1} -> {nextLevel}";
        }
        else
        {
            UI_Get<Text>((int)Texts.LevelText).text = "½Å±Ô È¹µæ";
        }
    }

    public void OnClick(PointerEventData data)
    {
        Player player = Managers.GameManagerEx.Player.GetComponent<Player>();
        if (_itemData.abilityType == Define.AbilityType.Init)
        {
            _item.Init();
            player.HasItem[(int)_itemData.itemType] = true;
            IsLive = false;
        }
        else
        {
            _level += 1;
            _item.LevelUp(_itemData.abilityType);

            if (_level >= _itemData.maxLevel)
            {
                IsLive = false;
                GetComponent<Button>().interactable = false;
            }
        }
    }
}
